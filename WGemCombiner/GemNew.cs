namespace WGemCombiner
{
	using System;
	using System.Collections.Generic;
	using static Globals;

	public class GemNew
	{
		#region Fields
		// Components
		private double blood;
		private double critMult;
		private double damage; // max damage
		private double leech;
		#endregion

		#region Constructors
		public GemNew(GemColors color)
		{
			this.Color = color;
			this.Cost = 1;
			this.Slot = 0;

			this.damage = 0; // gems that need damage will have that properly setted (dmg_yellow=1)
			if (color == GemColors.Black)
			{
				this.damage = 1.186168;
				this.blood = 1.0;
			}
			else if (color == GemColors.Kill)
			{
				this.damage = 1.0;
				this.critMult = 1.0;
				this.blood = 1.0;
			}
			else if (color == GemColors.Mana)
			{
				this.leech = 1.0;
				this.blood = 1.0;
			}
			else if (color == GemColors.Orange)
			{
				this.leech = 1.0;
			}
			else if (color == GemColors.Yellow)
			{
				this.damage = 1.0;
				this.critMult = 1.0;
			}
			else if (color == GemColors.Red)
			{
				this.damage = 0.909091;
			}
		}

		public GemNew(GemNew gem1, GemNew gem2)
		{
			ThrowNull(gem1, nameof(gem1));
			ThrowNull(gem2, nameof(gem2));
			if (gem2.Cost > gem1.Cost)
			{
				this.Components.Add(gem2);
				this.Components.Add(gem1);
			}
			else
			{
				this.Components.Add(gem1);
				this.Components.Add(gem2);
			}

			foreach (var component in this.Components)
			{
				this.Color |= component.Color;
				component.UseCount++;
			}

			this.GradeGrowth = (gem1.GradeGrowth > gem2.GradeGrowth) ? gem1.GradeGrowth : gem2.GradeGrowth;
			if (gem1.GradeGrowth == gem2.GradeGrowth)
			{
				this.GradeGrowth++;
				this.damage = CombineCalc(gem1.damage, gem2.damage, 0.87, 0.71);
				this.blood = CombineCalc(gem1.blood, gem2.blood, 0.78, 0.31);
				this.critMult = CombineCalc(gem1.critMult, gem2.critMult, 0.88, 0.5);
				this.leech = CombineCalc(gem1.leech, gem2.leech, 0.88, 0.5);
			}
			else if (Math.Abs(gem1.GradeGrowth - gem2.GradeGrowth) == 1)
			{
				this.damage = CombineCalc(gem1.damage, gem2.damage, 0.86, 0.7);
				this.blood = CombineCalc(gem1.blood, gem2.blood, 0.79, 0.29);
				this.critMult = CombineCalc(gem1.critMult, gem2.critMult, 0.88, 0.44);
				this.leech = CombineCalc(gem1.leech, gem2.leech, 0.89, 0.44);
			}
			else
			{
				this.damage = CombineCalc(gem1.damage, gem2.damage, 0.85, 0.69);
				this.blood = CombineCalc(gem1.blood, gem2.blood, 0.8, 0.27);
				this.critMult = CombineCalc(gem1.critMult, gem2.critMult, 0.88, 0.44);
				this.leech = CombineCalc(gem1.leech, gem2.leech, 0.9, 0.38);
			}

			this.damage = Math.Max(this.damage, Math.Max(gem1.damage, gem2.damage));
			this.Cost = gem1.Cost + gem2.Cost;
			this.Growth = Math.Log(this.Power, this.Cost);
		}
		#endregion

		#region Public Properties
		public bool CanCreate
		{
			// Concept only - should really be part of the collection checking when combining.
			get
			{
				if (this.Slot >= 0 || this.UseCount == 0)
				{
					return false;
				}

				var existing = new HashSet<GemNew>();
				foreach (var gem in this.GemList)
				{
					if (gem.Slot >= 0)
					{
						existing.Add(gem);
					}
				}

				return existing.IsSupersetOf(this.Components);
			}
		}

		public GemColors Color { get; }

		public ICollection<GemNew> Components { get; } = new List<GemNew>();

		public int Cost { get; set; }

		public IEnumerable<GemNew> GemList { get; }

		public int GradeGrowth { get; }

		public double Growth { get; }

		public bool IsBaseGem => this.GradeGrowth == 0;

		public double Power
		{
			get
			{
				switch (this.Color)
				{
					case GemColors.Orange:
						return this.leech;
					case GemColors.Black:
						return this.blood;
					case GemColors.Mana:
						return this.leech * this.blood;
					case GemColors.Yellow:
						return this.damage * this.critMult;
					case GemColors.Kill:
						return this.damage * this.critMult * this.blood * this.blood; // 1+blood makes no sense, g1 bb is set to 1 by convention
					default:
						return 0; // Red have no mana or kill power
				}
			}
		}

		/// <summary>Gets the slot requirement to create this gem NOT accounting for the possible need to duplicate any gems along the way.</summary>
		public int SlotRequirement
		{
			get
			{
				if (this.Slot < 0)
				{
					int slots = 0;
					foreach (var component in this.Components)
					{
						slots += component.SlotRequirement;
					}

					return slots;
				}
				else
				{
					return 1; // If we're already in a slot, we obviously take up a single slot.
				}
			}
		}

		public int Slot { get; private set; } = -1;

		public int UseCount { get; set; }
		#endregion

		#region Public Methods
		public void FakeCreate()
		{
			this.Slot = 0;
			foreach (var gem in this.Components)
			{
				gem.UseCount--;
				if (gem.UseCount == 0)
				{
					gem.Slot = -1;
				}
			}
		}
		#endregion

		#region Private Static Methods
		private static double CombineCalc(double value1, double value2, double multHigh, double multLow) => value1 > value2 ? (multHigh * value1) + (multLow * value2) : (multHigh * value2) + (multLow * value1);

		private static bool IsPowerOfTwo(int cost) => (cost != 0) && (cost & (cost - 1)) == 0;
		#endregion

	}
}