namespace WGemCombiner
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using static Globals;

	public class InstructionCollection : List<Instruction>
	{
		#region Constants
		private const bool UseOldBehavior = false;
		private const int Slot1A = 0;
		#endregion

		#region Constructors
		public InstructionCollection(IList<Gem> baseGems)
		{
			foreach (var gem in baseGems)
			{
				if (gem.Slot >= this.SlotsRequired)
				{
					this.SlotsRequired = gem.Slot + 1;
				}
			}
		}

		internal InstructionCollection(IList<Gem> baseGems, int condenserSlot)
		{
			if (condenserSlot >= 0)
			{
				// Assumes base gems will be in lower slots than the condenser gem slot, which should always be the case.
				for (int i = 0; i < condenserSlot; i++)
				{
					this.Empties.Add(i);
				}

				foreach (var gem in baseGems)
				{
					this.Empties.Remove(gem.Slot);
					this.SlotsRequired = gem.Slot + 1;
				}

				if (condenserSlot >= this.SlotsRequired)
				{
					this.SlotsRequired = condenserSlot + 1;
				}
			}
		}
		#endregion

		#region Public Properties
		public SortedSet<int> Empties { get; } = new SortedSet<int>();

		public int SlotsRequired { get; internal set; }
		#endregion

		#region Public Methods
		public bool Combine(Gem parentGem)
		{
			ThrowNull(parentGem, nameof(parentGem));
			if (UseOldBehavior)
			{
				return this.CombineOld(parentGem);
			}

			var slot2 = this.DuplicateIfNeeded(parentGem.Components[1]);
			var slot1 = this.DuplicateIfNeeded(parentGem.Components[0]);
			this.Add(new Instruction(ActionType.Combine, slot1, slot2));
			this.Empties.Add(slot1);
			parentGem.Slot = slot2;

			bool dupeHappened = false;
			foreach (var gem in parentGem.Components)
			{
				if (gem.UseCount == 0)
				{
					gem.Slot = Combiner.NotSlotted;
				}
				else
				{
					dupeHappened = true;
				}
			}

			return dupeHappened;
		}

		public void Move1A(Gem gem)
		{
			ThrowNull(gem, nameof(gem));
			if (this.Count > 0)
			{
				var lastInstruction = this[this.Count - 1];
				if (lastInstruction.To != Slot1A)
				{
					this.Add(new Instruction(ActionType.Move, lastInstruction.To, Slot1A));
				}
			}
		}

		public bool Upgrade(Gem gem)
		{
			ThrowNull(gem, nameof(gem));
			if (UseOldBehavior)
			{
				return this.UpgradeOld(gem);
			}

			var slot = this.DuplicateIfNeeded(gem.Components[0]);
			this.Add(new Instruction(ActionType.Upgrade, slot));
			gem.Slot = slot;
			if (gem.Components[0].UseCount == 0)
			{
				gem.Components[0].Slot = Combiner.NotSlotted;
				return false;
			}

			return true;
		}

		public void Verify(IList<Gem> baseGems)
		{
			bool[] slots = new bool[36];
			foreach (var gem in baseGems)
			{
				slots[gem.Slot] = true;
			}

			for (int i = 0; i < this.Count; i++)
			{
				var instruction = this[i];
				switch (instruction.Action)
				{
					case ActionType.Combine:
						slots[instruction.From] = false;
						if (!slots[instruction.To])
						{
							throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "The instruction {0} at offset {1} tried to combine to an unoccupied slot.", instruction, i));
						}

						break;
					case ActionType.Duplicate:
						slots[instruction.To] = true;
						if (!slots[instruction.From])
						{
							throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "The instruction {0} at offset {1} tried to duplicate from an unoccupied slot.", instruction, i));
						}

						break;
					case ActionType.Move:
						slots[instruction.From] = false;
						if (slots[instruction.To])
						{
							throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "The instruction {0} at offset {1} tried to move to an occupied slot.", instruction, i));
						}
						else
						{
							slots[instruction.To] = true;
						}

						break;
					default:
						break;
				}
			}

			var gemList = new List<string>();
			for (int slot = 0; slot < slots.Length; slot++)
			{
				if (slots[slot])
				{
					gemList.Add(Instruction.SlotName(slot));
				}
			}

			if (gemList.Count != 1)
			{
				throw new InvalidOperationException("At the end of the run, there were gems in slots: " + string.Join(", ", gemList));
			}
		}
		#endregion

		#region Internal Methods
		internal void OptimizeCondensedBaseGems(IList<Gem> baseGems)
		{
			// Optimize last dupes of base gems so we don't leave the base gems hanging around.
			// e.g., the final Dupe 1A-3A gets removed and all subsequent 3A instructions are updated to use 1A.
			foreach (var gem in baseGems)
			{
				var index = this.FindLastIndex(g => g.From == gem.Slot && g.Action == ActionType.Duplicate);
				var oldLocation = this[index].To;
				this.RemoveAt(index);
				for (int index2 = index; index2 < this.Count; index2++)
				{
					this[index2].Translate(oldLocation, gem.Slot);
				}
			}
		}
		#endregion

		#region Private Methods
		private bool CombineOld(Gem parentGem)
		{
			var slot1 = this.DuplicateIfNeeded(parentGem.Components[0]);
			var slot2 = this.DuplicateIfNeeded(parentGem.Components[1]);
			var dupeHappened = parentGem.Components[0].UseCount != 0 || parentGem.Components[1].UseCount != 0;
			this.Add(new Instruction(ActionType.Combine, parentGem.Components[0].Slot, parentGem.Components[1].Slot));
			this.Empties.Add(parentGem.Components[0].Slot);
			parentGem.Slot = parentGem.Components[1].Slot;
			parentGem.Components[0].Slot = parentGem.Components[0].UseCount == 0 ? -1 : slot1;
			parentGem.Components[1].Slot = parentGem.Components[1].UseCount == 0 ? -1 : slot2;

			return dupeHappened;
		}

		private int DuplicateIfNeeded(Gem gem)
		{
			// Dupe if not the last use
			ThrowNull(gem, nameof(gem));
			int slot;
			if (gem.UseCount > 0)
			{
				if (this.Empties.Count == 0)
				{
					slot = this.SlotsRequired;
					this.SlotsRequired++;
				}
				else
				{
					slot = this.Empties.Min;
					this.Empties.Remove(slot);
				}

				this.Add(new Instruction(ActionType.Duplicate, gem.Slot, slot));
			}
			else
			{
				slot = gem.Slot;
			}

			return slot;
		}

		private bool UpgradeOld(Gem gem)
		{
			var slot = this.DuplicateIfNeeded(gem.Components[0]);
			var dupeHappened = gem.Components[0].UseCount == 0;
			this.Add(new Instruction(ActionType.Upgrade, gem.Components[0].Slot));
			gem.Slot = gem.Components[0].Slot;
			gem.Components[0].Slot = slot;

			return dupeHappened;
		}
		#endregion
	}
}
