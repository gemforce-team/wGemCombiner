namespace WGemCombiner
{
	using System;
	using System.Globalization;
	using System.IO;
	using static Globals;

	#region Public Enums
	public enum ActionType
	{
		Combine,
		Duplicate,
		Move,
		Upgrade
	}
	#endregion

	public struct Instruction
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
			if (type == ActionType.Duplicate)
			{
				this.To = InstructionDuplicate;
			}
			else if (type == ActionType.Upgrade)
			{
				this.From = from;
				this.To = InstructionUpgrade;
			}
			else
			{
				throw new ArgumentOutOfRangeException(nameof(type), "Cannot specify Move or Combine as the type for this constructor.");
			}
		}

		// Now used only for loading/saving.
		public Instruction(int from, int to)
		{
			this.From = from;
			this.To = to;
			if (to == InstructionDuplicate)
			{
				this.Action = ActionType.Duplicate;
			}
			else if (to == InstructionUpgrade)
			{
				this.Action = ActionType.Upgrade;
			}
			else if (to < 0)
			{
				this.Action = ActionType.Move;
				this.To = -to;
			}
			else
			{
				this.Action = ActionType.Combine;
			}
		}

		// This constructor is a hack, but it's expected not to be needed once text file parsing is in place.
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Temporary")]
		public Instruction(Gem gem)
			: this(gem[0].Id, gem[1].Id)
		{
		}

		public Instruction(ActionType type, int from, int to)
		{
			// Dupe can also use the other constructors, but specifying the target via this one is prefered even though it won't affect anything in the actual combine. This is required by the CondenseSlots logic to move the final base gem when no longer needed, but is also useful for debugging.
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

		public int From { get; }

		public int To { get; set; }
		#endregion

		#region Operators
		public static bool operator ==(Instruction first, Instruction second) => Equals(first, second);

		public static bool operator !=(Instruction first, Instruction second) => !Equals(first, second);
		#endregion

		#region Public Static Methods
		public static bool Equals(Instruction first, Instruction second) => first.From == second.From && first.To == second.To;

		public static Instruction NewFromStream(BinaryReader stream)
		{
			ThrowNull(stream, nameof(stream));
			var from = stream.ReadInt32();
			var to = stream.ReadInt32();
			return new Instruction(from, to);
		}

		public static string SlotName(int slot)
		{
			int row = (slot / 3) + 1;
			int column = slot % 3;
			return row.ToString(CultureInfo.CurrentCulture) + "ABC".Substring(column, 1);
		}
		#endregion

		#region Public Methods
		public void Save(BinaryWriter stream)
		{
			ThrowNull(stream, nameof(stream));
			stream.Write(this.From);
			stream.Write(this.To);
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
