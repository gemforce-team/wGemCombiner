namespace WGemCombiner
{
    using System.Windows.Forms;

    public class BufferedGroupBox : GroupBox
    {
        public BufferedGroupBox()
            : base()
        {
            this.DoubleBuffered = true;
        }
    }
}
