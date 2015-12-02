namespace WGemCombiner
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using static Globals;

	#region Public Enums
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification = "In this context, Generic makes more sense than None.")]
	[Flags]
	public enum GemColors
	{
		Generic = 0,
		Orange = 1,
		Yellow = 1 << 1,
		Black = 1 << 2,
		Red = 1 << 3,
		Mana = Orange | Black | Red,
		Kill = Yellow | Black | Red
	}
	#endregion

	public class Gem
	{
		#region Constructors
		public Gem(Gem gem1, Gem gem2)
		{
			ThrowNull(gem1, nameof(gem1));
			ThrowNull(gem2, nameof(gem2));
			var components = new List<Gem>(2);
			components.Add(gem1);
			components.Add(gem2);
			this.Components = components;

			foreach (var component in this.Components)
			{
				this.Color |= component.Color;
				this.IsSpec |= component.IsSpec || component.Color != this.Color;
				component.UseCount++;
			}

			if (gem1.Grade == gem2.Grade)
			{
				this.Grade = gem1.Grade + 1;
				this.Damage = CombineCalc(gem1.Damage, gem2.Damage, 0.87, 0.71);
				this.Blood = CombineCalc(gem1.Blood, gem2.Blood, 0.78, 0.31);
				this.CriticalMultiplier = CombineCalc(gem1.CriticalMultiplier, gem2.CriticalMultiplier, 0.88, 0.5);
				this.Leech = CombineCalc(gem1.Leech, gem2.Leech, 0.88, 0.5);
			}
			else if (Math.Abs(gem1.Grade - gem2.Grade) == 1)
			{
				this.Grade = gem1.Grade > gem2.Grade ? gem1.Grade : gem2.Grade;
				this.Damage = CombineCalc(gem1.Damage, gem2.Damage, 0.86, 0.7);
				this.Blood = CombineCalc(gem1.Blood, gem2.Blood, 0.79, 0.29);
				this.CriticalMultiplier = CombineCalc(gem1.CriticalMultiplier, gem2.CriticalMultiplier, 0.88, 0.44);
				this.Leech = CombineCalc(gem1.Leech, gem2.Leech, 0.89, 0.44);
			}
			else
			{
				this.Grade = gem1.Grade > gem2.Grade ? gem1.Grade : gem2.Grade;
				this.Damage = CombineCalc(gem1.Damage, gem2.Damage, 0.85, 0.69);
				this.Blood = CombineCalc(gem1.Blood, gem2.Blood, 0.8, 0.27);
				this.CriticalMultiplier = CombineCalc(gem1.CriticalMultiplier, gem2.CriticalMultiplier, 0.88, 0.44);
				this.Leech = CombineCalc(gem1.Leech, gem2.Leech, 0.9, 0.38);
			}

			this.Damage = Math.Max(this.Damage, Math.Max(gem1.Damage, gem2.Damage));
			this.Cost = gem1.Cost + gem2.Cost;
			if (this.IsSpec)
			{
				this.Growth = this.Power / Math.Pow(this.Cost, this.Color == GemColors.Mana ? 0.627216 : 1.414061);
			}
			else
			{
				this.Growth = Math.Log(this.Power, this.Cost);
			}
		}

		protected Gem()
		{
		}
		#endregion

		#region Public Properties
		public GemColors Color { get; protected set; }

		public IReadOnlyList<Gem> Components { get; }

		public int Cost { get; protected set; }

		public int Grade { get; }

		public double Growth { get; }

		public bool IsBaseGem { get; protected set; } = false;

		public bool IsSpec { get; protected set; }

		public bool IsNeeded => this.Slot < 0 && this.UseCount > 0; // This has the side-effect of also ruling out base gems automatically

		public virtual bool IsPureUpgrade => this.IsUpgrade && this.Components[0].IsPureUpgrade && this.Components[1].IsPureUpgrade;

		public virtual bool IsUpgrade => this.Components[0] == this.Components[1];

		public double Power
		{
			get
			{
				if (this.Color == GemColors.Red)
				{
					return 0;
				}

				double power = 1;
				if (this.Color.HasFlag(GemColors.Black))
				{
					power *= this.Blood;
				}

				if (this.Color.HasFlag(GemColors.Orange))
				{
					power *= this.Leech;
				}

				if (this.Color.HasFlag(GemColors.Yellow))
				{
					power *= this.Damage * this.CriticalMultiplier;

					if (this.Color.HasFlag(GemColors.Black))
					{
						// blood is squared here
						power *= this.Blood;
					}
				}

				return power;
			}
		}

		public int Slot { get; set; }

		public string Title => string.Format(CultureInfo.CurrentCulture, "{0:0000000} ({1:0.00000}){2}", this.Cost, this.Growth, IsPowerOfTwo(this.Cost) ? "-" : string.Empty);

		public int UseCount { get; set; }
		#endregion

		#region Internal Static Properties
		internal static string GemInitializer { get; } = "oykmgbr";
		#endregion

		#region Protected Properties
		protected double Blood { get; set; }

		protected double CriticalMultiplier { get; set; }

		protected double Damage { get; set; } // max damage

		protected double Leech { get; set; }
		#endregion

		#region Public Methods
		public string DisplayInfo(bool showAll, int slots)
		{
			var retval = string.Format(CultureInfo.CurrentCulture, "Grade: +{0}\r\nCost: {1}x\r\nGrowth: {2:0.0####}\r\nSlots: {3}", this.Grade, this.Cost, this.Growth, slots);
			if (showAll)
			{
				retval += string.Format(CultureInfo.CurrentCulture, "\r\nPower: {0:0.0####}\r\nDamage: {1:0.0####}\r\nLeech: {2:0.0####}\r\nCrit: {3:0.0####}\r\nBbound: {4:0.0####}", this.Power, this.Damage, this.Leech, this.CriticalMultiplier, this.Blood);
			}

			return retval;
		}

		public virtual string Recipe() => "(" + this.Components[0].Recipe() + "+" + this.Components[1].Recipe() + ")";
		#endregion

		#region Public Override Methods
		public override string ToString() => string.Format(CultureInfo.CurrentCulture, "Grade {0} {1} (cost={2})", this.Grade + 1, this.Color, this.Cost);
		#endregion

		#region Private Static Methods
		private static double CombineCalc(double value1, double value2, double multHigh, double multLow) => value1 > value2 ? (multHigh * value1) + (multLow * value2) : (multHigh * value2) + (multLow * value1);

		private static bool IsPowerOfTwo(int cost) => (cost != 0) && (cost & (cost - 1)) == 0;
		#endregion

	}
}