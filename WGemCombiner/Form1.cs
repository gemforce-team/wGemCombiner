﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Resources;

namespace WGemCombiner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            optionsForm = new Options(this, helpForm);
        }

        private bool loaded = false;

        static Skin currentSkin = Skin.WindowsForms;//[HR]
        static bool hasBorder = true;//[HR]

        List<List<byte[]>> presets;
        List<List<string>> presetNames;
        List<char[]> presetColors;

        private static Assembly assembly = Assembly.GetExecutingAssembly();

        HelpForm helpForm = new HelpForm(); //[HR]
        Options optionsForm; //[HR]

        private void Form1_Load(object sender, EventArgs e)
        {
            CP.StepComplete += CStepC;
            FormSkinner.changeSkin(currentSkin, this);

            SetPresets();
            colorComboBox.SelectedIndex = 0;

            loaded = true;
        }


        //[HR] changed the whole preset system
        private void SetPresets()
        {
            // Put presets in arrays for easy access
            presets = new List<List<byte[]>>();
            presetNames = new List<List<string>>();
            presetColors = new List<char[]>();

            colorComboBox.Items.Add("Custom");

            AddOrange();
            AddManaSpec();
            AddManaComb();
            AddYellow();
            AddKillSpec();
            AddKillComb();

            //System.Diagnostics.Stopwatch t = new System.Diagnostics.Stopwatch();
            //t.Start();
            SortPresets();
            //t.Stop();
            //this.Text = t.ElapsedMilliseconds.ToString();
        }
        private void AddOrange()
        {
            addPresetGroup("WGemCombiner.Resources.leech.leechPresets.resources"); // Link to the resource file with all the schemes
            colorComboBox.Items.Add("Orange Combine"); // Title of the relevant combobox item
            presetColors.Add(new char[] { 'o' });
        }
        private void AddYellow()
        {
            addPresetGroup("WGemCombiner.Resources.yellow.yellowPresets.resources"); // Link to the resource file with all the schemes
            colorComboBox.Items.Add("Yellow Combine"); // Title of the relevant combobox item
            presetColors.Add(new char[] { 'y' });
        }
        private void AddManaComb()
        {
            addPresetGroup("WGemCombiner.Resources.mgComb.mgCombinePresets.resources"); // Link to the resource file with all the schemes
            colorComboBox.Items.Add("Managem Combine"); // Title of the relevant combobox item
            presetColors.Add(new char[] { 'm' });
        }
        private void AddKillSpec()
        {
            addPresetGroup("WGemCombiner.Resources.kgSpec.kgSpecPresets.resources"); // Link to the resource file with all the schemes
            colorComboBox.Items.Add("Killgem Spec"); // Title of the relevant combobox item
            presetColors.Add(new char[] { 'y', 'b', 'r' });
        }
        private void AddManaSpec()
        {

            addPresetGroup("WGemCombiner.Resources.mgSpec.mgSpecPresets.resources");
            colorComboBox.Items.Add("Managem Spec");
            presetColors.Add(new char[] { 'o', 'b', 'r' });
        }
        private void AddKillComb()
        {
            addPresetGroup("WGemCombiner.Resources.kgComb.kgCombinePresets.resources"); // Link to the resource file with all the schemes
            colorComboBox.Items.Add("Killgem Combine"); // Title of the relevant combobox item
            presetColors.Add(new char[] { 'k' });
        }

        /// <summary>
        /// Loads all files specified in the passed resource.resx, creates two lists (names and schemes) and then injects them into the master lists
        /// This injections help get rid of manually specifying where to put new elements, the list just grows.
        /// </summary>
        /// <param name="embeddedResourceFullName">Full path to the embedded assembly resource file containing the schemes</param>
        private void addPresetGroup(string embeddedResourceFullName)
        {
            ResourceReader resourceReader = new ResourceReader(assembly.GetManifestResourceStream(embeddedResourceFullName));
            IDictionaryEnumerator resourceEnumerator = resourceReader.GetEnumerator();

            List<string> presetNamesToAdd = new List<string>();
            List<byte[]> presetsToAdd = new List<byte[]>();

            while (resourceEnumerator.MoveNext())
            {
                presetNamesToAdd.Add((string)resourceEnumerator.Key);
                presetsToAdd.Add((byte[])resourceEnumerator.Value);
            }

            // Inject the newly formed lists into the master lists
            presets.Add(new List<byte[]>(presetsToAdd));
            presetNames.Add(new List<string>(presetNamesToAdd));
            resourceReader.Close();
        }
        private void SortPresets()
        {
            // Get the result gems for each preset, then sort by cost.
            for (int i = 0; i < presets.Count; i++)
            {
                List<Gem> gList = new List<Gem>();
                for (int iG = 0; iG < presets[i].Count; iG++)
                {
                    gList.Add(CombinePerformer.LoadGem(presets[i][iG], presetColors[i]));
                    gList.Last().Grade = iG; // Track original index.
                }
                gList.Sort();
                List<byte[]> sortedPresets = new List<byte[]>();
                for (int iG = 0; iG < presets[i].Count; iG++)
                { // Sort
                    sortedPresets.Add(presets[i][gList[iG].Grade]);
                    // After that, re-name all the presets to show their cost and growth rate. (and if it's one of those lame standard 2^n ones)
                    presetNames[i][iG] = gList[iG].Cost.ToString().PadLeft(6, '0') + " (" +
                        Math.Round(gList[iG].Growth, 5).ToString().PadRight(7, '0') + ")";
                    if (Math.Log(gList[iG].Cost, 2) % 1.0 == 0 && gList[iG].Cost >= 8)
                        presetNames[i][iG] += "-";
                }
                presets[i] = sortedPresets;
            }
        }

        // Preset selection [W]
        private void colorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loaded)
                return;
            combineComboBox.Items.Clear();
            if (colorComboBox.SelectedIndex > 0)
            {
                combineComboBox.Items.AddRange(presetNames[colorComboBox.SelectedIndex - 1].ToArray());
                parenthesisRadioButton.Checked = true;
                equationsRadioButton.Enabled = false;
            }
            else
            {
                equationsRadioButton.Enabled = true;
            }
        }
        private void combineComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (colorComboBox.SelectedIndex > 0)
            {
                int cID = colorComboBox.SelectedIndex - 1;
                Gem g = CombinePerformer.LoadGem(presets[cID][combineComboBox.SelectedIndex], presetColors[cID]);
                formulaInputTextBox.Text = g.GetFullCombine();
            }
        }

        CombinePerformer CP = new CombinePerformer();

        private void getInstructionsButton_Click(object sender, EventArgs e)
        {
            if (formulaInputTextBox.Text.Length <= 1) return; //[HR]

            if (formulaInputTextBox.Text.Contains("-combine:"))
            { // Remove X-combine: tag
                int tagEnd = formulaInputTextBox.Text.IndexOf(':') + 1;
                formulaInputTextBox.Text = formulaInputTextBox.Text.Substring(tagEnd);
            }

            CP.SetMethod(formulaInputTextBox.Text, equationsRadioButton.Checked);
            resultInfoLabel.Text = GetGemInfo(CP.resultGem) + "\nSlots: " + CP.Slots_Required;

            instructionsListBox.Items.Clear();
            for (int i = 0; i < CP.inst.Count; i++)
            {
                Point p = CP.inst[i];
                if (p.Y == CombinePerformer.INST_DUPE)
                    instructionsListBox.Items.Add(i + ": Dupe " + SlotStr(p.X));
                else if (p.Y == CombinePerformer.INST_UPGR)
                    instructionsListBox.Items.Add(i + ": Upgr " + SlotStr(p.X));
                else if (p.Y < 0)
                    instructionsListBox.Items.Add(i + ": Move " + SlotStr(p.X) + "->" + SlotStr(-p.Y - 1));
                else
                    instructionsListBox.Items.Add(i + ": Comb " + SlotStr(p.X) + "+" + SlotStr(p.Y));
            }

            if (CP.Slots_Required > 36)
                MessageBox.Show("Modified instructions to use 36- slots. Might not actually work!");

            stepNumeric.Maximum = CP.inst.Count - 1;

            // TEMPORARY SAVE CODE, COMMENT THIS
            // SaveGem();
        }

        private string SlotStr(int slotID)
        {
            int row = slotID / 3;
            int column = slotID % 3;
            string C = "A";
            if (column == 1)
                C = "B";
            else if (column == 2)
                C = "C";

            return (row + 1) + C;
        }

        private string GetGemInfo(Gem g)
        {
            return "Grade +" + g.Grade + "\n" +
                ("Cost: " + g.Cost + "x\n") +
                //("Power: " + Math.Round(g.Power, 6) + "\n") +
                ("Growth: " + Math.Round(g.Growth, 6) + " "); // [ie] need accuracy to 6 for tests
                //("Damage: " + Math.Round(g.damage, 6) + "\n") +
                //("Leech: " + Math.Round(g.leech, 6) + "\n") +
                //("Crit: " + Math.Round(g.critMult, 6) + "\n") +
                //("Bbound: " + Math.Round(g.blood, 6) + "");
        }

        [DllImport("user32.dll")]
        static extern ushort GetAsyncKeyState(int vKey);
        private void combineButton_Click(object sender, EventArgs e)
        {
            if (GetAsyncKeyState((int)Keys.D9) != 0)
            { MessageBox.Show("Key detection failed, or you were already holding 9. Try again."); return; }
            combineButton.Text = "Press 9";
            CP.sleep_time = (int)delayNumeric.Value;
            do
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(10);
                if (GetAsyncKeyState((int)Keys.Escape) != 0)//[HR] Cancel before starting
                {
                    combineButton.Text = "Combine";
                    return;
                }
            } while (GetAsyncKeyState((int)Keys.D9) == 0);

            combineButton.Text = "Working...";
            CP.PerformCombine((int)stepNumeric.Value);

            if (!CP.cancel_Combine)
                combineButton.Text = "Combine";
        }
        private void CStepC(int stepID)
        {
            Application.DoEvents();
            if (GetAsyncKeyState((int)Keys.Escape) != 0)
                CP.cancel_Combine = true;

            combineButton.Text = stepID.ToString();
        }

        private void copyList_Click(object sender, EventArgs e)
        {
            string instStr = "";
            for (int i = 0; i < instructionsListBox.Items.Count; i++)
                instStr += instructionsListBox.Items[i] + "\n";
            if (instStr == "") return; //[HR]
            Clipboard.SetText(instStr);
        }

        // Display help
        private void helpButton_Click(object sender, EventArgs e)
        {
            if (!helpForm.Visible)
            {
                helpForm = new HelpForm();
                optionsForm.updateParentForm(helpForm);
            }
            helpForm.Show();
        }

        // Credits
        private void creditsLabel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Gem Combiner was made by Suuper.\n" +
                "Testing help: Hellrage, 12345ieee" +
                "\nProgramming help: \n" +
                "-12345ieee (some spec parsing, recipes help) \n" +
                "-Hellrage (some GUI tweaks, skin, small bugfixes)");
        }

        //[HR] from here down
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (helpForm.Visible)
                helpForm.Close();
            if (optionsForm.Visible)
                optionsForm.Close();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region WindowDrag      //This part allows you to drag the window around while holding it anywhere
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        #endregion

        private void optionsButton_Click(object sender, EventArgs e)
        {
            if (!optionsForm.Visible)
                optionsForm = new Options(this, helpForm);
            optionsForm.Show();
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
                this.Size = new Size(383, 349);
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                hasBorder = false;
            }
            else
            {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                hasBorder = true;
            }
        }

        // Function used to save recipes, normally disabled
        string path = "";
        private void SaveGem()
        {
            System.IO.File.WriteAllBytes(path + CP.resultGem.GetColor().ToString() + "col" + CP.resultGem.Cost + "C", CP.GetSave());
        }

    }
}
