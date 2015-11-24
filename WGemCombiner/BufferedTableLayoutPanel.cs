namespace WGemCombiner
{
	using System.Windows.Forms;

	public class BufferedTableLayoutPanel : TableLayoutPanel
	{
		public BufferedTableLayoutPanel()
			: base()
		{
			this.DoubleBuffered = true;
		}
	}
}
