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
		#region Constants
		private const int RidiculousInstructionCount = 200000;
		#endregion

		#region Static Fields
		private static Dictionary<GemColors, string> gemEffectNames = new Dictionary<GemColors, string>()
		{
			[GemColors.Black] = "Bloodbound",
			[GemColors.HitFarm] = "Hit Farm", // Do we need this here?
			[GemColors.Kill] = "Kill",
			[GemColors.Mana] = "Mana",
			[GemColors.Orange] = "Leech",
			[GemColors.Red] = "Chain Hit",
			[GemColors.Yellow] = "Crit Hit"
		};
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
			foreach (var file in new string[] { "bbound", "kgcomb", "kgcomb-bbound", "kgcomb-exact", "leech", "mgcomb", "mgcomb-exact", "mgcomb-leech" })
			{
				foreach (var combiner in GetResourceRecipes(file))
				{
					this.AddRecipe(combiner, GetListName(combiner.Gem));
				}
			}

			var orangeName = GetColorName(GemColors.Orange);
			foreach (var file in new string[] { "mgspec-exact", "mgspec-appr" })
			{
				this.AddRecipes3(GetResourceRecipes(file), "Mana Spec | Coeff Amps", orangeName + " Amps | Spec");
			}

			var yellowName = GetColorName(GemColors.Yellow);
			foreach (var file in new string[] { "kgspec-exact", "kgspec-appr", "kgspec-mgsexact",  "kgspec-kgssemi", "kgspec-mgsappr" })
			{
				this.AddRecipes3(GetResourceRecipes(file), "Kill Spec | Coeff Amps", yellowName + " Amps | Spec");
			}

			foreach (var file in new string[] { "GESkgspec-exact", "GESkgspec-appr", "GESkgspec-mgsexact", "GESkgspec-kgssemi", "GESkgspec-mgspec" })
			{
				this.AddRecipes3(GetResourceRecipes(file), "GES Spec  | Coeff Amps", "GES Amps  | Spec");
			}

			this.AddTextFileRecipes(ExePath + @"\recipes.txt");

			this.InitializeComponent();

			this.SettingsHandler_BordersChanged(null, null);
			if ((Skin)Settings.Default.Skin == Skin.Hellrages)
			{
				this.SettingsHandler_SkinChanged(null, null);
			}

#if !DEBUG
			this.testAllButton.Visible = false;
#endif

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
			if (Settings.Default.FirstTimeOpen)
			{
				this.helpForm.Show();
				Settings.Default.FirstTimeOpen = false;
			}
		}
		#endregion

		#region Form/Control Methods
		private void ColorComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			var cb = this.combineComboBox.Items;
			cb.Clear();
			foreach (var item in this.recipes[this.colorComboBox.Text])
			{
				cb.Add(item.Title);
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
				Thread.Sleep(200);
			}

			this.combineButton.Text = "Press " + SettingsHandler.HotkeyText + " on A1"; // hotkey
			this.asyncWaiting = true;
			do
			{
				Application.DoEvents();
				Thread.Sleep(50);

				// [HR] Cancel before starting or if form is closing
				if (GetAsyncKeyState(Keys.Escape) != 0 || !CombinePerformer.Enabled)
				{
					this.combineButton.Text = "Combine";
					this.asyncWaiting = false;
					return;
				}
			}
			while (GetAsyncKeyState((Keys)Settings.Default.Hotkey) == 0 || Control.ModifierKeys != Keys.None);

			// User pressed hotkey
			this.asyncWaiting = false;
			CombinePerformer.SleepTime = (int)this.delayNumeric.Value;
			this.stopwatch.Reset();
			this.stopwatch.Start();
			this.combineProgressBar.Maximum = CombinePerformer.Instructions.Count;
			// Don't combine if recipe is a simple g1
			if (this.stepNumeric.Value > 0)
			{
				CombinePerformer.PerformCombine((int)this.stepNumeric.Value);
			}

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
			if (combine.Gem != null)
			{
				this.recipeInputRichTextBox.Text = combine.Gem.Recipe();
				if (Settings.Default.AutoCombine)
				{
					this.combineButton.PerformClick(); // Auto-load the combine button so all u have to press is "9" over the gem
				}
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
			try
			{
				// This approach assumes that there will be relatively few comments compared to recipe lines, and that therefore bulk adding and then removing will be faster than adding one-by-one.
				var newLines = new List<string>(this.recipeInputRichTextBox.Lines);
				for (int i = newLines.Count - 1; i >= 0; i--)
				{
					var line = newLines[i].Trim();
					if (line.Length == 0 || line.StartsWith("#", StringComparison.CurrentCulture) || line.StartsWith("//", StringComparison.CurrentCulture))
					{
						newLines.RemoveAt(i);
					}
				}

				bool equations = false;
				foreach (var line in newLines)
				{
					if (line.Contains("="))
					{
						equations = true;
						break;
					}
				}

				if (!equations)
				{
					newLines = new List<string>(Combiner.EquationsFromParentheses(string.Join(string.Empty, newLines))); // Join rather than using newLines[0] in case someone uses line breaks for formatting
				}

				var combine = new Combiner(newLines);
				this.CreateInstructions(combine);
				if (((Control)sender).Tag == null)
				{
					this.recipeInputRichTextBox.Text = combine.Gem?.Recipe() ?? string.Empty;
				}
				else
				{
					this.recipeInputRichTextBox.Text = string.Join(Environment.NewLine, newLines);
				}
			}
			catch (ArgumentException ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

		private void TestAll_Click(object sender, EventArgs e)
		{
			foreach (var kvp in this.recipes)
			{
				foreach (var combine in kvp.Value)
				{
					var instructions = combine.CreateInstructions();
					try
					{
						instructions.Verify(combine.BaseGems);
					}
					catch (InvalidOperationException ex)
					{
						MessageBox.Show(ex.Message, "Verification failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}
			}

			MessageBox.Show("Testing complete!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
		#endregion

		#region Private Static Methods
		private static string GetColorName(GemColors gemColor)
		{
			string gemGroup;
			if (Settings.Default.UseColors)
			{
				return gemColor.ToString();
			}
			else if (!gemEffectNames.TryGetValue(gemColor, out gemGroup))
			{
				return "Other";
			}

			return gemGroup;
		}

		private static string GetListName(Gem gem) => GetColorName(gem.Color) + " " + gem.SpecWord;

		private static List<Combiner> GetResourceRecipes(string name)
		{
			var retval = new List<Combiner>();
			var resourceName = "WGemCombiner.Resources.recipes." + name + ".txt";

			using (Stream stream = Assembly.GetManifestResourceStream(resourceName))
			using (StreamReader reader = new StreamReader(stream))
			{
				var file = reader.ReadToEnd().Replace("\r\n", "\n");
				var fileRecipes = file.Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
				foreach (var recipe in fileRecipes)
				{
					var combiner = new Combiner(recipe.Split('\n'));
					var gem = combiner.Gem;
					combiner.Title = string.Format(
						CultureInfo.CurrentCulture,
						"{0:0000000}{1} {2:0.000000}",
						gem.Cost,
						IsPowerOfTwo(gem.Cost) ? "*" : " ",
						gem.Growth);
					retval.Add(combiner);
				}
			}

			return retval;
		}
		#endregion

		#region Private Methods
		private void AddRecipe(IEnumerable<string> recipe)
		{
			var combiner = new Combiner(recipe);
			var gem = combiner.Gem;
			combiner.Title = string.Format(CultureInfo.CurrentCulture, "{0:0000000}{1} {2:0.000000}", gem.Cost, IsPowerOfTwo(gem.Cost) ? "*" : " ", gem.Growth);
			this.AddRecipe(combiner, GetListName(combiner.Gem));
		}

		private void AddRecipe(Combiner combiner, string gemGroup)
		{
			if (!this.recipes.ContainsKey(gemGroup))
			{
				this.recipes[gemGroup] = new RecipeCollection();
			}

			if (!this.recipes[gemGroup].Contains(combiner.Title))
			{
				this.recipes[gemGroup].Add(combiner);
			}
		}

		private void AddRecipes(List<Combiner> combiners, string gemGroupMain, string gemGroupAmp)
		{
			for (var recipeCounter = 0; recipeCounter < combiners.Count; recipeCounter += 2)
			{
				var mainCombiner = combiners[recipeCounter];
				var ampCombiner = combiners[recipeCounter + 1];
				mainCombiner.Title += string.Format(CultureInfo.CurrentCulture, " (use with {0}-{1:000000})", gemGroupAmp, ampCombiner.Gem.Cost);
				ampCombiner.Title += string.Format(CultureInfo.CurrentCulture, " (use with {0}-{1:000000})", gemGroupMain, mainCombiner.Gem.Cost);
				this.AddRecipe(mainCombiner, gemGroupMain);
				this.AddRecipe(ampCombiner, gemGroupAmp);
			}
		}

		private void AddRecipes3(List<Combiner> combiners, string gemGroupMain, string gemGroupAmp)
		{
			for (var recipeCounter = 0; recipeCounter < combiners.Count; recipeCounter += 2)
			{
				var mainCombiner = combiners[recipeCounter];
				var ampCombiner = combiners[recipeCounter + 1];
				mainCombiner.Title = string.Format(CultureInfo.CurrentCulture, "{0:0000000}{1}   {2:0.0000} {3:0000}", mainCombiner.Gem.Cost, IsPowerOfTwo(mainCombiner.Gem.Cost) ? "*" : " ", mainCombiner.Gem.Growth, ampCombiner.Gem.Cost);
				ampCombiner.Title = string.Format(CultureInfo.CurrentCulture, "{0:0000000}      {1:0000}", ampCombiner.Gem.Cost, mainCombiner.Gem.Cost);
				this.AddRecipe(mainCombiner, gemGroupMain);
				this.AddRecipe(ampCombiner, gemGroupAmp);
			}
		}

		private void AddTextFileRecipes(string filename)
		{
			// TODO: Add sorting
			if (File.Exists(filename))
			{
				var lines = File.ReadAllLines(filename);
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
								this.AddRecipe(recipe);
								recipe.Clear();
							}
						}
						else if (line.Contains("="))
						{
							recipe.Add(line);
						}
						else
						{
							try
							{
								var equations = Combiner.EquationsFromParentheses(trimmedLine);
								this.AddRecipe(equations);
							}
							catch (ArgumentException ex)
							{
								MessageBox.Show(ex.Message, "Error in " + filename, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return;
							}
						}
					}
				}

				if (recipe.Count > 0)
				{
					this.AddRecipe(recipe);
					recipe.Clear();
				}
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

				this.resultLabel.Text = combine.Gem == null ? "Empty recipe" : combine.Gem.DisplayInfo + string.Format(CultureInfo.CurrentCulture, "\r\nSlots:  {0}{1}\r\nSteps:  {2}", instructions.SlotsRequired, instructions.WasCondensed ? "*" : string.Empty, instructions.Count);
				this.baseGemsListBox.Items.Clear();

				var baseGems = new List<BaseGem>(combine.BaseGems);
				baseGems.Sort((g1, g2) => g1.OriginalSlot.CompareTo(g2.OriginalSlot));
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
				this.instructionsTextBox.SelectionLength = 0;
				this.instructionsTextBox.SelectionStart = 0;
				this.instructionsTextBox.ScrollToCaret();
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
		#endregion
	}
}
