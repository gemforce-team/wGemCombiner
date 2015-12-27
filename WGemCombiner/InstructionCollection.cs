namespace WGemCombiner
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Globalization;
	using static Globals;

	/// <summary>The InstructionCollection class represents the concrete set of instructions necessary to create a specific gem, as displayed on the main form. It also tracks how many slots are used by a specific set of instructions (or subset, in the case of the slot condenser).</summary>
	public class InstructionCollection : IReadOnlyList<Instruction>
	{
		#region Fields
		private List<Instruction> instructions = new List<Instruction>();
		private SortedSet<int> empties = new SortedSet<int>();
		#endregion

		#region Constructors
		public InstructionCollection(IEnumerable<Gem> gemsToIgnore)
		{
			ThrowNull(gemsToIgnore, nameof(gemsToIgnore));
			var slotsToIgnore = new List<int>();
			foreach (var gem in gemsToIgnore)
			{
				if (gem.Slot == Combiner.NotSlotted)
				{
					throw new InvalidOperationException("Instruction collection told to ignore an un-slotted gem.");
				}

				slotsToIgnore.Add(gem.Slot);
			}

			if (slotsToIgnore.Count > 0)
			{
				// slotsToIgnore should include both base gem slots and any slots the slot condenser has used along the way.
				slotsToIgnore.Sort();
				var highestSlot = slotsToIgnore[slotsToIgnore.Count - 1];
				for (int i = 0; i < highestSlot; i++)
				{
					this.empties.Add(i);
				}

				foreach (var slot in slotsToIgnore)
				{
					this.empties.Remove(slot);
					this.SlotsRequired = slot + 1;
				}
			}
		}

		public InstructionCollection(InstructionCollection instructions1, InstructionCollection instructions2, Gem combine)
		{
			ThrowNull(instructions1, nameof(instructions1));
			ThrowNull(instructions2, nameof(instructions2));
			this.instructions.AddRange(instructions1);
			this.instructions.AddRange(instructions2);
			this.Combine(combine);
			this.SlotsRequired = instructions2.SlotsRequired > instructions1.SlotsRequired ? instructions2.SlotsRequired : instructions1.SlotsRequired;
		}
		#endregion

		#region Public Properties
		public int Count => this.instructions.Count;

		public int SlotsRequired { get; private set; }

		public bool WasCondensed { get; set; }
		#endregion

		#region Public Indexers
		public Instruction this[int index] => this.instructions[index];
		#endregion

		#region Public Methods
		public bool Combine(Gem parentGem)
		{
			ThrowNull(parentGem, nameof(parentGem));
			var slot2 = this.DuplicateIfNeeded(parentGem.Component2);
			var slot1 = this.DuplicateIfNeeded(parentGem.Component1);
			if (slot2 > slot1)
			{
				// Dupe auto-sorts when both calls duplicate, but if they don't, we still need to choose the lowest manually.
				var temp = slot1;
				slot1 = slot2;
				slot2 = temp;
			}

			this.instructions.Add(new Instruction(ActionType.Combine, slot1, slot2));
			this.empties.Add(slot1);
			parentGem.Slot = slot2;

			bool dupeHappened = false;
			foreach (var gem in parentGem)
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

		public IEnumerator<Instruction> GetEnumerator() => this.instructions.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => this.instructions.GetEnumerator();

		public bool Upgrade(Gem gem)
		{
			ThrowNull(gem, nameof(gem));
			var slot = this.DuplicateIfNeeded(gem.Component1);
			this.instructions.Add(new Instruction(ActionType.Upgrade, slot));
			gem.Slot = slot;
			if (gem.Component1.UseCount == 0)
			{
				gem.Component1.Slot = Combiner.NotSlotted;
				return false;
			}

			return true;
		}

		public void Verify(IEnumerable<BaseGem> baseGems)
		{
			ThrowNull(baseGems, nameof(baseGems));
			bool[] slots = new bool[36];
			foreach (var gem in baseGems)
			{
				slots[gem.OriginalSlot] = true;
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
					default:
						break;
				}

				/*
				Code disabled, since slot condenser now working as expected. If re-enabled, must be made to work with gemsToIgnore variable rather than a single slot.
				if (condenserGemSlot >= 0 && slots[condenserGemSlot])
				{
					throw new InvalidOperationException("Condenser gem slot was used, when it should always be ignored!");
				}
				*/
			}

			if (!slots[0])
			{
				throw new InvalidOperationException("No gem in 1A at the end of the run.");
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
				throw new InvalidOperationException("More than one gem left at the end of the run. Occupied slots: " + string.Join(", ", gemList));
			}
		}
		#endregion

		#region Private Methods
		private int DuplicateIfNeeded(Gem gem)
		{
			// Dupe if not the last use
			ThrowNull(gem, nameof(gem));
			int slot;
			if (gem.UseCount > 0)
			{
				if (this.empties.Count == 0)
				{
					slot = this.SlotsRequired;
					this.SlotsRequired++;
				}
				else
				{
					slot = this.empties.Min;
					this.empties.Remove(slot);
				}

				this.instructions.Add(new Instruction(ActionType.Duplicate, gem.Slot, slot));
			}
			else
			{
				slot = gem.Slot;
			}

			return slot;
		}
		#endregion
	}
}
