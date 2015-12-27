namespace WGemCombiner
{
	using System;
	using System.Collections.Generic;
	using System.Windows.Forms;
	using Properties;
	using static NativeMethods;

	public partial class Options : Form
	{
		#region Fields
		private SortedSet<Keys> usedKeys = new SortedSet<Keys>()
		{
			Keys.B, Keys.T, Keys.A, Keys.W, Keys.R, Keys.G, Keys.Q, Keys.Space, Keys.N, Keys.X, Keys.D, Keys.U, Keys.Tab, Keys.OemPeriod, Keys.P, Keys.Escape,
			Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6,
			Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9,
		};
		#endregion

		#region Constructors
		internal Options()
		{
			this.InitializeComponent();
			this.SettingsHandler_BordersChanged(null, null);
			if ((Skin)Settings.Default.Skin == Skin.Hellrages)
			{
				this.SettingsHandler_SkinChanged(null, null);
			}

			this.SetRecommendationVisible();
			SettingsHandler.SkinChanged += this.SettingsHandler_SkinChanged;
			SettingsHandler.BordersChanged += this.SettingsHandler_BordersChanged;
		}
		#endregion

		#region Form/Control Methods
		private void AlwaysOnTopCheckBox_CheckedChanged(object sender, EventArgs e) // [ieee]
		{
			// This is the lazy way of doing it, since there's only the one form to worry about currently. Could be turned into an event if ever there are more TopMost forms.
			Application.OpenForms["GemCombiner"].TopMost = this.alwaysOnTopCheckBox.Checked;
			Settings.Default.TopMost = this.alwaysOnTopCheckBox.Checked;
		}

		private void AutoCombineCheckBox_CheckedChanged(object sender, EventArgs e) => Settings.Default.AutoCombine = this.autoCombineCheckBox.Checked;

		private void BordersCheckBox_CheckedChanged(object sender, EventArgs e) => SettingsHandler.ChangeBorders(!this.bordersCheckBox.Checked);

		private void CloseButton_Click(object sender, EventArgs e) => this.Close();

		private void HellrageSkinButton_CheckedChanged(object sender, EventArgs e) => SettingsHandler.ChangeSkin(Skin.Hellrages);

		private void HidePanelsCheckBox_CheckedChanged(object sender, EventArgs e) => Settings.Default.HidePanels = this.hidePanelsCheckBox.Checked;

		private void HotkeyTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			e.Handled = true;
			e.SuppressKeyPress = true;
			this.closeButton.Focus(); // should force it to only use the 1 key when focus is lost

			// key pressed, now validate it
			if (this.usedKeys.Contains(e.KeyCode) || e.Shift || e.Control || e.Alt)
			{
				MessageBox.Show("The hotkey '" + e.KeyCode.ToString() + "' is used by GemCraft."); // In-game hotkeys
			}
			else
			{
				// update actual keycode for the form to use
				Settings.Default.Hotkey = (int)e.KeyCode;
				SettingsHandler.SetHotkeyText(e.KeyData);
			}

			this.hotkeyTextBox.Text = SettingsHandler.HotkeyText;
		}

		// Take control of the "Tab" key also
		private void HotkeyTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Tab:
					e.IsInputKey = true;
					break;
			}
		}

		private void IEFix_CheckChanged(object sender, EventArgs e)
		{
			Settings.Default.IEFix = this.ieFixCheckBox.Checked;
		}

		private void Options_FormClosing(object sender, FormClosingEventArgs e)
		{
			// Modal forms are automatically hidden, not closed/disposed, so there's no need to cancel and hide here like there is on HelpForm. The FormClosing event still fires, however, so we have to check why it's closing and react appropriately.
			if (e.CloseReason != CloseReason.UserClosing)
			{
				SettingsHandler.BordersChanged -= this.SettingsHandler_BordersChanged;
				SettingsHandler.SkinChanged -= this.SettingsHandler_SkinChanged;
			}
		}

		private void Options_Load(object sender, EventArgs e)
		{
			this.bordersCheckBox.Checked = !Settings.Default.Borderless;
			this.alwaysOnTopCheckBox.Checked = Settings.Default.TopMost;
			this.autoCombineCheckBox.Checked = Settings.Default.AutoCombine;
			this.hellrageSkinButton.Checked = (Skin)Settings.Default.Skin == Skin.Hellrages;
			this.hidePanelsCheckBox.Checked = Settings.Default.HidePanels;
			this.hotkeyTextBox.Text = SettingsHandler.HotkeyText;
			this.ieFixCheckBox.Checked = Settings.Default.IEFix;
			this.useColorsButton.Checked = Settings.Default.UseColors;
			this.useEffectsButton.Checked = !Settings.Default.UseColors;
			this.winFormsSkinButton.Checked = (Skin)Settings.Default.Skin == Skin.WindowsForms;
		}

		private void Options_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(this.Handle, WmNclButtonDown, HtCaption, IntPtr.Zero);
			}
		}

		private void WinFormsButton_CheckChanged(object sender, EventArgs e) => SettingsHandler.ChangeSkin(Skin.WindowsForms);
		#endregion

		#region Private Methods
		private void SetRecommendationVisible() => this.recommendedLabel.Visible = (Skin)Settings.Default.Skin == Skin.Hellrages ? !Settings.Default.Borderless : false;

		private void SettingsHandler_BordersChanged(object sender, EventArgs e)
		{
			SettingsHandler.ApplyBorders(this);
			this.SetRecommendationVisible();
		}

		private void SettingsHandler_SkinChanged(object sender, EventArgs e)
		{
			SettingsHandler.ChangeFormSize(this);
			SettingsHandler.ApplySkin(this);
			this.SetRecommendationVisible();
		}

		private void UseColorsButton_CheckedChanged(object sender, EventArgs e) => Settings.Default.UseColors = true;

		private void UseEffectsButton_CheckedChanged(object sender, EventArgs e) => Settings.Default.UseColors = false;
		#endregion
	}
}