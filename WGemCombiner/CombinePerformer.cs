namespace WGemCombiner
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Drawing;
	using System.Globalization;
	using System.IO;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Threading;
	using System.Windows.Forms;
	using static Instruction;
	using static NativeMethods;

	// TODO: Redo most of this class so that equations are the default structure, as this goes through a LOT of unnecessary work right now to convert from equations to parenthetical format, then to this.combined which is essentially going back to equations again. Then the slot condenser starts doing the same thing, only recursively this time! Better to convert to equations at the start and then stick with those throughout.
	internal class CombinePerformer
	{
		#region Private Constants
		private const string GemcraftClassName = "ApolloRuntimeContentWindow";
		private const string GemcraftWindowName = "GemCraft Chasing Shadows";
		private const uint KeyEventFKeyUp = 0x2;
		private const double NativeScreenHeight = 612; // 1088 x 612 says spy++, 600 flash version
		private const double NativeScreenWidth = 1088;
		private const int SlotSize = 28;
		#endregion

		#region Static Fields
		private static Point cursorStart;

		private static Regex gemPower = new Regex(@"g?(?<num>[0-9]+)\s*(?<color>([a-z]|\([a-z]+\)))");

		private static double resolutionRatio = 1;

		private static int slotLimit;
		#endregion

		#region Fields
		private bool limitSlots;
		private List<Gem> combined = new List<Gem>();
		private int slotsRequired = 0;
		#endregion

		#region Constructors
		public CombinePerformer(bool limitSlots)
		{
			this.limitSlots = limitSlots;
		}
		#endregion

		#region Public Events
		public event EventHandler<int> StepComplete;
		#endregion

		#region Public Properties
		public IReadOnlyCollection<Gem> BaseGems
		{
			get
			{
				var list = new List<Gem>();
				foreach (var gem in this.combined)
				{
					if (gem.IsBaseGem)
					{
						list.Add(gem);
					}
					else
					{
						break;
					}
				}

				return list;
			}
		}

		public string BaseGemText
		{
			get
			{
				var sb = new StringBuilder();
				var i = 0;
				foreach (var gem in this.BaseGems)
				{
					sb.AppendLine(Instruction.SlotName(i) + ": " + gem.ColorName);
					i++;
				}

				return sb.ToString().TrimEnd();
			}
		}

		public bool CancelCombine { get; set; }

		public bool Enabled { get; set; }

		public List<Instruction> Instructions { get; set; } = new List<Instruction>();

		public Gem ResultGem { get; set; }

		public int SleepTime { get; set; } = 33;

		public int SlotsRequired
		{
			get
			{
				return this.slotsRequired;
			}

			private set
			{
				// Do not set unless setting higher
				if (value >= this.slotsRequired)
				{
					this.slotsRequired = value + 1;
				}
			}
		}
		#endregion

		#region Public Static Methods
		public static string LeveledPreparser(string recipe)
		{
			// Replaces leveled gems (e.g., "3o" becomes "((o+o)+(o+o))")
			var sb = new StringBuilder();
			var replacements = new SortedDictionary<int, string>();
			int lastPos = 0;
			foreach (Match match in gemPower.Matches(recipe))
			{
				var num = int.Parse(match.Groups["num"].Value, CultureInfo.InvariantCulture);
				var color = match.Groups["color"].Value;
				string newColor;
				if (num == 1)
				{
					newColor = color;
				}
				else
				{
					if (!replacements.TryGetValue(num, out newColor))
					{
						if (num == 2)
						{
							newColor = "*";
						}
						else
						{
							newColor = replacements[replacements.Count + 1];
						}

						for (int i = replacements.Count + 2; i <= num; i++)
						{
							newColor = newColor.Replace("*", "(*+*)");
							replacements.Add(i, newColor);
						}
					}

					newColor = newColor.Replace("*", color);
				}

				sb.Append(recipe.Substring(lastPos, match.Index - lastPos));
				sb.Append(newColor);
				lastPos = match.Index + match.Length;
			}

			return lastPos == 0 ? recipe : sb.Append(recipe.Substring(lastPos)).ToString();
		}

			/* public static bool SchemeIsValid(string scheme)
			{
				// TODO: Consider implementing a RegEx-based solution for each specific type.
				if (scheme.Length <= 2)
				{
					return false;
				}

				var letterCount = 0;
				var brackets = 0;
				var hasPlus = false;
				foreach (var c in scheme)
				{
					switch (c)
					{
						case '(':
							brackets++;
							break;
						case ')':
							brackets--;
							break;
						case '+':
							hasPlus = true;
							break;
						default:
							if (char.IsLetterOrDigit(c))
							{
								letterCount++;
							}

							break;
					}
				}

				return brackets == 0 && hasPlus && letterCount > 1;
			}
			*/
		#endregion

		#region Public Methods
		public void ChangeLastDestination(int slot)
		{
			if (this.Instructions.Count > 0)
			{
				var lastInstruction = this.Instructions[this.Instructions.Count - 1];
				if (lastInstruction.To != slot)
				{
					if (lastInstruction.Action != ActionType.Move)
					{
						this.Instructions.Add(new Instruction(ActionType.Move, lastInstruction.To, slot));
					}
					else
					{
						lastInstruction.To = slot;
					}
				}
			}
		}

		public void Parse(string formula)
		{
			formula = formula.Replace(" ", string.Empty); // Remove spaces, whitespace is for human readers.
			var val = formula.IndexOf("(val", StringComparison.OrdinalIgnoreCase);
			if (val >= 0)
			{
				formula = formula.Substring(val);
			}

			formula = LeveledPreparser(formula);
			if (formula.IndexOf('=') > -1)
			{
				formula = ParseEquations(formula);
			}
			else
			{
				formula = formula.Replace("\n", string.Empty); // Remove newlines or the parser crashes
			}

			this.SetMethod(formula);
		}

		public void PerformCombine(int mSteps)
		{
			if (!this.Enabled)
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
			this.CancelCombine = false;
			cursorStart = Cursor.Position;
			PressKey(KeyDot); // hide info box
			Stopwatch sw = new Stopwatch();
			sw.Start();
			for (int i = mSteps; i < this.Instructions.Count; i++)
			{
				var instruction = this.Instructions[i];
				Thread.Sleep(this.SleepTime);
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
						// Do NOT use the G key here. At least in the Steam version, combining gems without a sufficient delay will fail with they key, where the mouse moves appear to be buffered and will succeed.
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

				this.StepComplete?.Invoke(this, i);
				if (this.CancelCombine)
				{
					break;
				}
			}

			sw.Stop();
			Debug.WriteLine(sw.ElapsedMilliseconds);

			PressKey(KeyDot); // show info box
		}

		// Save a combine
		public void Save(string path)
		{
			using (var file = File.OpenWrite(path))
			using (var binaryWriter = new BinaryWriter(file))
			{
				foreach (var gem in this.combined)
				{
					if (!gem.IsBaseGem)
					{
						// TODO: It's actually a bit silly to create an instruction just to save it, but this whole saving system needs to be re-thought into a simple text list of formulae that's read at run-time.
						new Instruction(gem).Save(binaryWriter);
					}
				}
			}
		}

		public void SetMethod(string formula)
		{
			this.SetGems(formula); // Performs the combine pattern internally, recording the useage of each gem.
			this.CreateInstructions(); // Uses the result gem from SetGems and the useage data to create the instructions.
		}
		#endregion

		#region Private Static Methods
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

		private static string ParseEquations(string str)
		{
			var replace = new Regex(@"\r?\n?\(val=[0-9]+\)\t?").Replace(str, "\n");
			string[] strP = replace.Trim().Split('\n');
			var sb = new StringBuilder(strP[strP.Length - 1].Split('=')[1]);
			for (int i = strP.Length - 2; i >= 0; i--)
			{
				var strC = strP[i].Split('=');
				var strRep = strC[1];
				if (strRep.IndexOf('+') > -1)
				{
					strRep = "(" + strRep + ")";
				}

				sb.Replace(strC[0], strRep);
			}

			return sb.ToString();
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

		#region Private Methods
		// CondenseSlots seems to be messed up, I can't get the 262144-combine to work.
		private List<Instruction> CondenseSlots(Gem gem)
		{
			// Get the combined instructions for both components. (Will not include duplicating base gem.)
			// If the combine for a component exceeds the slotLimit, recursively call CondenseSlots to get new instructions.
			//
			// Each component's combine instructions must include placing the base gem in slot 0.
			// If I add the duplicate step to performer1.Instructions, then the new condensed one will already have it.
			// To solve this, try: If the instructions do not begin with duplicate step, add it.

			// TODO: re-examine to see if this is needed if and when new CreateInstructions() is completed for list-style gem.
			Gem gem1 = gem[0];
			Gem gem2 = gem[1];
			CombinePerformer performer1 = new CombinePerformer(false);
			performer1.SetMethod(gem1.GetFullCombine());
			performer1.ResultGem.Id = gem1.Id;
			CombinePerformer performer2 = new CombinePerformer(false);
			performer2.SetMethod(gem2.GetFullCombine());
			performer2.ResultGem.Id = gem2.Id;

			if (performer1.SlotsRequired > slotLimit)
			{
				performer1.Instructions = this.CondenseSlots(gem1);
			}

			// Move result gem to highest open slot
			slotLimit--;
			if (slotLimit < 3)
			{
				// Not sure what fantastical combine gets you here, but it's conceivable, at any rate.
				throw new InvalidOperationException("Slot limit is too low; cannot perform this combine.");
			}

			performer1.ChangeLastDestination(slotLimit);
			var slot1 = slotLimit;

			if (performer2.SlotsRequired > slotLimit)
			{
				performer2.Instructions = this.CondenseSlots(gem2);
			}

			var slot2 = performer2.Instructions[performer2.Instructions.Count - 1].To;

			List<Instruction> newInst = new List<Instruction>();

			// Merge instructions
			// Both combines require placing a base gem in slot 0 first.
			if (performer1.Instructions[0].From != Slot12C)
			{
				newInst.Add(new Instruction(ActionType.Duplicate, Slot12C, Slot1A));
			}

			newInst.AddRange(performer1.Instructions);

			if (performer2.Instructions[0].From != Slot12C)
			{
				newInst.Add(new Instruction(ActionType.Duplicate, Slot12C, Slot1A));
			}

			newInst.AddRange(performer2.Instructions);

			// Combine the two resulting gems
			newInst.Add(new Instruction(ActionType.Combine, slot2, slot1)); // Combine to the higher slot
			slotLimit++;

			return newInst;
		}

		private void CreateInstructions()
		{
			// TODO: Re-examine to see if this can be made more list-friendly.
			this.Instructions.Clear(); // Instructions list
			var empties = new SortedSet<int>();
			for (int i = 0; i < this.combined.Count; i++)
			{
				var gem = this.combined[i];
				gem.Slot = gem.IsBaseGem ? i : SlotUninitialized;
			}

			this.slotsRequired = this.BaseGems.Count; // Bypass setter for initial setup

			// Don't try to give instructions for placing the base gems.
			for (int combinedIndex = this.BaseGems.Count; combinedIndex < this.combined.Count; combinedIndex++)
			{
				var gem = this.combined[combinedIndex];
				var slots = new List<int>();
				foreach (var component in gem)
				{
					slots.Add(component.Slot);
					component.UseCount--;
					Debug.Assert(component.Slot >= 0, "Gem slot negative.");
				}

				if (gem[0] == gem[1])
				{
					this.DupeGem(gem[0], slots[0], empties);
					this.Instructions.Add(new Instruction(ActionType.Upgrade, slots[0]));
					gem.Slot = slots[0];
				}
				else
				{
					for (int i = 0; i < gem.Count; i++)
					{
						this.DupeGem(gem[i], slots[i], empties);
					}

					// Combine
					this.Instructions.Add(new Instruction(ActionType.Combine, slots[0], slots[1]));
					empties.Add(slots[0]);
					gem.Slot = slots[1];
				}

				foreach (var component in gem)
				{
					if (component.UseCount == 0)
					{
						component.Slot = SlotNoLongerUsed;
					}
				}

				// If there is only one usage of any remaining gem components, move that gem next in the combine order. This reduces slot usage by not holding on to gems throughout subsequent steps for no good reason.
				var scanFrom = combinedIndex + 2; // No need to scan i + 1 because if it's a single-use, it's already in the correct position.
				var insertPos = combinedIndex + 1;
				var secondaries = new List<Gem>();
				while (scanFrom < this.combined.Count)
				{
					Gem gemUse = this.combined[scanFrom];

					// Do we need? && (empties.Count > 0)
					if (gemUse[0].Slot >= 0 && gemUse[1].Slot >= 0)
					{
						if ((gemUse[0].UseCount == 1 && gemUse[1].UseCount == 1) || (gemUse[0] == gemUse[1] && gemUse[0].UseCount == 2))
						{
							// Move single-use gem to be the next one parsed.
							this.combined.RemoveAt(scanFrom);
							this.combined.Insert(insertPos, gemUse);
							insertPos++;
							if (scanFrom == insertPos)
							{
								scanFrom++;
							}
						}
						else if (gemUse[0].UseCount == 1 || gemUse[1].UseCount == 1)
						{
							secondaries.Add(gemUse);
							this.combined.RemoveAt(scanFrom);
							// No scanFrom or insertPos change, since we want to resume from the current (= next) gem.
						}
						else
						{
							scanFrom++;
						}
					}
					else
					{
						scanFrom++;
					}
				}

				this.combined.InsertRange(insertPos, secondaries);
			}

			this.CheckSlotLimit();
		}

		private void CheckSlotLimit()
		{
			// REDUCE SLOT REQUIREMENT
			if (this.limitSlots && this.SlotsRequired > 36)
			{
				slotLimit = 35;
				var instructions = this.Instructions;
				instructions.Clear();
				instructions.Add(new Instruction(ActionType.Move, Slot1A, Slot12C));
				instructions.AddRange(this.CondenseSlots(this.ResultGem));

				// Change last Dupe 12C to a Move.
				for (int i = instructions.Count - 1; i >= 0; i--)
				{
					var instruction = instructions[i];
					if (instruction.Action == ActionType.Duplicate && instruction.From == Slot12C)
					{
						if (i == 1)
						{
							instructions.RemoveAt(0);
							instructions.RemoveAt(0);
						}
						else
						{
							var to = instruction.To;
							instructions.RemoveAt(i);
							instructions.Insert(i, new Instruction(ActionType.Move, Slot12C, to)); // I think this should always be 1A, but let's be sure.
						}

						break;
					}
				}
			}
		}

		private void DupeGem(Gem gem, int atSlot, SortedSet<int> empties)
		{
			if (gem.UseCount > 0)
			{
				// Dupe if not the last use (two uses = gem[0] + gem[1])
				gem.Slot = this.GetEmpty(empties);
				this.Instructions.Add(new Instruction(ActionType.Duplicate, atSlot, gem.Slot));
				this.SlotsRequired = gem.Slot;
			}
		}

		private int GetEmpty(SortedSet<int> empties)
		{
			if (empties.Count == 0)
			{
				return this.SlotsRequired;
			}

			var min = empties.Min;
			empties.Remove(min);
			return min;
		}

		private void SetGems(string formula)
		{
			this.combined.Clear();

			// Check which letters are used, and replace each one with an internal identifier
			var id = 0;
			foreach (var c in "oybrkm")
			{
				if (formula.IndexOf(c) > -1)
				{
					var gem = new Gem(c);
					gem.Id = id;
					formula = formula.Replace(gem.Letter, gem.Id.ToString(CultureInfo.InvariantCulture)[0]); // gem.ID should always be < 10 here and since we have to convert one or the other, a char-char replacement is probably faster than a string-string one.
					this.combined.Add(gem);
					id++;
				}
			}

			// Scan for plus signs within the formula and add gems together appropriately.
			int plus = formula.IndexOf('+');
			while (plus > -1)
			{
				string thisCombine;
				var close = formula.IndexOf(')', plus);
				if (close == -1)
				{
					thisCombine = formula;
					formula = "(" + formula + ")"; // Add brackets so the final replace will work
				}
				else
				{
					var open = formula.LastIndexOf('(', close);
					thisCombine = formula.Substring(open + 1, close - open - 1);
				}

				string[] combineGems = thisCombine.Split('+');
				if (combineGems.Length != 2)
				{
					throw new ArgumentException("The formula provided contains more than a single plus sign within a pair of brackets. This is not currently supported.", nameof(formula));
				}

				var gem1 = combineGems[0];
				var gem2 = combineGems[1];
				var num1 = Convert.ToInt32(gem1, CultureInfo.InvariantCulture);
				var num2 = Convert.ToInt32(gem2, CultureInfo.InvariantCulture);
				var newNum = this.combined.Count;

				this.ResultGem = new Gem(this.combined[num1], this.combined[num2]);
				this.ResultGem.Id = newNum;
				this.combined.Add(this.ResultGem);
				formula = formula.Replace("(" + gem1 + "+" + gem2 + ")", newNum.ToString(CultureInfo.InvariantCulture));

				plus = formula.IndexOf('+');
			}
		}
		#endregion
	}
}