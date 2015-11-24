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
		}
		#endregion

		#region Form/Control Methods
		private void AlwaysOnTopCheckBox_CheckedChanged(object sender, EventArgs e) // [ieee]
		{
			// This is the lazy way of doing it, since there's only the one form to worry about currently. Could be turned into an event if ever there are more TopMost forms.
			Application.OpenForms["GemCombiner"].TopMost = this.alwaysOnTopCheckBox.Checked;
		}

		private void BordersCheckBox_CheckedChanged(object sender, EventArgs e) => SettingsHandler.ChangeBorders(!this.bordersCheckBox.Checked);

		private void CloseButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void HellrageSkinButton_Click(object sender, EventArgs e) => SettingsHandler.ChangeSkin(Skin.Hellrages);

		private void HotkeyTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			e.Handled = true;
			e.SuppressKeyPress = true;
			this.closeButton.Focus(); // should force it to only use the 1 key when focus is lost

			// key pressed, now validate it
			if (this.usedKeys.Contains(e.KeyCode) || e.Shift || e.Control)
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

		private void Options_FormClosing(object sender, FormClosingEventArgs e)
		{
			// Modal forms are automatically hidden, not closed/disposed, so there's no need to cancel and hide here like there is on HelpForm. The FormClosing event still fires, however, so we have to check why it's closing and react appropriately.
			if (e.CloseReason != CloseReason.UserClosing)
			{
				SettingsHandler.BordersChanged -= this.ApplyBorders;
				SettingsHandler.SkinChanged -= this.ApplySkin;
			}
		}

		private void Options_Load(object sender, EventArgs e)
		{
			this.ApplySkin(null, null);
			this.ApplyBorders(null, null);
			SettingsHandler.SkinChanged += this.ApplySkin;
			SettingsHandler.BordersChanged += this.ApplyBorders;
			this.alwaysOnTopCheckBox.Checked = Settings.Default.TopMost;
			this.bordersCheckBox.Checked = !Settings.Default.Borderless;
			this.hotkeyTextBox.Text = SettingsHandler.HotkeyText;
		}

		private void Options_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(this.Handle, WmNclButtonDown, HtCaption, IntPtr.Zero);
			}
		}

		private void WinFormsSkinButton_Click(object sender, EventArgs e) => SettingsHandler.ChangeSkin(Skin.WindowsForms);
		#endregion

		#region Private Methods
		private void ApplyBorders(object sender, EventArgs e)
		{
			SettingsHandler.ApplyBorders(this);
			this.SetRecommendationVisible();
		}

		private void ApplySkin(object sender, EventArgs e)
		{
			SettingsHandler.ApplySkin(this);
			this.SetRecommendationVisible();
		}

		private void SetRecommendationVisible() => this.recommendedLabel.Visible = (Skin)Settings.Default.Skin == Skin.Hellrages ? !Settings.Default.Borderless : false;
		#endregion
	}
}