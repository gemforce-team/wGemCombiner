namespace WGemCombiner
{
	using System;
	using System.Drawing;
	using System.Threading;
	using System.Windows.Forms;
	using Properties;
	using static NativeMethods;

	internal class CombinePerformer
	{
        #region Fields
        private static double resolutionRatio = 1;
        #endregion

        #region Private Constants
        private const string GemcraftClassName = "ApolloRuntimeContentWindow";
		private const string GemcraftWindowName = "GemCraft Chasing Shadows";
		private const uint KeyEventFKeyUp = 0x2;
		private const double NativeScreenHeight = 612; // 1088 x 612 says spy++, 600 flash version
		private const double NativeScreenWidth = 1088;
		private const int SlotSize = 28;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources", Justification = "Window handles are unmanaged singltons")]
        private IntPtr gcNativeWindowHandle = IntPtr.Zero;
        private Point gcNativeClientPointStart = Point.Empty;
		#endregion

		#region Public Events
		public static event EventHandler<int> StepComplete = (null1, null2) => { };
		#endregion

		#region Public Properties
		public static bool CancelCombine { get; set; }

		public static bool Enabled { get; set; }

		public static InstructionCollection Instructions { get; set; }

		public static int SleepTime { get; set; } = 0;
		#endregion

		#region Public Methods
		public void PerformCombine(int mSteps)
		{
			if (!Enabled)
			{
				return;
			}

            const ushort KeyD = 'D'; // = 0x44;
            const ushort KeyG = 'G'; // = 0x47;
            const ushort KeyU = 'U'; // = 0x55;
            const ushort KeyDot = 0xBE; // hide info box

			Rectangle clientRect;
			resolutionRatio = 1; // set the default ratio back to 1

			IntPtr gemcraftHandle = FindWindow(GemcraftClassName, GemcraftWindowName);

            this.gcNativeClientPointStart = Cursor.Position;
            this.gcNativeWindowHandle = WindowFromPoint(this.gcNativeClientPointStart);
            ScreenToClient(this.gcNativeWindowHandle, ref this.gcNativeClientPointStart);
            // System.Diagnostics.Debug.WriteLine("Window Handle: {0:X8} Client Rect: ({1},{2})", this.gcNativeWindowHandle, this.gcNativeClientPointStart.X, this.gcNativeClientPointStart.Y);
            // return;

			// Verify that Gemcraft is a running process.
			if (gemcraftHandle == IntPtr.Zero)
			{
                // Gemcraft Steam verison not running, defaulting back to flash version
                // SetForegroundWindow(this.gcNativeWindowHandle);
                // PressMouse();
                // ReleaseMouse(); // Just to give focus to the window
			}
			else
			{
				// set gemcraft window focus
				SetForegroundWindow(gemcraftHandle);

				// grab window size
				GetClientRect(gemcraftHandle, out clientRect);

				double width = clientRect.Width;
				double height = clientRect.Height;
				double ratio = NativeScreenWidth / NativeScreenHeight; // 1088x612//1.7777
				double newHeight = width / ratio;
				double newWidth = height * ratio;

				// Please modify if there is a better way.
                // Max: Is native aspect ratio forced at all resolutions?
				if (newHeight <= height)
				{
					resolutionRatio = width / NativeScreenWidth;
				}
				else if (newWidth <= width)
				{
					resolutionRatio = height / NativeScreenHeight;
				}

				/*
				MessageBox.Show("newheight="+newheight.ToString("0.#####")+
				 " newwidth="+newwidth.ToString("0.#####")+
				 " height="+height.ToString("0.#####")+
				 " width="+width.ToString("0.#####")+
				 " ratio="+ratio.ToString("0.#####")+
				 " resolutionRatio="+resolutionRatio.ToString("0.#####"));
				 return;
				*/
			}

			CancelCombine = false;
			if (Settings.Default.HidePanels)
			{
				this.PressKey(KeyDot, NativeMethods.scancode_dot); // hide info box
			}

			mSteps--; // Visually 1-based, but internally 0-based
			for (int i = mSteps; i < Instructions.Count; i++)
			{
				var instruction = Instructions[i];
				Thread.Sleep(SleepTime);
				switch (instruction.Action)
				{
					case ActionType.Duplicate:
						this.MoveMouse(this.GetSlotCursorPoint(instruction.From), false);
Thread.Sleep(20);
						this.PressKey(KeyD, NativeMethods.scancode_D);
						break;
					case ActionType.Upgrade:
						this.MoveMouse(this.GetSlotCursorPoint(instruction.From), false);
Thread.Sleep(20);
						this.PressKey(KeyU, NativeMethods.scancode_U);
						break;
					case ActionType.Combine:
                        // Do NOT use the G key here. At least in the Steam version, combining gems without a sufficient delay will fail with the key, where the mouse moves appear to be buffered and will succeed.
                        // GetSlotCursorPoint(-1);
                        // PressMouse();
                        // ReleaseMouse();
                        Point destCursorPoint = this.GetSlotCursorPoint(instruction.To);

                        this.PressKey(KeyG, NativeMethods.scancode_G);
                        this.PressMouse(this.GetSlotCursorPoint(instruction.From));
                        // this.MoveMouse(destCursorPoint, true);
						this.ReleaseMouse(destCursorPoint, true);
						break;
				}

				StepComplete(null, i + 1);
				if (CancelCombine)
				{
					break;
				}
			}

			if (Settings.Default.HidePanels)
			{
				this.PressKey(KeyDot, NativeMethods.scancode_dot); // show info box
			}
		}
		#endregion

		#region Private Methods
		private static Point GetSlotPos(int s)
		{
			int row = s / 3;
			int column = s % 3;
			return new Point(column, row);
		}

		private Point GetSlotCursorPoint(int slot)
		{
			if (slot == -1)
			{
				return new Point(this.gcNativeClientPointStart.X - (int)(-0.5 * SlotSize * resolutionRatio), this.gcNativeClientPointStart.Y - (int)(12.8 * SlotSize * resolutionRatio));
			}

			var cursorDestination = GetSlotPos(slot);
			var scaledPoint = new Point(
                this.gcNativeClientPointStart.X - (int)(cursorDestination.X * SlotSize * resolutionRatio),
                this.gcNativeClientPointStart.Y - (int)(cursorDestination.Y * SlotSize * resolutionRatio));
			if (Settings.Default.ExtremeLag)
			{
				Thread.Sleep(SleepTime / 2);
			}

            return scaledPoint;
        }

        private void PressKey(ushort keyCode, byte scancode)
		{
            IntPtr code = new IntPtr(1 + (((int)scancode) << 16));
            NativeMethods.SendMessage(this.gcNativeWindowHandle, NativeMethods.WmKeyDown, new IntPtr(keyCode), code);
            code += (int)1 << 30;
            NativeMethods.SendMessage(this.gcNativeWindowHandle, NativeMethods.WmKeyUp, new IntPtr(keyCode), code);
        }

        // private void PressMouse() => mouse_event(2, 0, 0, 0, UIntPtr.Zero);
        private void PressMouse(Point p)
        {
            IntPtr xy = new IntPtr((short)p.X + ((int)((short)p.Y) << 16));
            System.Diagnostics.Debug.WriteLine("Dn: {0:X8} Rect: ({1},{2}) [{3}]", this.gcNativeWindowHandle, p.X, p.Y, xy);
            // NativeMethods.PostMessage(this.gcNativeWindowHandle, NativeMethods.WmLButtonDown, new IntPtr(1), xy);
            NativeMethods.SendMessage(this.gcNativeWindowHandle, NativeMethods.WmLButtonDown, new IntPtr(1), xy);
        }

        private void ReleaseMouse(Point p, bool wait)
        {
            IntPtr xy = new IntPtr((short)p.X + ((int)((short)p.Y) << 16));
            System.Diagnostics.Debug.WriteLine("Up: {0:X8} Rect: ({1},{2}) [{3}]", this.gcNativeWindowHandle, p.X, p.Y, xy);
            if (!wait)
            {
                // NativeMethods.PostMessage(this.gcNativeWindowHandle, NativeMethods.WmLButtonUp, IntPtr.Zero, xy);
                NativeMethods.SendMessage(this.gcNativeWindowHandle, NativeMethods.WmLButtonUp, IntPtr.Zero, xy);
            }
            else
            {
                NativeMethods.SendMessage(this.gcNativeWindowHandle, NativeMethods.WmLButtonUp, IntPtr.Zero, xy);
            }
        }

        private void MoveMouse(Point p, bool drag)
        {
            IntPtr xy = new IntPtr((short)p.X + ((int)((short)p.Y) << 16));
            System.Diagnostics.Debug.WriteLine("Mv: {0:X8} Rect: ({1},{2}) [{3}]", this.gcNativeWindowHandle, p.X, p.Y, xy);
            // NativeMethods.PostMessage(this.gcNativeWindowHandle, NativeMethods.WmMouseMove, drag ? new IntPtr(1) : IntPtr.Zero, xy);
            NativeMethods.SendMessage(this.gcNativeWindowHandle, NativeMethods.WmMouseMove, drag ? new IntPtr(1) : IntPtr.Zero, xy);
        }

        #endregion
    }
}
