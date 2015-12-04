namespace WGemCombiner
{
	using System;
	using System.Globalization;

	#region Public Enums
	public enum ActionType
	{
		Combine,
		Duplicate,
		Upgrade
	}
	#endregion

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "Instructions will never be compared for equality and will not be used as hash keys.")]
	public struct Instruction
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

		#region Public Properties
		public ActionType Action { get; }

		public int From { get; }

		public int To { get; }
		#endregion

		#region Public Static Methods
		public static string SlotName(int slot)
		{
			int row = (slot / 3) + 1;
			int column = slot % 3;
			return row.ToString(CultureInfo.CurrentCulture) + "ABC".Substring(column, 1);
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
				default:
					return "Combine " + fromSlot + "→" + SlotName(this.To);
			}
		}
		#endregion
	}
}
