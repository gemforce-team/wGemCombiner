namespace WGemCombiner
{
	using System;
	using System.Collections.Generic;
	using System.Windows.Forms;

	public partial class NewGemTester : Form
	{
		public NewGemTester()
		{
			this.InitializeComponent();
		}

		private void Button1_Click(object sender, EventArgs e)
		{
			var gemList = new List<GemNew>();
			gemList.Add(new GemNew(GemColors.Orange));
			gemList.Add(new GemNew(gemList[0], gemList[0]));
			gemList.Add(new GemNew(gemList[1], gemList[1]));
			gemList.Add(new GemNew(gemList[1], gemList[0]));
			gemList.Add(new GemNew(gemList[3], gemList[0]));
			gemList.Add(new GemNew(gemList[4], gemList[0]));
			gemList.Add(new GemNew(gemList[5], gemList[0]));
			gemList.Add(new GemNew(gemList[6], gemList[0]));
			gemList.Add(new GemNew(gemList[7], gemList[1]));
			gemList.Add(new GemNew(gemList[8], gemList[2]));

			foreach (var gem in gemList)
			{
				if (!gem.IsBaseGem)
				{
					for (int j = 0; j < gemList.Count; j++)
					{
						var gem2 = gemList[j];
						this.textBox1.AppendText(string.Format("{0}: {1}, {2}, {3}, {4}", j, gem2.SlotRequirement, gem2.UseCount, gem2.Slot, gem2.CanCreate) + "\r\n");
					}

					this.textBox1.AppendText("\r\n");
					gem.FakeCreate();
				}
			}

			var lastGem = gemList[gemList.Count - 1];
			this.textBox1.AppendText(string.Format("Final: {0}, {1}", lastGem.SlotRequirement, lastGem.UseCount));
		}
	}
}
