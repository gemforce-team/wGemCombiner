namespace WGemCombiner
{
	using System;
	using System.Drawing;
	using System.Runtime.InteropServices;
	using System.Windows.Forms;

	internal static class NativeMethods
	{
		#region Public Constants
		public const int WmSetRedraw = 0xB;
		public const int WmNclButtonDown = 0xA1;
        public const int WmMouseMove = 0x0200;
        public const int WmLButtonDown = 0x0201;
        public const int WmLButtonUp = 0x0202;
        public const int WmKeyDown = 0x0100;
        public const int WmKeyUp = 0x0101;
        public const int WmSetText = 0x000C;
        public const byte scancode_D = 0x20;
        public const byte scancode_G = 0x22;
        public const byte scancode_U = 0x16;
        public const byte scancode_dot = 0x34;
        #endregion

        #region Native Callback Delegates
        public delegate void SendMessageDelegate(IntPtr hWnd, uint uMsg, UIntPtr dwData, IntPtr lResult);
        #endregion

        #region Public Static Properties
        // By definition, an object cannot be a constant, so make it a static property instead.
        public static IntPtr HtCaption { get; } = new IntPtr(2);
        #endregion

        #region External Methods
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName); // find gemcraft window

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "0", Justification = "Known Microsoft Bug")]
        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point p);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ScreenToClient(IntPtr hWnd, ref Point p);

        [DllImport("user32.dll")]
		public static extern short GetAsyncKeyState(Keys vKey);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect); // grab the demensions of the window

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        // [DllImport("user32.dll")]
		// public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
		[DllImport("user32.dll")]
		public static extern uint MapVirtualKey(uint uCode, uint uMapType);

		// [DllImport("user32.dll")]
		// public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, UIntPtr dwExtraInfo);
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ReleaseCapture();

        // [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        // public static extern bool SendNotifyMessage(IntPtr hWnd, uint Msg, UIntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
		public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetForegroundWindow(IntPtr hWnd); // set focus to the window

		[DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
		public static extern int SetWindowTheme(IntPtr hWnd, string appname, string idlist);
        #endregion
    }
}
