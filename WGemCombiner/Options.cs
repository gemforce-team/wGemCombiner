using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WGemCombiner
{
    public partial class Options : Form
    {

        private Keys[] usedKeys = { Keys.W, Keys.T, Keys.A, Keys.B, Keys.R, Keys.G, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9, Keys.Escape };
        
        Form1 parentForm1;
        HelpForm parentHelpForm;
        CombinePerformer CP;

        //Static properties allow to keep border\skin settings when closing and reopening windows
        static Skin currentSkin = Skin.WindowsForms;
        static bool hasBorder = true; 

        //Passing main and help form links so that i know whose functions to call
        internal Options(Form1 parentForm1,HelpForm parentHelpForm,CombinePerformer CP)
        {
            this.parentForm1 = parentForm1;
            this.parentHelpForm = parentHelpForm;
            this.CP = CP;
            InitializeComponent();
        }

        private void Options_Load(object sender, EventArgs e)
        {
            changeSkin(currentSkin);
            setBorder(hasBorder);
            alwaysOnTopCheckBox.Checked = parentForm1.TopMost;
            bordersCheckBox.Checked = hasBorder;
            if (currentSkin == Skin.Hellrages)
                recommendedLabel.Visible = hasBorder;
            hotkeyTextBox.Text = parentForm1.hotkeyText;
        }

        //When the help form is closed it's link points to a disposed object, we have to update our options form with a new one
        public void updateParentForm(Form form)
        {
            if (form is Form1)
                parentForm1 = (Form1)form;
            if (form is HelpForm)
                parentHelpForm = (HelpForm)form;
        }

        private void hellrageSkinButton_Click(object sender, EventArgs e)
        {
            parentForm1.changeSkin(Skin.Hellrages);
            parentHelpForm.changeSkin(Skin.Hellrages);
            this.changeSkin(Skin.Hellrages);
            recommendedLabel.Visible = hasBorder;
        }

        private void winFormsSkinButton_Click(object sender, EventArgs e)
        {
            parentForm1.changeSkin(Skin.WindowsForms);
            parentHelpForm.changeSkin(Skin.WindowsForms);
            this.changeSkin(Skin.WindowsForms);
            recommendedLabel.Visible = false;
        }

        #region WindowDrag      //This part allows you to drag the window around while holding it anywhere
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Options_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        #endregion


        public void changeSkin(Skin newSkin)
        {
            currentSkin = newSkin;
            FormSkinner.changeSkin(currentSkin, this);
        }

        private void toggleBorder()
        {
            if (hasBorder)
            {
                setBorder(false);
            }
            else
            {
                setBorder(true);
            }
        }

        private void setBorder(bool border)
        {
            if (!border)
            {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                hasBorder = false;
            }
            else
            {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
                hasBorder = true;
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void alwaysOnTopCheckBox_CheckedChanged(object sender, EventArgs e) //[ieee]
        {
            parentForm1.TopMost = alwaysOnTopCheckBox.Checked;
        }

        private void bordersCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (bordersCheckBox.Checked != hasBorder)
            {
                parentForm1.toggleBorder();
                parentHelpForm.toggleBorder();
                this.toggleBorder();

                //Check whether to show the "Recommended: "Off"!" Label
                if (currentSkin == Skin.Hellrages)
                    recommendedLabel.Visible = hasBorder;
                else
                    recommendedLabel.Visible = false;
            }
        }

        private void hotkeyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
            closeButton.Focus();//should force it to only use the 1 key when focus is lost

            //key pressed, now validate it
            if (Array.IndexOf(usedKeys, e.KeyCode) > -1 || e.Shift || e.Control)
            {
                MessageBox.Show("The hotkey '" + e.KeyCode.ToString() + "' is used by GemCraft.");//Ingame hotkeys
                hotkeyTextBox.Text = parentForm1.hotkeyText;
                return;
            }
            hotkeyTextBox.Text = "";
            /*
            System.Text.StringBuilder messageBoxCS = new System.Text.StringBuilder();
            messageBoxCS.AppendFormat("{0} = {1}", "Alt", e.Alt);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Control", e.Control);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Handled", e.Handled);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "KeyCode", e.KeyCode);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "KeyValue", e.KeyValue);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "KeyData", e.KeyData);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Modifiers", e.Modifiers);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Shift", e.Shift);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "SuppressKeyPress", e.SuppressKeyPress);
            messageBoxCS.AppendLine();
            MessageBox.Show(messageBoxCS.ToString(), "KeyDown Event");
            */
            var converter = new KeysConverter();
            string keyText = converter.ConvertToString(e.KeyData);
            
            if (e.Alt)
                hotkeyTextBox.Text = "ALT";

            if (keyText.Contains("Oem"))//show the [ ] ~ etc keys instead of Oem1
            {
                e.Handled = false;
                e.SuppressKeyPress = false;
            }
            else
                hotkeyTextBox.Text = keyText;

            //update actual keycode for the form to use
            parentForm1.hotkey = (int)e.KeyCode;
        }

        //Take control of the "Tab" key also
        private void hotkeyTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Tab:
                    e.IsInputKey = true;
                    break;
            }
        }

        private void hotkeyTextBox_TextChanged(object sender, EventArgs e)
        {
            //update static
            parentForm1.hotkeyText = hotkeyTextBox.Text;
        }
    }
}