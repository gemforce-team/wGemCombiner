namespace WGemCombiner
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class BaseGem : Gem
	{
		#region Static Fields
		private static SortedDictionary<GemColors, double> gemDamages = new SortedDictionary<GemColors, double>()
		{
			// Percent damage for base gems compared to Yellow
			[GemColors.Black] = 1.18181818181818,
			[GemColors.Generic] = 0,
			[GemColors.Kill] = 1,
			[GemColors.Mana] = 0,
			[GemColors.Orange] = 0.7272727272727272,
			[GemColors.Red] = .909090909090909,
			[GemColors.Yellow] = 1,
		};

		private static SortedDictionary<char, GemColors> gemTypes = new SortedDictionary<char, GemColors>()
		{
			['b'] = GemColors.Black,
			['g'] = GemColors.Generic,
			['k'] = GemColors.Kill,
			['m'] = GemColors.Mana,
			['o'] = GemColors.Orange,
			['r'] = GemColors.Red,
			['y'] = GemColors.Yellow,
		};
		#endregion

		#region Fields
		private string letter;
		#endregion

		#region Constructors
		public BaseGem(char letter)
		{
			GemColors color;
			if (!gemTypes.TryGetValue(letter, out color))
			{
				throw new ArgumentOutOfRangeException(nameof(letter), "Invalid letter value for gem: " + letter);
			}

			this.Color = color;
			this.Cost = 1;
			this.IsBaseGem = true;
			this.Slot = 0;

			this.Blood = color.HasFlag(GemColors.Black) ? 1 : 0;
			this.CriticalMultiplier = color.HasFlag(GemColors.Yellow) ? 1 : 0;
			this.Leech = color.HasFlag(GemColors.Orange) ? 1 : 0;
			this.Damage = gemDamages[color];

			this.letter = letter.ToString();
		}
		#endregion

		#region Public Override Properties
		public override bool IsPureUpgrade => true;

		public override bool IsUpgrade => false;
		#endregion

		#region Public Override Methods
		public override string Recipe() => this.letter;
		#endregion
	}
}
