namespace WGemCombiner
{
	using System;
	using System.Globalization;

	#region Public Enums
	public enum ActionType
	{
		Combine,
		Duplicate,
		Move,
		Upgrade
	}
	#endregion

	public class Instruction
	{
		#region Constructors
		public Instruction(ActionType action, int from)
			: this(action, from, from)
		{
		}

		public Instruction(ActionType action, int from, int to)
		{
			if (from < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(from), "Invalid slot in instruction.");
			}

			if (to < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(to), "Invalid slot in instruction.");
			}

			if (action == ActionType.Upgrade)
			{
				if (from != to)
				{
					throw new ArgumentException("From and to parameters are not equal in an Upgrade instruction.");
				}
			}
			else
			{
				if (from == to)
				{
					throw new ArgumentException("From and to parameters cannot be equal except in an Upgrade instruction.");
				}
			}

			this.Action = action;
			this.From = from;
			this.To = to;
		}
		#endregion

		#region Public Static Properties
		public static int Slot1A { get; } = 0;

		public static int Slot12C { get; } = 35;
		#endregion

		#region Public Properties
		public ActionType Action { get; }

		public int From { get; private set; }

		public int To { get; private set; }
		#endregion

		#region Public Static Methods
		public static bool Equals(Instruction first, Instruction second) => first?.From == second?.From && first?.To == second?.To;

		public static string SlotName(int slot)
		{
			int row = (slot / 3) + 1;
			int column = slot % 3;
			return row.ToString(CultureInfo.CurrentCulture) + "ABC".Substring(column, 1);
		}
		#endregion

		#region Public Methods
		public void Translate(int oldLocation, int newLocation)
		{
			if (this.From == oldLocation)
			{
				this.From = newLocation;
			}

			if (this.To == oldLocation)
			{
				this.To = newLocation;
			}
		}
		#endregion

		#region Public Override Methods
		public override string ToString()
		{
			var fromSlot = SlotName(this.From);
			switch (this.Action)
			{
				case ActionType.Duplicate:
					return "Dupe " + fromSlot + (this.To >= 0 ? "→" + SlotName(this.To) : string.Empty);
				case ActionType.Upgrade:
					return "Upgrade " + fromSlot;
				case ActionType.Move:
					return "Move " + fromSlot + "→" + SlotName(this.To);
				default:
					return "Combine " + fromSlot + "→" + SlotName(this.To);
			}
		}
		#endregion

		#region Internal Methods
		internal void Swap()
		{
			// Action type is not checked here because this is internal, but this should obviously only be used on combines or moves.
			var temp = this.From;
			this.From = this.To;
			this.To = temp;
		}
		#endregion
	}
}
