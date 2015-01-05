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
    public partial class HelpForm : Form
    {
        List<string> helpMessages;
        List<string> helpTitles;
        int helpMessageShown;

        static bool hasBorder = true;
        static Skin currentSkin = Skin.WindowsForms;

        public HelpForm()
        {
            InitializeComponent();
            helpMessages = new List<string>();
            helpTitles = new List<string>();
            helpMessageShown = 0;
        }

        private void HelpForm_Load(object sender, EventArgs e)
        {
            fillHelpMessages();
            updateTextBox();
            changeSkin(currentSkin);
            setBorder(hasBorder);
        }

        // This is the only function you need to make changes to when adding new help pages
        private void fillHelpMessages()
        {
            addHelpMessage("Working with the combiner", "\nPaste gem combining equations or a parenthesis formula into the textbox, then click 'Get Instructions'.\n" +
                "Set the 'delay' to at LEAST as many miliseconds as a frame on GC2 is taking. I recommend at least 45-50 for no lag. " +
                "(It will usually work with much lower, but going any lower than the frame time will not actually speed up the process, " +
                "as the game will only do one step per frame.)\nTo further reduce lag disable the gem info tooltips (\".\")\n\n" +
                "To have the program perform the combining method:\n" +
                "-Place the base gem in the bottom-right inventory slot. Empty inventory up to as many slots as the combine requires.\n" +
                "-Click 'Combine'\n-Hover your cursor over the base gem.\n-Press the '9' key.\n" +
                "\nYou can cancel the combine by pressing the 'Escape' key. (You may have to hold it for a second with lag?) " +
                "Using a delay lower than GC2's frame time will cause input to be ahead of what you see happening, and so canceling may appear to not work. " +
                "If your cursor has stopped moving, the program has stopped.\n\n" +
                "To have the program combine a managem with Black/Orange gems (or other), place your Black/Orange gem in the base gem's slot, and a gem " +
                "that is the Black/Orange gem combined with the base gem. Then set 'Start at step' to 2. (First two steps are always duplicate and upgrade base gem.)\n\n" +
                "You can do any number of steps manually to get whatever mix you want, or to use several hitfarmed gems in the combine.\n\n" +
                "The displayed list of instructions should be pretty self-explanatory. 1A is bottom-right, 1C is bottom-left, 12C is top-left.");

            addHelpMessage("Speccing", "\nGem Combiner now supports speccing. To spec, place base gems of different color in 1A, 1B, etc, as many colors as needed.\n" +
                "Order of colors should be, starting at 1A: Orange, Yellow, Black, Red. (Then killgem and managem.)\n" +
                "Speccing provides another way of squeezing red out of a gem. Simply select your managem/killgem combine and replace one of the 'k' or 'm' with another " +
                "(valid) color's letter. Be sure you have your two gems in the proper slots, though!");

            addHelpMessage("Input Format", "\nExample of combining equations:\n(val = 1)\t0 = g1 orange\n(val = 2)\t1 = 0 + 0\n(val = 3)\t2 = 1 + 0" +
                        "\nWorks if you have first gem as '1' and second as '2', etc, as well." +
                        "\n\nExample of parenthesis formula:\n(2+1)+1\nor\n(1+0)+0\nor\n(2m+m)+m\nIf zeros are present they are treated as 1s and 1s as 2s.");

            addHelpMessage("Presets", "\nPreset schemes are now supported. To use one, first select a color + combine/spec from the top drop-down list.\n" +
                "After selecting a color + combine/spec, the second drop-down list will display the preset options. First number is number of base gems, " +
                "second number is the growth rate. Ones marked with a '-' are standard 2^n combines [over 8], and are NEVER the best for growth rate per cost. " +
                "(Only included for convenience if you want easily comparable gem costs.)" +
                "Selecting a preset will change the textbox's text. This text is used to combine, not the preset. Any changes to the text displayed will affect the combine." +
                "\n\nKg/Yellow and Mg/Orange combines are set to both use the same number of base gems, so that they can be used easily in amps. " + 
                "These are not the best combines by themselves." +
                "\n\nSorry, but not all presets are added in yet! Be patient.");


        }

        private void addHelpMessage(string title, string message)
        {
            helpTitles.Add(title);
            helpMessages.Add(message);
        }

        private void updateTextBox()
        {
            helpMessageTextBox.Clear();
            helpMessageTextBox.Text = helpMessages[helpMessageShown];
            helpTitleTextBox.Text = helpTitles[helpMessageShown];
        }

        public void changeSkin(Skin newSkin)
        {
            currentSkin = newSkin;
            FormSkinner.changeSkin(currentSkin, this);
        }

        public void toggleBorder()
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

        #region WindowDrag      //This part allows you to drag the window around while holding it anywhere
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void HelpForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        #endregion

        private void leftButton_Click(object sender, EventArgs e)
        {
            if (helpMessageShown > 0)
            {
                helpMessageShown--;
                updateTextBox();
            }
            else
            {
                helpMessageShown = helpMessages.Count - 1;
                updateTextBox();
            }
        }

        private void rightButton_Click(object sender, EventArgs e)
        {
            if (helpMessageShown == helpMessages.Count - 1)
            {
                helpMessageShown = 0;
                updateTextBox();
            }
            else
            {
                helpMessageShown++;
                updateTextBox();
            }
        }

        private void closeHelpButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
