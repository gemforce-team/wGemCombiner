namespace WGemCombiner
{
	using System;
	using System.Drawing;
	using System.Resources;
	using System.Windows.Forms;
	using Properties;
	using Resources;
	using static NativeMethods;

	#region Public Enums
	public enum Skin
	{
		WindowsForms,
		Hellrages,
	}
	#endregion

	internal static class SettingsHandler
	{
		#region Static Constructor
		static SettingsHandler()
		{
			SetHotkeyText((Keys)Settings.Default.Hotkey);
		}
		#endregion

		#region Events
		public static event EventHandler BordersChanged;

		public static event EventHandler SkinChanged;
		#endregion

		#region Public Properties
		public static string HotkeyText { get; private set; }
		#endregion

		#region Public Methods
		public static void ApplyBorders(Form form)
		{
			if (form == null)
			{
				return;
			}

			form.FormBorderStyle = Settings.Default.Borderless ? FormBorderStyle.None : FormBorderStyle.Sizable;
		}

		public static void ApplySkin(Form form)
		{
			if (form == null)
			{
				return;
			}

			switch ((Skin)Settings.Default.Skin)
			{
				case Skin.Hellrages:
					ResourceManager resManager = Images.ResourceManager;
					form.BackgroundImage = (Image)resManager.GetObject(form.Name + "BG_Framed");
					ApplyHellragesSkin(form);
					break;

				case Skin.WindowsForms:
					form.BackgroundImage = null;
					ApplyWinFormsSkin(form);
					break;
			}
		}

		// Could have been a toggle, but in practice, it was easier to have the option to set it specifically.
		public static void ChangeBorders(bool newBorderless)
		{
			if (Settings.Default.Borderless != newBorderless)
			{
				Settings.Default.Borderless = newBorderless;
				BordersChanged?.Invoke(null, EventArgs.Empty);
			}
		}

		public static void ChangeSkin(Skin newSkin)
		{
			var intSkin = (int)newSkin;
			if (Settings.Default.Skin != intSkin)
			{
				Settings.Default.Skin = intSkin;
				SkinChanged?.Invoke(null, EventArgs.Empty);
			}
		}

		public static void SetHotkeyText(Keys keyData)
		{
			if (keyData.HasFlag(Keys.Alt))
			{
				HotkeyText = "Alt";
				return;
			}

			var converter = new KeysConverter();
			string keyText = converter.ConvertToString(keyData);

			// show the [ ] ~ etc keys instead of Oem<whatever>
			if (keyText.StartsWith("Oem", StringComparison.Ordinal))
			{
				uint nonVirtualKey = MapVirtualKey((uint)keyData, 2) & 0x7fffffff;
				char mappedChar = Convert.ToChar(nonVirtualKey);
				HotkeyText = mappedChar == '\0' ? keyText : mappedChar.ToString();
			}
			else
			{
				HotkeyText = keyText;
			}
		}
		#endregion

		#region Private Methods
		private static void ApplyHellragesSkin(Control controlToSkin)
		{
			// Changed to iterate over the Controls collection only once.
			foreach (Control control in controlToSkin.Controls)
			{
				if (control is TextBox || control is RichTextBox || control is ComboBox || control is ListBox || control is NumericUpDown)
				{
					control.BackColor = SystemColors.InactiveCaptionText;
					control.ForeColor = Color.SandyBrown;
					continue;
				}

				if (control is RadioButton || control is Label || control is CheckBox)
				{
					control.BackColor = Color.Transparent;
					control.ForeColor = Color.DarkOrange;
					continue;
				}

				var button = control as Button;
				if (button != null)
				{
					button.BackColor = SystemColors.InactiveCaptionText;
					button.ForeColor = Color.DarkOrange;
					button.BackgroundImageLayout = ImageLayout.Stretch;
					button.FlatStyle = FlatStyle.Popup;
					button.BackgroundImage = Images.ButtonsDark2;
					button.Cursor = Cursors.Hand;
					continue;
				}

				if (control is TableLayoutPanel)
				{
					control.BackColor = Color.Transparent; // SystemColors.InactiveCaptionText;
					ApplyHellragesSkin(control);
					// continue;
				}
			}
		}

		private static void ApplyWinFormsSkin(Control controlToSkin)
		{
			foreach (Control control in controlToSkin.Controls)
			{
				if (control is TextBox || control is RichTextBox || control is ComboBox || control is ListBox || control is NumericUpDown)
				{
					control.BackColor = SystemColors.Window;
					control.ForeColor = SystemColors.WindowText;
					continue;
				}

				if (control is RadioButton || control is Label || control is CheckBox)
				{
					control.BackColor = SystemColors.Control;
					control.ForeColor = SystemColors.ControlText;
					continue;
				}

				var button = control as Button;
				if (button != null)
				{
					button.BackColor = SystemColors.Control;
					button.ForeColor = SystemColors.ControlText;
					button.BackgroundImage = null;
					button.FlatStyle = FlatStyle.System;
					button.Cursor = Cursors.Default;
					continue;
				}

				if (control is Panel)
				{
					control.BackColor = SystemColors.Window;
					ApplyWinFormsSkin(control);
					// continue;
				}
			}
		}
		#endregion
	}
}