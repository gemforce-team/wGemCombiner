namespace WGemCombiner
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using static Globals;

	public class GemNew
	{
		#region Static Fields
		private static Dictionary<GemColors, double> gemDamages = new Dictionary<GemColors, double>()
		{
			[GemColors.Black] = 1.18181818181818,
			[GemColors.Kill] = 1,
			[GemColors.Mana] = 0,
			[GemColors.Orange] = 0,
			[GemColors.Red] = .90909090909091,
			[GemColors.Yellow] = 1,
		};
		#endregion

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
			this.blood = color.HasFlag(GemColors.Black) ? 1 : 0;
			this.critMult = color.HasFlag(GemColors.Yellow) ? 1 : 0;
			this.leech = color.HasFlag(GemColors.Orange) ? 1 : 0;
			this.damage = gemDamages[color]; // Will throw an error if someone passes an odd gem color like Yellow | Black as a base gem, which is fine cuz they should never actually do that.
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

		private GemNew()
		{
		}
		#endregion

		#region Public Static Properties
		// Do we want this in this class or just locally to whatever uses it?
		public static SortedDictionary<GemColors, char> GemLetters { get; } = new SortedDictionary<GemColors, char>()
		{
			[GemColors.Black] = 'b',
			[GemColors.Kill] = 'k',
			[GemColors.Mana] = 'm',
			[GemColors.Orange] = 'o',
			[GemColors.Red] = 'r',
			[GemColors.Yellow] = 'y'
		};
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
		public string DisplayInfo(bool showAll, int slots)
		{
			var retval = string.Format(CultureInfo.CurrentCulture, "Grade: +{0}\r\nCost: {1}x\r\nGrowth: {2:0.0####}\r\nSlots: {3}", this.GradeGrowth, this.Cost, this.Growth, slots);
			if (showAll)
			{
				retval += string.Format(CultureInfo.CurrentCulture, "\r\nPower: {0:0.0####}\r\nDamage: {1:0.0####}\r\nLeech: {2:0.0####}\r\nCrit: {3:0.0####}\r\nBbound: {4:0.0####}", this.Power, this.damage, this.leech, this.critMult, this.blood);
			}

			return retval;
		}

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

		#region Public Override Methods
		public override string ToString() => string.Format(CultureInfo.CurrentCulture, "Grade {0} {1}", this.GradeGrowth + 1, this.Color);
		#endregion

		#region Private Static Methods
		private static double CombineCalc(double value1, double value2, double multHigh, double multLow) => value1 > value2 ? (multHigh * value1) + (multLow * value2) : (multHigh * value2) + (multLow * value1);
		#endregion

	}
}