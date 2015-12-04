namespace WGemCombiner
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Drawing;
	using System.Globalization;
	using System.IO;
	using System.Text;
	using System.Threading;
	using System.Windows.Forms;
	using Properties;
	using static Globals;
	using static Instruction;
	using static NativeMethods;

	public partial class GemCombiner : Form
	{
		#region Static Fields
		private static Dictionary<GemColors, string> gemEffectNames = new Dictionary<GemColors, string>()
		{
			[GemColors.Black] = "Bloodbound",
			[GemColors.Kill] = "Kill",
			[GemColors.Mana] = "Mana",
			[GemColors.Orange] = "Leech",
			[GemColors.Red] = "Chain Hit",
			[GemColors.Yellow] = "Critical Hit"
		};
		#endregion

		#region Constants
		private const int RidiculousInstructionCount = 200000;
		private const string CustomRecipePlaceholder = "Insert custom recipe here";
		#endregion

		#region Fields
		private HelpForm helpForm = new HelpForm();
		private Options optionsForm = new Options();
		private bool asyncWaiting = false;
		private Dictionary<string, RecipeCollection> recipes = new Dictionary<string, RecipeCollection>();
		private Stopwatch stopwatch = new Stopwatch();
		#endregion

		#region Constructors
		public GemCombiner()
		{
			this.AddResourceRecipe("leech");
			this.AddResourceRecipe("bbound");
			this.AddResourceRecipe("mgcomb-exact");
			this.AddTextFileRecipes(ExePath + @"\recipes.txt");
			this.InitializeComponent();
			this.SettingsHandler_BordersChanged(null, null);
			if ((Skin)Settings.Default.Skin == Skin.Hellrages)
			{
				this.SettingsHandler_SkinChanged(null, null);
			}

			CombinePerformer.StepComplete += this.CombinePerformer_StepComplete;
			SettingsHandler.SkinChanged += this.SettingsHandler_SkinChanged;
			SettingsHandler.BordersChanged += this.SettingsHandler_BordersChanged;
			this.TopMost = Settings.Default.TopMost;

			var cb = this.colorComboBox.Items;
			foreach (var key in this.recipes.Keys)
			{
				cb.Add(key);
			}

			this.colorComboBox.SelectedIndex = 0;
			CombinePerformer.Enabled = true;
		}
		#endregion

		#region Form/Control Methods
		private void ColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			var cb = this.combineComboBox.Items;
			cb.Clear();
			foreach (var item in this.recipes[this.colorComboBox.Text])
			{
				cb.Add(item.Gem.Title);
			}

			this.combineComboBox.SelectedIndex = 0; // Preselect the first in the box
		}

		private void CombineButton_Click(object sender, EventArgs e)
		{
			if (this.asyncWaiting)
			{
				return; // there was already a thread waiting for hotkey
			}

			while (GetAsyncKeyState((Keys)Settings.Default.Hotkey) != 0)
			{
				// MessageBox.Show("Key detection failed, or you were already holding hotkey. Try again.");
				Thread.Sleep(500);
			}

			this.combineButton.Text = "Press " + SettingsHandler.HotkeyText + " on A1"; // hotkey
			this.asyncWaiting = true;
			do
			{
				Application.DoEvents();
				Thread.Sleep(10);

				// [HR] Cancel before starting or if form is closing
				if (GetAsyncKeyState(Keys.Escape) != 0 || !CombinePerformer.Enabled)
				{
					this.combineButton.Text = "Combine";
					this.asyncWaiting = false;
					return;
				}
			}
			while (GetAsyncKeyState((Keys)Settings.Default.Hotkey) == 0);

			// User pressed hotkey
			this.asyncWaiting = false;
			CombinePerformer.SleepTime = (int)this.delayNumeric.Value;
			this.stopwatch.Reset();
			this.stopwatch.Start();
			this.combineProgressBar.Maximum = CombinePerformer.Instructions.Count;
			CombinePerformer.PerformCombine((int)this.stepNumeric.Value);

			// Combine finished
			this.combineProgressBar.Value = this.combineProgressBar.Minimum;
			this.GuessEta();
			this.combineButton.Text = "Combine";
			if (Settings.Default.AutoCombine)
			{
				Thread.Sleep(500); // guess give it 0.5sec before going again
				this.combineButton.PerformClick(); // guess it's finished, click the "combine" again
			}
		}

		private void CombineComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			var combine = this.recipes[this.colorComboBox.Text][this.combineComboBox.Text];
			this.CreateInstructions(combine);
			this.SetCustomRecipeContent(combine.Gem.Recipe());
			if (Settings.Default.AutoCombine)
			{
				this.combineButton.PerformClick(); // Auto-load the combine button so all u have to press is "9" over the gem
			}
		}

		private void DelayNumeric_ValueChanged(object sender, EventArgs e) => this.GuessEta();

		private void ExitButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void GemCombiner_FormClosing(object sender, FormClosingEventArgs e)
		{
			CombinePerformer.Enabled = false;
			Settings.Default.Save();
			CombinePerformer.StepComplete -= this.CombinePerformer_StepComplete;
			SettingsHandler.BordersChanged -= this.SettingsHandler_BordersChanged;
			SettingsHandler.SkinChanged -= this.SettingsHandler_SkinChanged;
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

		private void HelpButton_Click(object sender, EventArgs e)
		{
			this.helpForm.Show();
		}

		private void OptionsButton_Click(object sender, EventArgs e)
		{
			// Open modally or we can trigger the combine while setting the hotkey. Could be worked around in other ways, but it's unlikely that a user will want to leave the Options screen open for any reason.
			this.optionsForm.ShowDialog(this);
		}

		private void ParseRecipeButton_Click(object sender, EventArgs e)
		{
			var parsedText = this.recipeInputRichTextBox.Text;
			if (parsedText.Contains("-combine:"))
			{
				// Remove X-combine: tag
				int tagEnd = parsedText.IndexOf(':') + 1;
				parsedText = parsedText.Substring(tagEnd).Trim();
			}

			Combiner combine;
			try
			{
				combine = new Combiner(parsedText);
			}
			catch (ArgumentException ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (combine != null && combine.Gem != null)
			{
				this.CreateInstructions(combine);
			}
		}

		private void SlotLimitUpDown_ValueChanged(object sender, EventArgs e)
		{
			Combiner.SlotLimit = (int)this.slotLimitUpDown.Value;
		}

		private void StepNumeric_ValueChanged(object sender, EventArgs e)
		{
			var style = this.stepNumeric.Value == 1 ? FontStyle.Regular : FontStyle.Bold;
			this.stepNumeric.Font = new Font(this.stepNumeric.Font, style);
			this.stepLabel.Font = new Font(this.stepNumeric.Font, style);
			this.GuessEta();
		}
		#endregion

		#region Private Methods
		private void AddFromLines(IEnumerable<string> lines)
		{
			var recipe = new List<string>();
			foreach (var line in lines)
			{
				if (!line.StartsWith("#", StringComparison.Ordinal) && !line.StartsWith("//", StringComparison.Ordinal))
				{
					var trimmedLine = line.Trim();
					if (trimmedLine.Length == 0)
					{
						if (recipe.Count > 0)
						{
							this.AddRecipe(new Combiner(recipe));
							recipe.Clear();
						}
					}
					else if (line.Contains("="))
					{
						recipe.Add(line);
					}
					else
					{
						this.AddRecipe(new Combiner(trimmedLine));
					}
				}
			}

			if (recipe.Count > 0)
			{
				this.AddRecipe(new Combiner(recipe));
				recipe.Clear();
			}
		}

		private void AddResourceRecipe(string name)
		{
			var resourceName = "WGemCombiner.Resources.recipes." + name + ".txt";

			using (Stream stream = Assembly.GetManifestResourceStream(resourceName))
			using (StreamReader reader = new StreamReader(stream))
			{
				var file = reader.ReadToEnd().Replace("\r\n", "\n").Replace('\r', '\n');
				var lines = file.Split('\n');
				this.AddFromLines(lines);
			}
		}

		private void AddTextFileRecipes(string filename)
		{
			if (File.Exists(filename))
			{
				var lines = File.ReadAllLines(filename);
				this.AddFromLines(lines);
			}
		}

		private void AddRecipe(Combiner combine)
		{
			var gem = combine.Gem;
			string gemGroup;
			if (Settings.Default.UseColors)
			{
				gemGroup = gem.Color.ToString();
			}
			else if (!gemEffectNames.TryGetValue(gem.Color, out gemGroup))
			{
				gemGroup = "Other";
			}

			gemGroup += " " + (combine.Gem.IsSpec ? "Spec" : "Combine");
			if (!this.recipes.ContainsKey(gemGroup))
			{
				this.recipes[gemGroup] = new RecipeCollection();
			}

			if (!this.recipes[gemGroup].Contains(gem.Title))
			{
				// TODO: Consider some other method of checking if these truly are duplicates or not.
				// Ignores gems with identical CombineTitles. Conceivably, there could be two different combines with identical titles, but I think this is fairly unlikely.
				this.recipes[gemGroup].Add(combine);
			}
		}

		private void CombinePerformer_StepComplete(object sender, int step)
		{
			Application.DoEvents();
			if (GetAsyncKeyState(Keys.Escape) != 0)
			{
				CombinePerformer.CancelCombine = true;
			}

			this.combineButton.Text = step.ToString(CultureInfo.CurrentCulture);
			this.combineProgressBar.Value = step;
			this.GetRealEta(step);
		}

		private void CreateInstructions(Combiner combine)
		{
			try
			{
				var instructions = combine.CreateInstructions();
				if (instructions.Count > RidiculousInstructionCount)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Creating this gem in {0} slots would require an excessive number of steps ({1}).", Combiner.SlotLimit, instructions.Count));
				}

				this.resultLabel.Text = combine.Gem.DisplayInfo(false) + string.Format(CultureInfo.CurrentCulture, "\r\nSlots: {0}\r\nSteps: {1}", instructions.SlotsRequired, instructions.Count);
				this.baseGemsListBox.Items.Clear();

				var baseGems = new List<BaseGem>(combine.BaseGems);
				baseGems.Sort((g1, g2) => g1.Slot.CompareTo(g2.Slot));
				foreach (var gem in baseGems)
				{
					if (gem.OriginalSlot != Combiner.NotSlotted)
					{
						this.baseGemsListBox.Items.Add(SlotName(gem.OriginalSlot) + ": " + gem.Color.ToString());
					}
				}

				var sb = new StringBuilder();
				for (int i = 1; i <= instructions.Count; i++)
				{
					sb.AppendLine(i.ToString(CultureInfo.CurrentCulture) + ": " + instructions[i - 1].ToString());
				}

				this.instructionsTextBox.Text = sb.ToString();
				this.instructionsTextBox.SelectionStart = 0;
				this.stepNumeric.Minimum = instructions.Count == 0 ? 0 : 1;
				this.stepNumeric.Maximum = instructions.Count;

				CombinePerformer.Instructions = instructions;
				this.GuessEta();
			}
			catch (InvalidOperationException ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void FormatEta(TimeSpan eta)
		{
			string separator = CultureInfo.CurrentCulture.DateTimeFormat.TimeSeparator;
			string format = "h\\" + separator + "mm\\" + separator + "ss";
			this.combineProgressBar.Text = "ETA: " + eta.ToString(format, CultureInfo.CurrentCulture);
		}

		private void GetRealEta(int step)
		{
			var time = this.stopwatch.ElapsedMilliseconds;
			var eta = ((time * CombinePerformer.Instructions.Count) / step) - time;
			this.FormatEta(new TimeSpan(0, 0, 0, 0, (int)eta));
		}

		private void GuessEta()
		{
			// Overhead beyond the delay time is usually around 2.5-3ms, so be safe and use 3.
			double eta = CombinePerformer.Instructions == null ? 0 : ((double)this.delayNumeric.Value + 3) * (CombinePerformer.Instructions.Count - ((int)this.stepNumeric.Value - 1));
			this.FormatEta(new TimeSpan(0, 0, 0, 0, (int)eta));
		}

		private void SettingsHandler_BordersChanged(object sender, EventArgs e) => SettingsHandler.ApplyBorders(this);

		private void SettingsHandler_SkinChanged(object sender, EventArgs e)
		{
			SettingsHandler.ChangeFormSize(this);
			SettingsHandler.ApplySkin(this);
		}

		private void SetCustomRecipeContent(string text)
		{
			this.recipeInputRichTextBox.Text = text;
			this.recipeInputRichTextBox.Font = new Font(this.recipeInputRichTextBox.Font, this.recipeInputRichTextBox.Font.Style & ~FontStyle.Italic);
		}
		#endregion
	}
}
