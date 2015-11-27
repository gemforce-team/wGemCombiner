namespace WGemCombiner
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using static Globals;

	public class InstructionCollection : Collection<Instruction>
	{
		#region Fields
		private SortedSet<int> empties = new SortedSet<int>();
		private int slotsRequired;
		private Collection<GemNew> baseGems = new Collection<GemNew>();
		#endregion

		#region Public Properties
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
		#endregion

		#region Public Methods
		public void AddBaseGem(GemNew gem)
		{
			this.baseGems.Add(gem);
			this.slotsRequired++;
		}

		public bool Combine(GemNew parentGem)
		{
			ThrowNull(parentGem, nameof(parentGem));
			var slot1 = this.DuplicateIfNeeded(parentGem.Components[0]);
			var slot2 = this.DuplicateIfNeeded(parentGem.Components[1]);
			var dupeHappened = slot1 != parentGem.Components[0].Slot || slot2 != parentGem.Components[1].Slot;
			if (slot2 > slot1)
			{
				var temp = slot1;
				slot1 = slot2;
				slot2 = temp;
			}

			this.Add(new Instruction(ActionType.Combine, slot1, slot2));
			this.empties.Add(slot1);
			parentGem.Slot = slot2;
			return dupeHappened;
		}

		public bool Upgrade(GemNew gem)
		{
			ThrowNull(gem, nameof(gem));
			var slot = this.DuplicateIfNeeded(gem.Components[0]);
			this.Add(new Instruction(ActionType.Upgrade, slot));
			var dupeHappened = gem.Slot != slot;
			gem.Slot = slot;
			return dupeHappened;
		}
		#endregion

		#region Private Methods
		private int DuplicateIfNeeded(GemNew gem)
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
		#endregion
	}
}
