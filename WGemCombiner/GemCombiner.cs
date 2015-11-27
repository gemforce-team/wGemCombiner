namespace WGemCombiner
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Globalization;
	using System.IO;
	using System.Text;
	using System.Threading;
	using System.Windows.Forms;
	using Properties;
	using static Instruction;
	using static NativeMethods;

	public partial class GemCombiner : Form
	{
		#region Fields
		private HelpForm helpForm = new HelpForm();
		private Options optionsForm = new Options();
		private CombinePerformer combinePerformer = new CombinePerformer(false);
		private bool asyncWaiting = false;
		private Dictionary<string, RecipeCollection> recipes = new Dictionary<string, RecipeCollection>();
		#endregion

		#region Constructors
		public GemCombiner()
		{
			this.InitializeComponent();
			this.testButton.Visible = Type.GetType("WGemCombiner.NewGemTester") != null;
		}
		#endregion

		#region Form/Control Methods
		private void ColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.colorComboBox.SelectedIndex == 0)
			{
				this.combineComboBox.Items.Clear();
			}
#if DEBUG
			else if (this.colorComboBox.SelectedIndex == this.colorComboBox.Items.Count - 1)
			{
				this.importFileDialog.ShowDialog();
				RecipeConverter.ConvertFromFile(this.importFileDialog.FileName);
			}
#endif
			else
			{
				var cb = this.combineComboBox.Items;
				cb.Clear();
				foreach (var item in this.recipes[this.colorComboBox.Text])
				{
					cb.Add(item.CombineTitle);
				}

				this.combineComboBox.SelectedIndex = 0; // Preselect the first in the box
			}
		}

		private void CombineButton_Click(object sender, EventArgs e)
		{
			if (this.asyncWaiting)
			{
				return; // there was already a thread waiting for hotkey
			}

			if (GetAsyncKeyState((Keys)Settings.Default.Hotkey) != 0)
			{
				////MessageBox.Show("Key detection failed, or you were already holding hotkey. Try again.");
				Thread.Sleep(500);
				this.combineButton.PerformClick(); // ignore holding hotkey error and try again.
				return;
			}

			if (this.combinePerformer == null)
			{
				this.parseRecipeButton.PerformClick();
			}

			this.combineButton.Text = "Press " + SettingsHandler.HotkeyText + " on A1"; // hotkey
			this.asyncWaiting = true;
			do
			{
				Application.DoEvents();
				Thread.Sleep(10);

				// [HR] Cancel before starting or if form is closing
				if (GetAsyncKeyState(Keys.Escape) != 0 || this.combinePerformer == null)
				{
					this.combineButton.Text = "Combine";
					this.asyncWaiting = false;
					return;
				}
			}
			while (GetAsyncKeyState((Keys)Settings.Default.Hotkey) == 0);

			// User pressed hotkey
			this.asyncWaiting = false;
			this.combineButton.Text = "Working...";
			this.combinePerformer.SleepTime = (int)this.delayNumeric.Value;
			this.combinePerformer.PerformCombine((int)this.stepNumeric.Value);
			if (!this.combinePerformer.CancelCombine)
			{
				this.combineButton.Text = "Combine";
				Thread.Sleep(500); // guess give it 0.5sec before going again
				this.combineButton.PerformClick(); // guess its finished, click the "combine" again
			}
		}

		private void CombineComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.colorComboBox.SelectedIndex > 0)
			{
				var gem = this.recipes[this.colorComboBox.Text][this.combineComboBox.Text];
				var result = gem.GetFullCombine();
				this.formulaInputRichTextBox.Text = result;
				this.ParseRecipeButton_Click(this, EventArgs.Empty); // Auto-load instructions, so u don't have to even press that button
				this.combineButton.PerformClick(); // Auto-load the combine button so all u have to press is "9" over the gem
			}
		}

		private void CopyList_Click(object sender, EventArgs e)
		{
			if (this.instructionsListBox.Items.Count > 0)
			{
				var sb = new StringBuilder();
				foreach (var instruction in this.instructionsListBox.Items)
				{
					sb.AppendLine((string)instruction);
				}

				Clipboard.SetText(sb.ToString());
			}
		}

		private void ExitButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void GemCombiner_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.combinePerformer = null;
			Settings.Default.Save();
			SettingsHandler.BordersChanged -= this.ApplyBorders;
			SettingsHandler.SkinChanged -= this.ApplySkin;
		}

		private void GemCombiner_Load(object sender, EventArgs e)
		{
			this.combinePerformer.StepComplete += this.CombinePerformer_StepComplete;
			this.ApplySkin(null, null);
			this.ApplyBorders(null, null);
			SettingsHandler.SkinChanged += this.ApplySkin;
			SettingsHandler.BordersChanged += this.ApplyBorders;
			this.TopMost = Settings.Default.TopMost;
			var cb = this.colorComboBox.Items;
			cb.Add("Custom");
			this.AddResourceRecipes();
			// this.AddTextFileRecipes();
			foreach (var key in this.recipes.Keys)
			{
				cb.Add(key);
			}
#if DEBUG
			cb.Add("Import...");
#endif
			this.colorComboBox.SelectedIndex = 0;
			this.combinePerformer.Enabled = true;
		}

		private void GemCombiner_MouseDown(object sender, MouseEventArgs e)
		{
			// This part allows you to drag the window around while holding it anywhere
			if (e.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(this.Handle, WmNclButtonDown, HtCaption, IntPtr.Zero);
			}
		}

		private void ParseRecipeButton_Click(object sender, EventArgs e)
		{
			var parsedText = this.formulaInputRichTextBox.Text;
			if (parsedText.Contains("-combine:"))
			{
				// Remove X-combine: tag
				int tagEnd = parsedText.IndexOf(':') + 1;
				parsedText = parsedText.Substring(tagEnd).Trim();
			}

			if (parsedText.Length <= 1)
			{
				return; // [HR]
			}

			this.combinePerformer.Parse(parsedText);
			this.combinePerformer.ChangeLastDestination(Slot1A);
			if (this.combinePerformer.ResultGem == null)
			{
				return; // this happens when the input formula is invalid
			}

			this.resultLabel.Text = this.combinePerformer.ResultGem.DisplayInfo(false, this.combinePerformer.SlotsRequired);
			this.baseGemsListBox.Items.Clear();
			int ngem = 0;
			foreach (var gem in this.combinePerformer.BaseGems)
			{
				this.baseGemsListBox.Items.Add(SlotName(ngem) + ": " + gem.ColorName);
				ngem++;
			}

			this.instructionsListBox.Items.Clear();
			var items = this.instructionsListBox.Items;
			var instructions = this.combinePerformer.Instructions;
			for (int i = 0; i < instructions.Count; i++)
			{
				items.Add(string.Format(CultureInfo.CurrentCulture, "{0}: {1}", i, instructions[i]));
			}

			this.stepNumeric.Maximum = this.combinePerformer.Instructions.Count - 1;
		}

		private void HelpButton_Click(object sender, EventArgs e)
		{
			this.helpForm.Show();
		}

		private void OptionsButton_Click(object sender, EventArgs e)
		{
			// Open modally or we can trigger the combine while setting the hotkey. Could be worked around in other ways, but it's unlikely that a user will want to leave the Options screen open for any reason.
			this.optionsForm.ShowDialog(this);
		}

		private void StepNumeric_ValueChanged(object sender, EventArgs e)
		{
			var style = this.stepNumeric.Value == 0 ? FontStyle.Regular : FontStyle.Bold;
			this.stepNumeric.Font = new Font(this.stepNumeric.Font, style);
			this.stepLabel.Font = new Font(this.stepNumeric.Font, style);
		}
		#endregion

		#region Private Methods
		private void AddResourceRecipes()
		{
			var recipeDictionary = this.recipes;
			recipeDictionary["Orange Combine"] = new RecipeCollection(Preset.ReadResources("WGemCombiner.Resources.orange.orangePresets.resources", "o"));
			recipeDictionary["Mana Gem Spec"] = new RecipeCollection(Preset.ReadResources("WGemCombiner.Resources.mgSpec.mgSpecPresets.resources", "obr"));
			recipeDictionary["Mana Gem Combine"] = new RecipeCollection(Preset.ReadResources("WGemCombiner.Resources.mgComb.mgCombPresets.resources", "m"));
			recipeDictionary["Yellow Combine"] = new RecipeCollection(Preset.ReadResources("WGemCombiner.Resources.yellow.yellowPresets.resources", "y"));
			recipeDictionary["Kill Gem Spec"] = new RecipeCollection(Preset.ReadResources("WGemCombiner.Resources.kgSpec.kgSpecPresets.resources", "ybr"));
			recipeDictionary["Kill Gem Combine"] = new RecipeCollection(Preset.ReadResources("WGemCombiner.Resources.kgComb.kgCombPresets.resources", "k"));
		}

		private void AddTextFileRecipes()
		{
			foreach (var filename in Directory.EnumerateFiles(".", "*.txt", SearchOption.TopDirectoryOnly))
			{
				var lines = File.ReadAllLines(filename);
				var recipe = new StringBuilder();
				int lineNum = 0;
				foreach (var line in lines)
				{
					lineNum++;
					if (!line.StartsWith("#", StringComparison.Ordinal) && !line.StartsWith("//", StringComparison.Ordinal))
					{
						var trimmedLine = line.Trim();
						if (trimmedLine.Length == 0)
						{
							if (recipe.Length > 0)
							{
								this.combinePerformer.Parse(recipe.ToString());
								recipe.Clear();
							}
						}
						else if (line.Contains("="))
						{
							recipe.AppendLine(line);
						}
						else
						{
							this.combinePerformer.Parse(trimmedLine);
						}
					}
				}
			}
		}

		private void ApplyBorders(object sender, EventArgs e)
		{
			SettingsHandler.ApplyBorders(this);
		}

		private void ApplySkin(object sender, EventArgs e)
		{
			SettingsHandler.ApplySkin(this);
		}

		private void CombinePerformer_StepComplete(object sender, int stepID)
		{
			Application.DoEvents();
			if (GetAsyncKeyState(Keys.Escape) != 0)
			{
				this.combinePerformer.CancelCombine = true;
			}

			this.combineButton.Text = stepID.ToString(CultureInfo.CurrentCulture);
		}
		#endregion

		private void TestButton_Click(object sender, EventArgs e)
		{
			try
			{
				var testForm = Activator.CreateInstance(Type.GetType("WGemCombiner.NewGemTester")) as Form;
				testForm.Show();
			}
			catch (Exception ex)
			{
				MessageBox.Show("This button is for testing only and will not work for anyone else.");
			}
		}
	}
}
