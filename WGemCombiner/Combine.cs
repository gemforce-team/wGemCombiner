namespace WGemCombiner
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Text;
	using System.Text.RegularExpressions;

	// New class for combining gems; made distinct from CombinePerformer so that class can strictly be about interacting with GC while this one is for internal use
	public class Combine : List<Gem>
	{
		#region Static Fields
		private static Regex equationParser = new Regex(@"(<lhs>\d+)\s*=\s*(<rhs>\d+)");
		private static Regex gemPower = new Regex(@"g?(?<num>[0-9]+)\s*(?<color>([a-z]|\([a-z]+\)))");
		#endregion

		#region Fields
		private int slotsRequired = 0;
		#endregion

		#region Public Properties
		public IReadOnlyCollection<Gem> BaseGems
		{
			get
			{
				var list = new List<Gem>();
				foreach (var gem in this)
				{
					if (gem.IsBaseGem)
					{
						list.Add(gem);
					}
					else
					{
						break;
					}
				}

				return list;
			}
		}

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

		#region Public Static Methods
		public static Combine FromParentheses(string recipe)
		{
			var retval = new Combine();
			recipe = recipe.Replace(" ", string.Empty); // Remove spaces, whitespace is for human readers.
			recipe = recipe.Replace("\n", string.Empty); // Remove newlines or the parser crashes
			recipe = LeveledPreparser(recipe);

			return retval;
		}
		#endregion

		#region Public Methods
		public void Add(string equation)
		{
			var match = equationParser.Match(equation);
			if (!match.Success)
			{
				throw new ArgumentException("Invalid equation: " + equation);
			}

			var lhs = int.Parse(match.Groups["lhs"].Value);
			var rhs = int.Parse(match.Groups["rhs"].Value);

			if (lhs > this.Count - 1 || rhs > this.Count - 1)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Gem values in equation {0} are higher than the gem count of {1}.", equation, this.Count));
			}

			this.Add(new Gem(this[lhs], this[rhs]));
		}

		public void AddRange(string equations)
		{
			foreach (var line in equations.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
			{
				this.Add(line);
			}
		}
		#endregion

		#region Private Static Methods
		private static string LeveledPreparser(string recipe)
		{
			// Replaces leveled gems (e.g., "3o" becomes "((o+o)+(o+o))")
			var sb = new StringBuilder();
			var replacements = new SortedDictionary<int, string>();
			int lastPos = 0;
			foreach (Match match in gemPower.Matches(recipe))
			{
				var num = int.Parse(match.Groups["num"].Value, CultureInfo.InvariantCulture);
				var color = match.Groups["color"].Value;
				string newColor;
				if (num == 1)
				{
					newColor = color;
				}
				else
				{
					if (!replacements.TryGetValue(num, out newColor))
					{
						if (num == 2)
						{
							newColor = "*";
						}
						else
						{
							newColor = replacements[replacements.Count + 1];
						}

						for (int i = replacements.Count + 2; i <= num; i++)
						{
							newColor = newColor.Replace("*", "(*+*)");
							replacements.Add(i, newColor);
						}
					}

					newColor = newColor.Replace("*", color);
				}

				sb.Append(recipe.Substring(lastPos, match.Index - lastPos));
				sb.Append(newColor);
				lastPos = match.Index + match.Length;
			}

			return lastPos == 0 ? recipe : sb.Append(recipe.Substring(lastPos)).ToString();
		}
		#endregion
	}
}
