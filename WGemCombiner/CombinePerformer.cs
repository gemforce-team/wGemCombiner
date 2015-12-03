namespace WGemCombiner
{
	using System;
	using System.Diagnostics;
	using System.Drawing;
	using System.Threading;
	using System.Windows.Forms;
	using Properties;
	using static NativeMethods;

	internal static class CombinePerformer
	{
		#region Private Constants
		private const string GemcraftClassName = "ApolloRuntimeContentWindow";
		private const string GemcraftWindowName = "GemCraft Chasing Shadows";
		private const uint KeyEventFKeyUp = 0x2;
		private const double NativeScreenHeight = 612; // 1088 x 612 says spy++, 600 flash version
		private const double NativeScreenWidth = 1088;
		private const int SlotSize = 28;
		#endregion

		#region Fields
		private static Point cursorStart;

		private static double resolutionRatio = 1;
		#endregion

		#region Public Events
		public static event EventHandler<int> StepComplete = (null1, null2) => { };
		#endregion

		#region Public Properties
		public static bool CancelCombine { get; set; }

		public static bool Enabled { get; set; }

		public static InstructionCollection Instructions { get; set; }

		public static int SleepTime { get; set; } = 33;
		#endregion

		#region Public Methods
		public static void PerformCombine(int mSteps)
		{
			if (!Enabled)
			{
				return;
			}

			const byte KeyD = 0x44;
			// const byte KeyG = 0x47;
			const byte KeyU = 0x55;
			const byte KeyDot = 0xBE; // hide info box

			Rectangle clientRect;
			resolutionRatio = 1; // set the default ratio back to 1

			IntPtr gemcraftHandle = FindWindow(GemcraftClassName, GemcraftWindowName);

			// Verify that Gemcraft is a running process.
			if (gemcraftHandle == IntPtr.Zero)
			{
				// Gemcraft Steam verison not running, defaulting back to flash version
				PressMouse();
				ReleaseMouse(); // Just to give focus to the window
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

			// In limited experiments, the mouse drag operations seemed to be the most prone to failure when Gemcraft was laggy, so I originally added a bit of extra sleep time both before and after the mouse moves. This may not be necessary, per the note at case ActionType.Combine. Needs further testing.
			CancelCombine = false;
			cursorStart = Cursor.Position;
			if (Settings.Default.HidePanels)
			{
				PressKey(KeyDot); // hide info box
			}

			mSteps--; // Visually 1-based, but internally 0-based
			for (int i = mSteps; i < Instructions.Count; i++)
			{
				var instruction = Instructions[i];
				Debug.WriteLine(instruction);
				Thread.Sleep(SleepTime);
				switch (instruction.Action)
				{
					case ActionType.Duplicate:
						MoveCursorToSlot(instruction.From);
						PressKey(KeyD);
						break;
					case ActionType.Upgrade:
						MoveCursorToSlot(instruction.From);
						PressKey(KeyU);
						break;
					case ActionType.Move:
						SleepTime = 1000;
						// Move gem (only used when slots are compressed)
						MoveCursorToSlot(instruction.From);
						PressMouse();
						// Thread.Sleep(this.SleepTime / 2); // Extra sleep for mouse drag.
						MoveCursorToSlot(instruction.To);
						// Thread.Sleep(this.SleepTime / 2);
						ReleaseMouse();
						break;
					case ActionType.Combine:
						// PressKey(KeyG);
						// Do NOT use the G key here. At least in the Steam version, combining gems without a sufficient delay will fail with the key, where the mouse moves appear to be buffered and will succeed.
						MoveCursorToSlot(-1);
						PressMouse();
						ReleaseMouse();
						MoveCursorToSlot(instruction.From);
						PressMouse();
						// Thread.Sleep(this.SleepTime / 2); // Extra sleep for mouse drag
						MoveCursorToSlot(instruction.To);
						// Thread.Sleep(this.SleepTime / 2);
						ReleaseMouse();
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
				PressKey(KeyDot); // show info box
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

		private static void MoveCursorToSlot(int slot)
		{
			if (slot == -1)
			{
				Cursor.Position = new Point(cursorStart.X - (int)(-0.5 * SlotSize * resolutionRatio), cursorStart.Y - (int)(12.8 * SlotSize * resolutionRatio));
				return;
			}

			var cursorDestination = GetSlotPos(slot);
			var scaledPoint = new Point(
				cursorStart.X - (int)(cursorDestination.X * SlotSize * resolutionRatio),
				cursorStart.Y - (int)(cursorDestination.Y * SlotSize * resolutionRatio));
			Cursor.Position = scaledPoint;
		}

		private static void PressKey(byte keyCode)
		{
			keybd_event(keyCode, 0, 0, UIntPtr.Zero);
			Thread.Sleep(3);
			keybd_event(keyCode, 0, KeyEventFKeyUp, UIntPtr.Zero);
		}

		private static void PressMouse() => mouse_event(2, 0, 0, 0, UIntPtr.Zero);

		private static void ReleaseMouse() => mouse_event(4, 0, 0, 0, UIntPtr.Zero);
		#endregion
	}
}