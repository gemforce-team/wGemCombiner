namespace WGemCombiner
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using static Globals;

	public class InstructionCollection : Collection<Instruction>
	{
		private SortedSet<int> empties = new SortedSet<int>();
		private int slotsRequired;
		private Collection<GemNew> baseGems = new Collection<GemNew>();

		public IReadOnlyCollection<GemNew> BaseGems => this.baseGems;

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

		public void AddBaseGem(GemNew gem)
		{
			this.baseGems.Add(gem);
			this.slotsRequired++;
		}

		public void Combine(GemNew gem, int slot1, int slot2)
		{
			ThrowNull(gem, nameof(gem));
			if (slot2 > slot1)
			{
				var temp = slot1;
				slot1 = slot2;
				slot2 = temp;
			}

			this.Add(new Instruction(ActionType.Combine, slot1, slot2));
			this.empties.Add(slot1);
			gem.Slot = slot2;
		}

		public int DuplicateIfNeeded(GemNew gem)
		{
			ThrowNull(gem, nameof(gem));
			if (gem.UseCount > 0)
			{
				// Dupe if not the last use (two uses = gem[0] + gem[1])
				var slot = this.empties.Count == 0 ? this.SlotsRequired : this.empties.Min;
				this.SlotsRequired = slot;
				this.Add(new Instruction(ActionType.Duplicate, gem.Slot, slot));
				this.empties.Remove(slot);

				return slot;
			}

			return gem.Slot;
		}

		public void Upgrade(GemNew gem, int slot)
		{
			ThrowNull(gem, nameof(gem));
			this.Add(new Instruction(ActionType.Upgrade, slot));
			gem.Slot = slot;
		}
	}
}
