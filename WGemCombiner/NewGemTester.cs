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
				// var combine = new Combine(File.ReadAllText(@"C:\Users\rmorl\Documents\GitHubVisualStudio\wGemCombiner\WGemCombiner\test.txt"));
				combine = new Combine(this.textBoxInput.Text);
			}
			catch (ArgumentException ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			if (combine != null)
			{
				this.textBox1.Clear();
				var instructions = combine.GetInstructions();
				foreach (var instruction in instructions)
				{
					this.textBox1.AppendText(instruction.ToString() + Environment.NewLine);
				}

				this.textBox1.AppendText(Environment.NewLine + "Instruction count: " + instructions.Count);
				this.textBox1.AppendText(Environment.NewLine + "Slots required: " + instructions.SlotsRequired);
			}
		}
	}
}
