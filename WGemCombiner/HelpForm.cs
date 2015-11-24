namespace WGemCombiner
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Windows.Forms;
	using Resources;
	using static NativeMethods;

	public partial class HelpForm : Form
	{
		#region Fields
		private List<string> helpMessages = new List<string>();
		private List<string> helpTitles = new List<string>();
		private int helpMessageShown = 0;
		#endregion

		#region Constructors
		public HelpForm()
		{
			this.InitializeComponent();
		}
		#endregion

		#region Form/Control Methods
		// This part allows you to drag the window around while holding it anywhere
		private void CloseHelpButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void HelpForm_Load(object sender, EventArgs e)
		{
			this.ApplySkin(null, null);
			this.ApplyBorders(null, null);
			SettingsHandler.SkinChanged += this.ApplySkin;
			SettingsHandler.BordersChanged += this.ApplyBorders;
			this.FillHelpMessages();
			this.UpdateTextBox();
		}

		private void HelpForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				e.Cancel = true;
				this.Hide();
			}
			else
			{
				// This will normally not run, since the help form just falls out of scope when the main form closes, but in case there's some unexpected close method, make sure we detach from the event properly.
				SettingsHandler.BordersChanged -= this.ApplyBorders;
				SettingsHandler.SkinChanged -= this.ApplySkin;
			}
		}

		private void HelpForm_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(this.Handle, WmNclButtonDown, HtCaption, IntPtr.Zero);
			}
		}

		private void HelpMessageTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(e.LinkText);
		}

		private void LeftButton_Click(object sender, EventArgs e)
		{
			if (this.helpMessageShown > 0)
			{
				this.helpMessageShown--;
				this.UpdateTextBox();
			}
			else
			{
				this.helpMessageShown = this.helpMessages.Count - 1;
				this.UpdateTextBox();
			}
		}

		private void RightButton_Click(object sender, EventArgs e)
		{
			if (this.helpMessageShown == this.helpMessages.Count - 1)
			{
				this.helpMessageShown = 0;
				this.UpdateTextBox();
			}
			else
			{
				this.helpMessageShown++;
				this.UpdateTextBox();
			}
		}
		#endregion

		#region Private Methods
		private void AddHelpMessage(string title, string message, params object[] parameters)
		{
			this.helpTitles.Add(title);
			this.helpMessages.Add(string.Format(CultureInfo.CurrentCulture, message, parameters));
		}

		private void ApplyBorders(object sender, EventArgs e)
		{
			SettingsHandler.ApplyBorders(this);
		}

		private void ApplySkin(object sender, EventArgs e)
		{
			SettingsHandler.ApplySkin(this);
		}

		// This is the only function you need to make changes to when adding new help pages
		private void FillHelpMessages()
		{
			this.AddHelpMessage(Messages.HelpCombinerTitle, Messages.HelpCombinerMessage, SettingsHandler.HotkeyText);
			this.AddHelpMessage(Messages.HelpSpeccingTitle, Messages.HelpSpeccingMessage, SettingsHandler.HotkeyText);
			this.AddHelpMessage(Messages.HelpInputFormatTitle, Messages.HelpInputFormatMessage);
			this.AddHelpMessage(Messages.HelpPresetsTitle, Messages.HelpPresetsMessage);
#if DEBUG
			this.AddHelpMessage(Messages.HelpImportingPresetsTitle, Messages.HelpImportingPresetsMessage);
#endif
			var shortVersion = Application.ProductVersion;
			shortVersion = shortVersion.Substring(0, shortVersion.LastIndexOf('.'));
			this.AddHelpMessage(Messages.HelpCreditsTitle, Messages.HelpCreditsMessage, shortVersion);
		}

		private void UpdateTextBox()
		{
			this.helpMessageTextBox.Clear();
			this.helpMessageTextBox.Text = this.helpMessages[this.helpMessageShown];
			this.helpTitleTextBox.Text = this.helpTitles[this.helpMessageShown];
		}
		#endregion
	}
}