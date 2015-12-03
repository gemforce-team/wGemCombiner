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
		#endregion

		#region Public Static Properties
		// By definition, an object cannot be a constant, so make it a static property instead.
		public static IntPtr HtCaption { get; } = new IntPtr(2);
		#endregion

		#region External Methods
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName); // find gemcraft window

		[DllImport("user32.dll")]
		public static extern short GetAsyncKeyState(Keys vKey);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect); // grab the demensions of the window

		[DllImport("user32.dll")]
		public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

		[DllImport("user32.dll")]
		public static extern uint MapVirtualKey(uint uCode, uint uMapType);

		[DllImport("user32.dll")]
		public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, UIntPtr dwExtraInfo);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ReleaseCapture();

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
