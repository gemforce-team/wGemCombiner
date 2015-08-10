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

        Form1 parentForm1;

        public HelpForm(Form1 parentForm1)
        {
            this.parentForm1 = parentForm1; // keep track of the parent form
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
                "Set the 'delay' to at LEAST as many miliseconds as a frame on GC2 is taking. I recommend at least 45-50 for no lag " +
                "(it will usually work with much lower, but going any lower than the frame time will not actually speed up the process, " +
                "as the game will only do one step per frame).\n" +
                "Game gem info tooltips will be hidden and reshown after the combine (\".\"), be sure to have them activated before combining\n" +
                "\n" +
                "To have the program perform the combining method:\n" +
                "- Place the base gem in the bottom-right inventory slot. Empty inventory up to as many slots as the combine requires\n" +
                "- Choose the Combine Spec and Amount\n" +
                "- Hover your cursor over the base gem\n" +
                "- Press the '" + parentForm1.hotkeyText + "' key.\n" +
                "\n" +
                "You can cancel the combine by pressing the 'Escape' key (you may have to hold it for a second with lag). " +
                "Using a delay lower than GC2's frame time will cause input to be ahead of what you see happening, and so canceling may appear to not work. " +
                "If your cursor has stopped moving, the program has stopped.\n\n" +
                "The displayed list of instructions should be pretty self-explanatory. 1A is bottom-right, 1C is bottom-left, 12C is top-left.");

            addHelpMessage("Speccing", "\nGem Combiner now supports speccing.\n" +
                "To spec, place base gems of different color in 1A, 1B, etc, as many colors as needed.\n" +
                "\n" +
                "Order of colors should be, starting at 1A: Orange, Yellow, Black, Red (then killgem and managem).\n" +
                "\n" +
                "Speccing provides another way of squeezing red out of a gem. Simply select your managem/killgem combine and replace one of the 'k' or 'm' with another " +
                "(valid) color's letter. Be sure you have your two gems in the proper slots, though!\n" +
                "\n" +
                "EXAMPLE:\n" +
                "Pick Managem Spec\n" +
                "Choose the default 8 pattern\n" +
                "Put Orange grade 1 in 1A(bottom right slot)\n" +
                "Put Black or White gem grade 1 in 1B (bottom middle slot)\n" +
                "Put Red gem grade 1 in 1C (bottom left slot)\n" +
                "Press '" + parentForm1.hotkeyText + "' with your cursor over the Orange gem in 1A\n" +
                "Enjoy!");

            addHelpMessage("Input Format", "\nExample of combining equations:\n" +
                "(val = 1)\t0 = g1 orange\n(val = 2)\t1 = 0 + 0\n(val = 3)\t2 = 1 + 0\n" +
                "Works if you have first gem as '1' and second as '2', etc, as well.\n" +
                "\n" +
                "Example of parenthesis formula:\n" +
                "(2+1)+1\nor\n(1+0)+0\nor\n(2m+m)+m\n" +
                "If zeros are present they are treated as 1s and 1s as 2s.");

            addHelpMessage("Presets", "\nPreset schemes are supported. To use one, first select a color + combine/spec from the top drop-down list.\n" +
                "After selecting a color + combine/spec, the second drop-down list will display the preset options. First number is number of base gems, " +
                "second number is the growth rate. Ones marked with a '-' are 2^n combines [over 8], and are NEVER the best for growth rate per cost " +
                "(included for convenience if you want easily comparable gem costs)." +
                "Selecting a preset will change the textbox's text. This text is used to combine, not the preset. Any changes to the text displayed will affect the combine.\n" +
                "\n" +
                "Kg/Yellow and Mg/Orange combines are set to both use the same number of base gems, so that they can be used easily for gem and amps. " +
                "Note that the preset combines are optimized for different combines between gem and amps, the optimal combines for a whole setup aren't included," +
                "but will be very similar to the gem ones");

#if DEBUG
            addHelpMessage("Importing new presets","\n In debug mode you can import new presets from a .txt file with recipes.\n"+
                "Your txt must contain one recipe per line."+
                "The recipes have to be valid parenthesis formulas (not equations). The parser is somewhat robust but i wouldn't test it too hard :)\n"+
                "To start importing, choose \"Import...\" option from the Preset drop-down, then choose the .txt file with recipes");
#endif

            addHelpMessage("Credits", "\nGem Combiner was made by Suuper.\n" +
                "\n" +
                "Contributors:\n" +
                "- 12345ieee (some spec parsing, preset recipes)\n" +
                "- Hellrage (some GUI tweaks, skin, small bugfixes, preset and resource management)\n" +
                "- CooLTanG (Automate the 'Get Instructions' & 'Combine' steps, fix window size/resolution issues)\n" +
                "\n" +
                "Release version: "+ releaseVersion + "\n" +
                "\n" +
                "To report a problem, go to\n" +
                "https://github.com/gemforce-team/wGemCombiner/issues");
        }
        string releaseVersion = "1.0.8";

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
