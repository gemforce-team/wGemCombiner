namespace WGemCombiner
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Windows.Forms;
    using static NativeMethods;

    public class TextProgressBar : ProgressBar
    {
        #region Fields
        private string text;
        #endregion

        #region Constructors
        public TextProgressBar()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.DoubleBuffered = true;
        }
        #endregion

        #region Public Properties
        [Browsable(true)]
        public override Font Font
        {
            get
            {
                return base.Font;
            }

            set
            {
                base.Font = value;
            }
        }

        [Browsable(true)]
        public override string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                this.text = value;
                this.Invalidate();
            }
        }

        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(SystemColors), "HighlightText")]
        [Description("Selects the text color.")]
        [Editor("System.Windows.Forms.ColorEditor", typeof(UITypeEditor))]
        public Color TextColor { get; set; }
        #endregion

        #region Protected Override Methods
        protected override void OnHandleCreated(EventArgs e)
        {
            if (SetWindowTheme(this.Handle, string.Empty, string.Empty) == 0)
            {
                base.OnHandleCreated(e);
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_PAINT = 0x000F;
            base.WndProc(ref m);

            if (m.Msg == WM_PAINT)
            {
                var flags = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine | TextFormatFlags.WordEllipsis;
                using (Graphics g = Graphics.FromHwnd(this.Handle))
                using (Brush textBrush = new SolidBrush(this.ForeColor))
                {
                    TextRenderer.DrawText(g, this.Text, this.Font, new Rectangle(0, 0, this.Width, this.Height), this.TextColor, flags);
                }
            }
        }
        #endregion
    }
}