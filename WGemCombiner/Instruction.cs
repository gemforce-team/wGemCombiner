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

	public struct Instruction : IEquatable<Instruction>
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

		public int From { get; }

		public int To { get; }
		#endregion

		#region Operators
		public static bool operator ==(Instruction lhs, Instruction rhs) => Equals(lhs, rhs);

		public static bool operator !=(Instruction lhs, Instruction rhs) => !Equals(lhs, rhs);
		#endregion

		#region Public Static Methods
		public static string SlotName(int slot)
		{
			int row = (slot / 3) + 1;
			int column = slot % 3;
			return row.ToString(CultureInfo.CurrentCulture) + "ABC".Substring(column, 1);
		}
		#endregion

		#region Public Methods
		public bool Equals(Instruction other) => Equals(this, other);
		#endregion

		#region Public Override Methods
		public override bool Equals(object obj) => obj is Instruction ? Equals(this, (Instruction)obj) : false;

		public override int GetHashCode() => ((int)this.Action * 36 * 36) ^ (this.From * 36) ^ this.To;

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

		#region Private Static Methods
		private static bool Equals(Instruction lhs, Instruction rhs) => ReferenceEquals(lhs, rhs) || (lhs.Action == rhs.Action && lhs.From == rhs.From && lhs.To == rhs.To);
		#endregion
	}
}
