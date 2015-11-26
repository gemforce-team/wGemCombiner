namespace WGemCombiner
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using System.Text;
	using System.Text.RegularExpressions;
	using static Globals;

	#region Public Enums
	[Flags]
	public enum GemColors
	{
		None,
		Orange = 1,
		Yellow = 1 << 1,
		Black = 1 << 2,
		Red = 1 << 3,
		Mana = Orange | Black | Red,
		Kill = Yellow | Black | Red
	}
	#endregion

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Represents a tree node, so makes more sense to name in the singular.")]
	public class Gem : Collection<Gem>
	{
		#region Static Fields
		// private static Regex equationLine = new Regex(@"\(val=\d+\)\t?\d+=(<lhs>\d+)\+(<rhs>\d+)");
		private static SortedDictionary<char, GemColors> gemTypes = new SortedDictionary<char, GemColors>()
		{
			['b'] = GemColors.Black,
			['k'] = GemColors.Kill,
			['m'] = GemColors.Mana,
			['o'] = GemColors.Orange,
			['r'] = GemColors.Red,
			['y'] = GemColors.Yellow,
		};

		private static SortedDictionary<GemColors, string> gemNames = new SortedDictionary<GemColors, string>()
		{
			[GemColors.Black] = "Black",
			[GemColors.Kill] = "Kill gem",
			[GemColors.Mana] = "Mana gem",
			[GemColors.Orange] = "Orange",
			[GemColors.Red] = "Red",
			[GemColors.Yellow] = "Yellow",
		};

		private static StringBuilder combineBuilder = new StringBuilder();
		#endregion

		#region Fields
		// Components
		private double blood;
		private double critMult;
		private double damage; // max damage
		private double leech;
		#endregion

		#region Constructors
		public Gem(char letter)
		{
			var color = gemTypes[letter];
			this.Letter = letter;
			this.Color = color;
			this.GradeGrowth = 0;
			this.Cost = 1;
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

		public Gem(Gem gem1, Gem gem2)
		{
			ThrowNull(gem1, nameof(gem1));
			ThrowNull(gem2, nameof(gem2));
			gem1.UseCount++;
			gem2.UseCount++;
			if (gem2.Cost > gem1.Cost)
			{
				this.Add(gem2);
				this.Add(gem1);
			}
			else
			{
				this.Add(gem1);
				this.Add(gem2);
			}

			if (gem1.Color == gem2.Color)
			{
				this.Color = gem1.Color;
			}
			else
			{
				if (gem1.Color == GemColors.Kill || gem2.Color == GemColors.Kill || gem1.Color == GemColors.Yellow || gem2.Color == GemColors.Yellow)
				{
					// Since the colors aren't the same and yellow is involved, this must be a kill gem.
					this.Color = GemColors.Kill;
				}
				else if (gem1.Color == GemColors.Mana || gem2.Color == GemColors.Mana || gem1.Color == GemColors.Orange || gem2.Color == GemColors.Orange)
				{
					// Since the colors aren't the same and orange is involved, this must be a mana gem.
					this.Color = GemColors.Mana;
				}
				else
				{
					this.Color = GemColors.Red; // Ignore any black component for now and call this red. Since it's not yellow or orange, it'll still be picked up as part of a kill/mana gem in subsequent combines.
				}
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

		private Gem()
		{
		}
		#endregion

		#region Public Properties
		public GemColors Color { get; }

		public string ColorName => gemNames[this.Color];

		public string CombineTitle => string.Format(CultureInfo.CurrentCulture, "{0:000000} ({1:0.00000}){2}", this.Cost, this.Growth, IsPowerOfTwo(this.Cost) ? "-" : string.Empty);

		public int Cost { get; set; }

		public int GradeGrowth { get; set; }

		public double Growth { get; set; } = 1; // ???? Math.Log10(1.379) / Math.Log10(2);

		public int Id { get; set; }

		public bool IsBaseGem => this.GradeGrowth == 0;

		public char Letter { get; }

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

		public int Slot { get; set; }

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

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Lengthy operation that should not be a property.")]
		public string GetFullCombine()
		{
			combineBuilder.Clear();
			this.DoFullCombine();
			var retval = combineBuilder.ToString();
			combineBuilder.Clear();
			return retval;
		}
		#endregion

		#region Public Override Methods
		public override string ToString() => this.CombineTitle;
		#endregion

		#region Private Static Methods
		private static double CombineCalc(double value1, double value2, double multHigh, double multLow) => value1 > value2 ? (multHigh * value1) + (multLow * value2) : (multHigh * value2) + (multLow * value1);

		private static bool IsPowerOfTwo(int cost) => (cost != 0) && (cost & (cost - 1)) == 0;
		#endregion

		#region Private Methods
		private void DoFullCombine()
		{
			foreach (var component in this)
			{
				component.DoSubCombine();
				combineBuilder.Append('+');
			}

			combineBuilder.Remove(combineBuilder.Length - 1, 1);
		}

		private void DoSubCombine()
		{
			if (this.IsBaseGem)
			{
				combineBuilder.Append(this.Letter);
			}
			else
			{
				combineBuilder.Append("(");
				this.DoFullCombine();
				combineBuilder.Append(")");
			}
		}
		#endregion
	}
}