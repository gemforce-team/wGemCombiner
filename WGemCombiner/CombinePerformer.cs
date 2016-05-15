namespace WGemCombiner
{
    using System;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    using Properties;
    using static NativeMethods;

    internal class CombinePerformer : IDisposable
    {
        #region Fields
        private static double resolutionRatio = 1;
        #endregion

        #region Private Constants
        private const string GemcraftClassName = "ApolloRuntimeContentWindow";
        private const string GemcraftWindowName = "GemCraft Chasing Shadows";
        private const double NativeScreenHeight = 612; // 1088 x 612 says spy++, 600 flash version
        private const double NativeScreenWidth = 1088;
        private const int SlotSize = 28;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources", Justification = "Window handles are unmanaged singletons")]
        private IntPtr gcNativeWindowHandle = IntPtr.Zero;
        private Point gcNativeClientPointStart = Point.Empty;
        #endregion

        public CombinePerformer()
        {
        }

        ~CombinePerformer()
        {
            this.Dispose(false);
        }

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

            // Verify that Gemcraft is a running process.
            if (gemcraftHandle == IntPtr.Zero)
            {
                // Gemcraft Steam version not running, defaulting back to flash version
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

            /* TIMING CHANGE STRATEGY
             *  ----------------------
             *   Thread sleeping is not an effective form of time-sensitive delay, because sleeping and waking for
             *  threads occurs on a 16ms or 20ms clock, depending on how the system is configured for performance
             *  in system settings. For example sleeping for 50ms will often sleep for 60ms instead.
             *    Instead, I'm using a "high performance counter" for accurate microsecond timing; in C# this is the
             *  Stopwatch class. I send out 20ms worth of commands beyond than the time-math indicates, then sleep.
             *  Upon waking, I calculate how many instructions should have been sent out while we were sleeping,
             *  and send out the appropriate number. The count will always be a fixed/consistent number ahead.
             *    This makes sure GemCraft has plenty of instructions to be working on; intra-instruction delays are irrelevant
             *  with this method, we only care about overfilling the message queue which happens when 600+ instructions
             *  are sent in advance. Ideally, we want to keep the message queue at around 50 messages for maximum speed.
             */
            System.Diagnostics.Stopwatch timeElapsed = System.Diagnostics.Stopwatch.StartNew();
            int currentStep = mSteps;
            int sentSteps = 0;
            int maxStep = Instructions.Count;
            // Round SleepTime up to the next highest 20ms, + another 20 for good measure
            double anticipatedSleepDuration = ((SleepTime + 19) / 20) * 20 + 20;
            // Actual Sleep() value. Never sleep for 0; it's inefficient and will never make things faster.
            int requestedSleepTime = Math.Max(1, SleepTime);
            while (!CancelCombine)
            {
                // SleepTime is the delay we want between each instruction, not actually the time to sleep. Rename variable to "stepDelay"?
                int instructionsThatShouldBeSentByNow;
                if (SleepTime == 0)
                {
                    instructionsThatShouldBeSentByNow = maxStep - currentStep;
                }
                else
                {
                    instructionsThatShouldBeSentByNow = (int)((timeElapsed.ElapsedMilliseconds + anticipatedSleepDuration) / SleepTime);
                }

                int instructionsToSend = instructionsThatShouldBeSentByNow - sentSteps;

                // System.Diagnostics.Debug.WriteLine("{0} Should:{1} Sent:{2} Sending:{3} Cur:{4}/{5}", timeElapsed.ElapsedMilliseconds, instructionsThatShouldBeSentByNow, sentSteps, instructionsToSend, currentStep, maxStep);
                for (int i = 0; i < instructionsToSend && !CancelCombine; i++)
                {
                    var instruction = Instructions[currentStep];

                    switch (instruction.Action)
                    {
                        case ActionType.Duplicate:
                            this.MoveMouse(this.GetSlotCursorPoint(instruction.From), false);
                            this.PressKey(KeyD, NativeMethods.scancode_D);
                            break;
                        case ActionType.Upgrade:
                            this.MoveMouse(this.GetSlotCursorPoint(instruction.From), false);
                            this.PressKey(KeyU, NativeMethods.scancode_U);
                            break;
                        case ActionType.Combine:
                            Point destCursorPoint = this.GetSlotCursorPoint(instruction.To);

                            this.PressKey(KeyG, NativeMethods.scancode_G);
                            this.PressMouse(this.GetSlotCursorPoint(instruction.From));
                            this.ReleaseMouse(destCursorPoint);
                            break;
                    }

                    ++currentStep;
                    ++sentSteps;
                    if (currentStep >= maxStep)
                    {
                        CancelCombine = true;
                    }

                    StepComplete(null, currentStep);
                }

                // System.Diagnostics.Debug.WriteLine("{0} Sleep({1}) currentStep:{2}", timeElapsed.ElapsedMilliseconds, requestedSleepTime, currentStep);
                if (!CancelCombine)
                {
                    Thread.Sleep(requestedSleepTime);
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

            return scaledPoint;
        }

        private void PressKey(ushort keyCode, byte scancode)
        {
            IntPtr code = new IntPtr(1 + (((int)scancode) << 16));
            NativeMethods.PostMessage(this.gcNativeWindowHandle, NativeMethods.WmKeyDown, new IntPtr(keyCode), code);
            code += (int)1 << 30;
            NativeMethods.PostMessage(this.gcNativeWindowHandle, NativeMethods.WmKeyUp, new IntPtr(keyCode), code);
            // System.Diagnostics.Debug.WriteLine("Kp: {0:X8} Rect: ({1}) [{1:G}]", this.gcNativeWindowHandle, scancode);
        }

        // private void PressMouse() => mouse_event(2, 0, 0, 0, UIntPtr.Zero);
        private void PressMouse(Point p)
        {
            IntPtr xy = new IntPtr((short)p.X + ((int)((short)p.Y) << 16));
            // System.Diagnostics.Debug.WriteLine("Dn: {0:X8} Rect: ({1},{2}) [{3}]", this.gcNativeWindowHandle, p.X, p.Y, xy);
            NativeMethods.PostMessage(this.gcNativeWindowHandle, NativeMethods.WmLButtonDown, new IntPtr(1), xy);
            // NativeMethods.SendMessage(this.gcNativeWindowHandle, NativeMethods.WmLButtonDown, new IntPtr(1), xy);
        }

        private void ReleaseMouse(Point p)
        {
            IntPtr xy = new IntPtr((short)p.X + ((int)((short)p.Y) << 16));
            // System.Diagnostics.Debug.WriteLine("Up: {0:X8} Rect: ({1},{2}) [{3}]", this.gcNativeWindowHandle, p.X, p.Y, xy);
            NativeMethods.PostMessage(this.gcNativeWindowHandle, NativeMethods.WmLButtonUp, IntPtr.Zero, xy);
            // NativeMethods.SendMessage(this.gcNativeWindowHandle, NativeMethods.WmLButtonUp, IntPtr.Zero, xy);
        }

        private void MoveMouse(Point p, bool drag)
        {
            IntPtr xy = new IntPtr((short)p.X + ((int)((short)p.Y) << 16));
            // System.Diagnostics.Debug.WriteLine("Mv: {0:X8} Rect: ({1},{2}) [{3}]", this.gcNativeWindowHandle, p.X, p.Y, xy);
            NativeMethods.PostMessage(this.gcNativeWindowHandle, NativeMethods.WmMouseMove, drag ? new IntPtr(1) : IntPtr.Zero, xy);
            // NativeMethods.SendMessage(this.gcNativeWindowHandle, NativeMethods.WmMouseMove, drag ? new IntPtr(1) : IntPtr.Zero, xy);
        }

        //private string GetGemcraftWindowName()
        //{
        //    int length = GetWindowTextLength(this.gcNativeWindowHandle);
        //    System.Text.StringBuilder sb = new System.Text.StringBuilder(length + 1);
        //    GetWindowText(this.gcNativeWindowHandle, sb, sb.Capacity);
        //    return sb.ToString();
        //}

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            this.Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                this.disposedValue = true;
            }
        }

        #endregion
    }
}
