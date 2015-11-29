namespace WGemCombiner
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using static Globals;

	public class InstructionCollection : List<Instruction>
	{
		#region Constructors
		public InstructionCollection()
		{
		}

		internal InstructionCollection(int condenserSlot)
		{
			if (condenserSlot >= 0)
			{
				// Assumes base gems will be in lower slots than the condenser gem slot, which should always be the case.
				for (int i = 0; i < condenserSlot; i++)
				{
					this.Empties.Add(i);
				}

				this.SlotsRequired = condenserSlot + 1;
			}
		}
		#endregion

		#region Public Properties
		public Collection<GemNew> BaseGems { get; } = new Collection<GemNew>();

		public SortedSet<int> Empties { get; } = new SortedSet<int>();

		public int SlotsRequired { get; internal set; }

		public bool UseOldBehavior { get; set; }
		#endregion

		#region Public Methods
		public bool Combine(GemNew parentGem)
		{
			ThrowNull(parentGem, nameof(parentGem));
			if (this.UseOldBehavior)
			{
				return this.CombineOld(parentGem);
			}

			var slot2 = this.DuplicateIfNeeded(parentGem.Components[1]);
			var slot1 = this.DuplicateIfNeeded(parentGem.Components[0]);
			var dupeHappened = slot1 != parentGem.Components[0].Slot || slot2 != parentGem.Components[1].Slot;
			this.Add(new Instruction(ActionType.Combine, slot1, slot2));
			this.Empties.Add(slot1);
			parentGem.Slot = slot2;
			return dupeHappened;
		}

		public void Move1A(GemNew gem)
		{
			ThrowNull(gem, nameof(gem));
			if (this.Count > 0)
			{
				var lastInstruction = this[this.Count - 1];
				if (lastInstruction.To != 0)
				{
					this.Add(new Instruction(ActionType.Move, lastInstruction.To, 0));
				}
			}
		}

		public void Reset()
		{
			this.Clear();
			this.Empties.Clear();
			this.SlotsRequired = 0;
			foreach (var gem in this.BaseGems)
			{
				this.SlotsRequired = gem.Slot + 1;
			}
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
			var dupeHappened = gem.Components[0].Slot != slot;
			gem.Slot = slot;
			return dupeHappened;
		}
		#endregion

		#region Internal Methods
		internal void AddBaseGem(GemNew gem)
		{
			this.BaseGems.Add(gem);
			this.Empties.Remove(gem.Slot);
			if (this.SlotsRequired <= gem.Slot)
			{
				this.SlotsRequired = gem.Slot + 1;
			}
		}
		#endregion

		#region Private Methods
		private bool CombineOld(GemNew parentGem)
		{
			var slot1 = this.DuplicateIfNeeded(parentGem.Components[0]);
			var slot2 = this.DuplicateIfNeeded(parentGem.Components[1]);
			var dupeHappened = slot1 != parentGem.Components[0].Slot || slot2 != parentGem.Components[1].Slot;
			this.Add(new Instruction(ActionType.Combine, parentGem.Components[0].Slot, parentGem.Components[1].Slot));
			this.Empties.Add(parentGem.Components[0].Slot);
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
				int slot;
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
