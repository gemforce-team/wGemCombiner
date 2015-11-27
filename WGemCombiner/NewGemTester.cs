namespace WGemCombiner
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Windows.Forms;

	public partial class NewGemTester : Form
	{
		public NewGemTester()
		{
			this.InitializeComponent();
		}

		private void Button1_Click(object sender, EventArgs e)
		{
			var combine = new Combine(@"
				0=o
				1=0+0
				2=1+1
				3=1+0
				4=3+0
				5=4+0
				6=5+0
				7=6+0
				8=7+1
				9=8+2");
			var instructions = combine.GetInstructions();
			foreach (var instruction in instructions)
			{
				this.textBox1.AppendText(instruction.ToString() + Environment.NewLine);
			}

			this.textBox1.AppendText(Environment.NewLine + "Slots required: " + instructions.SlotsRequired);
		}
	}
}
