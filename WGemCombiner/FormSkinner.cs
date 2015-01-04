using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.Resources;

namespace WGemCombiner
{
    static class FormSkinner
    {
        public static void changeSkin(Skin newSkin, Control controlToSkin)
        {
            switch (newSkin)
            {
                case Skin.Hellrages:
                    applyHellragesSkin(controlToSkin);
                    break;

                case Skin.WindowsForms:
                    applyWinFormsSkin(controlToSkin);
                    break;
            }
        }

        private static void applyHellragesSkin(Control controlToSkin)
        {
            if (controlToSkin is Form)
            {
                ResourceManager resManager = WGemCombiner.Resources.Graphics.Graphics.ResourceManager;//new ResourceManager("WGemCombiner.Resources.Graphics", Assembly.GetExecutingAssembly());
                controlToSkin.BackgroundImage = (Image)resManager.GetObject(controlToSkin.Name + "BG_Framed");
                controlToSkin.BackgroundImageLayout = ImageLayout.Stretch;
            }

            foreach (Button button in controlToSkin.Controls.OfType<Button>())
            {
                button.BackColor = SystemColors.InactiveCaptionText;
                button.ForeColor = Color.DarkOrange;
                button.BackgroundImageLayout = ImageLayout.Stretch;
                button.FlatStyle = FlatStyle.Popup;
                button.BackgroundImage = WGemCombiner.Resources.Graphics.Graphics.ButtonsDark2;
                button.Cursor = Cursors.Hand;
            }

            foreach (TextBox textBox in controlToSkin.Controls.OfType<TextBox>())
            {
                textBox.BackColor = SystemColors.InactiveCaptionText;
                textBox.ForeColor = Color.SandyBrown;
            }

            foreach (RichTextBox richTextBox in controlToSkin.Controls.OfType<RichTextBox>())
            {
                richTextBox.BackColor = SystemColors.InactiveCaptionText;
                richTextBox.ForeColor = Color.SandyBrown;
            }

            foreach (Panel panel in controlToSkin.Controls.OfType<Panel>())
            {
                panel.BackColor = SystemColors.InactiveCaptionText;
                changeSkin(Skin.Hellrages, panel);
            }

            foreach (ComboBox comboBox in controlToSkin.Controls.OfType<ComboBox>())
            {
                comboBox.BackColor = SystemColors.InactiveCaptionText;
                comboBox.ForeColor = Color.SandyBrown;
            }

            foreach (RadioButton radioButton in controlToSkin.Controls.OfType<RadioButton>())
            {
                radioButton.ForeColor = Color.DarkOrange;
                radioButton.BackColor = Color.Transparent;
            }

            foreach (Label label in controlToSkin.Controls.OfType<Label>())
            {
                label.BackColor = Color.Transparent;
                label.ForeColor = Color.DarkOrange;
            }

            foreach (ListBox listBox in controlToSkin.Controls.OfType<ListBox>())
            {
                listBox.ForeColor = Color.SandyBrown;
                listBox.BackColor = SystemColors.InactiveCaptionText;
            }

            foreach (NumericUpDown numericUpDown in controlToSkin.Controls.OfType<NumericUpDown>())
            {
                numericUpDown.BackColor = SystemColors.InactiveCaptionText;
            }

            foreach(CheckBox checkBox in controlToSkin.Controls.OfType<CheckBox>())
            {
                checkBox.BackColor = Color.Transparent;
                checkBox.ForeColor = Color.DarkOrange;
            }

        }

        private static void applyWinFormsSkin(Control controlToSkin)
        {
            foreach (Button button in controlToSkin.Controls.OfType<Button>())
            {
                button.BackColor = SystemColors.Control;
                button.ForeColor = SystemColors.ControlText;
                button.BackgroundImage = null;
                button.FlatStyle = FlatStyle.System;
                button.Cursor = Cursors.Default;
            }

            foreach (TextBox textBox in controlToSkin.Controls.OfType<TextBox>())
            {
                textBox.BackColor = SystemColors.Window;
                textBox.ForeColor = SystemColors.WindowText;
            }

            foreach (RichTextBox richTextBox in controlToSkin.Controls.OfType<RichTextBox>())
            {
                richTextBox.BackColor = SystemColors.Window;
                richTextBox.ForeColor = SystemColors.WindowText;
            }

            foreach (Panel panel in controlToSkin.Controls.OfType<Panel>())
            {
                panel.BackColor = SystemColors.Window;
                changeSkin(Skin.WindowsForms, panel);
            }

            foreach (ComboBox comboBox in controlToSkin.Controls.OfType<ComboBox>())
            {
                comboBox.BackColor = SystemColors.Window;
                comboBox.ForeColor = SystemColors.WindowText;
            }

            foreach (RadioButton radioButton in controlToSkin.Controls.OfType<RadioButton>())
            {
                radioButton.ForeColor = SystemColors.WindowText;
                radioButton.ForeColor = SystemColors.WindowText;
            }

            foreach (Label label in controlToSkin.Controls.OfType<Label>())
            {
                label.BackColor = SystemColors.Control;
                label.ForeColor = SystemColors.ControlText;
            }

            foreach (ListBox listBox in controlToSkin.Controls.OfType<ListBox>())
            {
                listBox.ForeColor = SystemColors.WindowText;
                listBox.BackColor = SystemColors.Window;
            }

            foreach (NumericUpDown numericUpDown in controlToSkin.Controls.OfType<NumericUpDown>())
            {
                numericUpDown.BackColor = SystemColors.Window;
            }

            foreach (CheckBox checkBox in controlToSkin.Controls.OfType<CheckBox>())
            {
                checkBox.BackColor = SystemColors.Control;
                checkBox.ForeColor = SystemColors.ControlText;
            }

            controlToSkin.BackgroundImage = null;

        }
    }
}
