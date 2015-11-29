namespace WGemCombiner
{
	using System;
	using System.IO;
	using System.Windows.Forms;

	public partial class NewGemTester : Form
	{
		public NewGemTester()
		{
			this.InitializeComponent();
		}

		private void Button1_Click(object sender, EventArgs e)
		{
			Combine combine = null;
			try
			{
				if (this.inputRichTextBox.Text.Length == 0)
				{
					this.inputRichTextBox.Text = @"C:\Users\rmorl\Documents\GitHubVisualStudio\wGemCombiner\WGemCombiner\test.txt";
				}

				if (this.inputRichTextBox.Text.EndsWith(".txt"))
				{
					combine = new Combine(File.ReadAllText(this.inputRichTextBox.Text));
				}
				else
				{
					combine = new Combine(this.inputRichTextBox.Text);
				}
			}
			catch (ArgumentException ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			if (combine != null)
			{
				this.textBox1.Clear();
				combine.GetInstructions();
				this.textBox1.AppendText(combine.Gem.DisplayInfo(true, combine.Instructions.SlotsRequired) + "\r\n\r\n");
				for (int i = 0; i < combine.Instructions.Count; i++)
				{
					this.textBox1.AppendText(string.Format("{0}: {1}{2}", i, combine.Instructions[i], Environment.NewLine));
				}

				this.textBox1.AppendText(Environment.NewLine + "Instruction count: " + combine.Instructions.Count);
				this.textBox1.AppendText(Environment.NewLine + "Slots required: " + combine.Instructions.SlotsRequired);
			}
		}
	}
}
