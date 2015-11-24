namespace WGemCombiner
{
	using System;
	using System.Drawing;
	using System.Globalization;
	using System.Text;
	using System.Threading;
	using System.Windows.Forms;
	using Properties;
	using static Instruction;
	using static Localization;
	using static NativeMethods;

	public partial class GemCombiner : Form
	{
		#region Fields
		private HelpForm helpForm = new HelpForm();
		private Options optionsForm = new Options();
		private CombinePerformer combinePerformer = new CombinePerformer(true);
		private bool asyncWaiting = false;
		#endregion

		#region Constructors
		public GemCombiner()
		{
			this.InitializeComponent();
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
				var preset = (Preset)this.colorComboBox.SelectedItem;
				this.combineComboBox.Items.Clear();
				foreach (var entry in preset.Entries)
				{
					this.combineComboBox.Items.Add(entry);
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

			this.combineButton.Text = "Press " + SettingsHandler.HotkeyText + " on A1"; // hotkey
			this.combinePerformer.SleepTime = (int)this.delayNumeric.Value;
			this.asyncWaiting = true;
			do
			{
				Application.DoEvents();
				Thread.Sleep(10);

				// [HR] Cancel before starting or if form is closing
				if (GetAsyncKeyState(Keys.Escape) != 0 || this.IsDisposed)
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
				var gem = (Gem)this.combineComboBox.SelectedItem;
				var result = gem.GetFullCombine();
				this.formulaInputRichTextBox.Text = result;
				this.GetInstructionsButton_Click(this, EventArgs.Empty); // Auto-load instructions, so u don't have to even press that button
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

		private void DelayNumeric_ValueChanged(object sender, EventArgs e)
		{
			// quick fix to make sure its use even if combine is already pressed
			this.combinePerformer.SleepTime = (int)this.delayNumeric.Value;
		}

		private void ExitButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void GemCombiner_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.combinePerformer.Enabled = false;
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
			var cb = this.colorComboBox.Items;
			cb.Add("Custom");
			cb.Add(new Preset("o", "Orange Combine", "WGemCombiner.Resources.orange.orangePresets.resources"));
			cb.Add(new Preset("obr", "Mana Gem Spec", "WGemCombiner.Resources.mgSpec.mgSpecPresets.resources"));
			cb.Add(new Preset("m", "Mana Gem Combine", "WGemCombiner.Resources.mgComb.mgCombPresets.resources"));
			cb.Add(new Preset("y", "Yellow Combine", "WGemCombiner.Resources.yellow.yellowPresets.resources"));
			cb.Add(new Preset("ybr", "Kill Gem Spec", "WGemCombiner.Resources.kgSpec.kgSpecPresets.resources"));
			cb.Add(new Preset("k", "Kill Gem Combine", "WGemCombiner.Resources.kgComb.kgCombPresets.resources"));
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

		private void GetInstructionsButton_Click(object sender, EventArgs e)
		{
			var parsedText = this.formulaInputRichTextBox.Text;
			if (sender == this)
			{
				// If we know this is from a preset, skip pre-parsing
				this.combinePerformer.SetMethod(parsedText);
			}
			else
			{
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
			}

			this.combinePerformer.ChangeLastDestination(Slot1A);
			if (this.combinePerformer.ResultGem == null)
			{
				return; // this happens when the input formula is invalid
			}

			this.resultLabel.Text = this.combinePerformer.ResultGem.DisplayInfo(false, this.combinePerformer.SlotsRequired);
			this.baseGemsTextBox.Text = this.combinePerformer.BaseGemText;
			this.instructionsListBox.Items.Clear();
			var items = this.instructionsListBox.Items;
			var instructions = this.combinePerformer.Instructions;
			for (int i = 0; i < instructions.Count; i++)
			{
				items.Add(CurrentCulture($"{i}: {instructions[i]}"));
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
	}
}
