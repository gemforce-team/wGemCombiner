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
            if (hasBorder)
                recommendedLabel.Visible = true;
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

        private void toggleBorderButton_Click(object sender, EventArgs e)
        {
            parentForm1.toggleBorder();
            parentHelpForm.toggleBorder();
            this.toggleBorder();

            //Check whether to show the "Recommended: "Off"!" Label
            if (hasBorder)
            {
                toggleBorderLabel.Text = "On";
                if (currentSkin == Skin.Hellrages)
                    recommendedLabel.Visible = true;
            }
            else
            {
                toggleBorderLabel.Text = "Off";
                recommendedLabel.Visible = false;
            }
        }

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
    }
}
