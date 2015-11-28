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

		public bool UseOldBehavior { get; set; }
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
			if (this.UseOldBehavior)
			{
				return this.CombineOld(parentGem);
			}

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
			if (this.UseOldBehavior)
			{
				return this.UpgradeOld(gem);
			}

			var slot = this.DuplicateIfNeeded(gem.Components[0]);
			this.Add(new Instruction(ActionType.Upgrade, slot));
			var dupeHappened = gem.Slot != slot;
			gem.Slot = slot;
			return dupeHappened;
		}
		#endregion

		#region Private Methods
		private bool CombineOld(GemNew parentGem)
		{
			var slot1 = this.DuplicateIfNeeded(parentGem.Components[0]);
			var slot2 = this.DuplicateIfNeeded(parentGem.Components[1]);
			var dupeHappened = slot1 != parentGem.Components[0].Slot || slot2 != parentGem.Components[1].Slot;
			this.Add(new Instruction(ActionType.Combine, parentGem.Components[0].Slot, parentGem.Components[1].Slot));
			this.empties.Add(parentGem.Components[0].Slot);
			parentGem.Slot = parentGem.Components[1].Slot;
			parentGem.Components[0].Slot = parentGem.Components[0].UseCount == 0 ? -1 : slot1;
			parentGem.Components[1].Slot = parentGem.Components[1].UseCount == 0 ? -1 : slot2;

			return dupeHappened;
		}

		private int DuplicateIfNeeded(GemNew gem)
		{
			ThrowNull(gem, nameof(gem));
			if (gem.UseCount > 0)
			{
				// Dupe if not the last use (two uses = gem.Components[0] + gem.Components[1])
				var slot = this.empties.Count == 0 ? this.SlotsRequired : this.empties.Min;
				this.SlotsRequired = slot;
				this.Add(new Instruction(ActionType.Duplicate, gem.Slot, slot));
				this.empties.Remove(slot);

				return slot;
			}

			return gem.Slot;
		}

		private bool UpgradeOld(GemNew gem)
		{
			var slot = this.DuplicateIfNeeded(gem.Components[0]);
			var dupeHappened = gem.Components[0].Slot != slot;
			this.Add(new Instruction(ActionType.Upgrade, gem.Components[0].Slot));
			gem.Slot = gem.Components[0].Slot;
			gem.Components[0].Slot = slot;
			return dupeHappened;
		}
		#endregion
	}
}
