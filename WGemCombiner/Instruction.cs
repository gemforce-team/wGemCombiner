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
		#region private Constants
		private const int InstructionDuplicate = -99;
		private const int InstructionUpgrade = -98;
		#endregion

		#region Constructors
		public Instruction(ActionType type, int from)
		{
			this.Action = type;
			this.From = from;
			if (type == ActionType.Upgrade)
			{
				this.From = from;
				this.To = InstructionUpgrade;
			}
			else
			{
				throw new ArgumentOutOfRangeException(nameof(type), "You can only use this constructor for the Upgrade instruction.");
			}
		}

		public Instruction(ActionType type, int from, int to)
		{
			this.Action = type;
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

		#region Operators
		public static bool operator ==(Instruction first, Instruction second) => Equals(first, second);

		public static bool operator !=(Instruction first, Instruction second) => !Equals(first, second);
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
		public override bool Equals(object obj)
		{
			if (obj is Instruction)
			{
				return Equals(this, (Instruction)obj);
			}

			return false;
		}

		public override int GetHashCode() => this.From.GetHashCode() ^ this.To.GetHashCode();

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
	}
}
