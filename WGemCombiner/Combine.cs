namespace WGemCombiner
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Windows.Forms;
	using static Globals;

	// New class for combining gems; made distinct from CombinePerformer so that class can strictly be about interacting with GC while this one is for internal use
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Represents a concrete concept rather than a simply a GemCollection.")]
	internal class Combine
	{
		#region Static Fields
		private static Regex equationParser = new Regex(@"(?<index>\d+)\s*=\s*((?<lhs>\d+)\s*\+\s*(?<rhs>\d+)|g?\d*\s*(?<letter>[a-z]))");
		private static Regex gemPower = new Regex(@"g?(?<num>[0-9]+)\s*(?<color>([a-z]|\([a-z]+\)))");
		#endregion

		#region Fields
		private SortedSet<GemNew> gems = new SortedSet<GemNew>(new GemSorter());
		#endregion

		#region Constructors
		public Combine(GemNew parentGem)
		{
			this.AddGemTree(parentGem);
		}

		public Combine(string recipe)
		{
			if (string.IsNullOrWhiteSpace(recipe))
			{
				throw new ArgumentNullException(nameof(recipe), "Combine recipe was blank.");
			}

			if (!recipe.Contains("="))
			{
				recipe = EquationsFromParentheses(recipe);
			}

			this.AddRecipe(recipe);
		}
		#endregion

		#region Public Static Properties
		public static int NotSlotted { get; } = -1;
		#endregion

		#region Public Properties
		public GemNew Gem
		{
			get
			{
				return this.gems.Count == 0 ? null : this.gems.Max;
			}
		}
		#endregion

		#region Public Static Methods
		public static string EquationsFromParentheses(string recipe)
		{
			if (string.IsNullOrWhiteSpace(recipe))
			{
				return string.Empty;
			}

			var equations = new List<string>();
			recipe = "(" + recipe + ")"; // If this is a duplication of effort, we'll silently ignore it later.
			recipe = recipe.Replace(" ", string.Empty); // Remove spaces, whitespace is for human readers.
			recipe = recipe.Replace("\r", string.Empty);
			recipe = recipe.Replace("\n", string.Empty); // Remove newlines or the parser crashes
			recipe = LeveledPreparser(recipe);

			var id = 0;
			foreach (var c in GemNew.GemInitializer)
			{
				if (recipe.IndexOf(c) > -1)
				{
					recipe = recipe.Replace(c, id.ToString(CultureInfo.InvariantCulture)[0]);
					equations.Add(string.Format(CultureInfo.InvariantCulture, "{0}={1}", id, c));
					id++;
				}
			}

			if (equations.Count == 0)
			{
				throw new ArgumentException("Recipe did not contain any recognizable gems.");
			}

			// Scan for plus signs within the formula and add gems together appropriately.
			int plus = recipe.IndexOf('+');
			string newNum = (id - 1).ToString(CultureInfo.InvariantCulture);
			while (plus > -1)
			{
				string thisCombine;
				var close = recipe.IndexOf(')', plus);
				if (close >= 0)
				{
					var open = recipe.LastIndexOf('(', close);
					if (open < 0)
					{
						throw new ArgumentException("Bracket mismatch in formula.");
					}

					thisCombine = recipe.Substring(open + 1, close - open - 1);

					string[] combineGems = thisCombine.Split('+');
					if (combineGems.Length != 2)
					{
						throw new ArgumentException("The formula provided contains more than a single plus sign within a pair of brackets. This is not currently supported.");
					}

					if (combineGems[0].Length == 0 || combineGems[1].Length == 0)
					{
						throw new ArgumentException("Invalid formula part: (" + thisCombine + ")");
					}

					newNum = id.ToString(CultureInfo.InvariantCulture);
					recipe = recipe.Replace("(" + thisCombine + ")", newNum);
					equations.Add(string.Format(CultureInfo.InvariantCulture, "{0}={1}+{2}", id, combineGems[0], combineGems[1]));
				}

				plus = recipe.IndexOf('+');
				id++;
			}

			bool replacedBrackets;
			do
			{
				replacedBrackets = false;
				if (recipe.StartsWith("(", StringComparison.Ordinal) && recipe.EndsWith(")", StringComparison.Ordinal))
				{
					recipe = recipe.Substring(1, recipe.Length - 2);
					replacedBrackets = true;
				}
			}
			while (replacedBrackets);

			if (recipe != newNum)
			{
				if (recipe.Contains("("))
				{
					throw new ArgumentException("Bracket mismatch in formula.");
				}

				throw new ArgumentException("Invalid recipe.");
			}

			return string.Join(Environment.NewLine, equations);
		}
		#endregion

		#region Public Methods
		public void AddRecipe(string equations)
		{
			if (!string.IsNullOrWhiteSpace(equations))
			{
				var gemList = new List<GemNew>();
				foreach (var line in equations.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
				{
					var match = equationParser.Match(line);
					if (!match.Success)
					{
						throw new ArgumentException("Invalid equation: " + line);
					}

					var index = int.Parse(match.Groups["index"].Value, CultureInfo.InvariantCulture);
					if (index != gemList.Count)
					{
						throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Index in equation {0} does not match current gem count of {1}.", index, gemList.Count));
					}

					var letter = match.Groups["letter"].Value;
					if (letter.Length != 0)
					{
						gemList.Add(new GemNew(letter[0]));
					}
					else
					{
						var lhs = int.Parse(match.Groups["lhs"].Value, CultureInfo.InvariantCulture);
						var rhs = int.Parse(match.Groups["rhs"].Value, CultureInfo.InvariantCulture);

						if (lhs > gemList.Count - 1 || rhs > gemList.Count - 1)
						{
							throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Gem values in equation {0} must be less than {1}.", line, gemList.Count));
						}

						gemList.Add(new GemNew(gemList[lhs], gemList[rhs]));
					}
				}

				/*
				GemColors gemColor = gemList[gemList.Count - 1].Color;
				switch (gemColor)
				{
					// case GemColors.Black:
					case GemColors.Kill:
					case GemColors.Mana:
					case GemColors.Orange:
					// case GemColors.Red:
					case GemColors.Yellow:
						break;
					default:
						throw new ArgumentException("Invalid color or combination for base gems: " + gemColor.ToString());
				}
				*/

				this.gems.Clear();
				foreach (var gem in gemList)
				{
					var count = this.gems.Count;
					this.gems.Add(gem);

					if (gem.UseCount == 0)
					{
						var index = gemList.IndexOf(gem);
						if (index != gemList.Count - 1)
						{
							throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Equation {0} is unused.", index));
						}
					}

					if (this.gems.Count == count)
					{
						int duplicateIndex;
						for (duplicateIndex = 0; duplicateIndex < gemList.Count; duplicateIndex++)
						{
							var dupeGem = gemList[duplicateIndex];
							if (dupeGem.IsBaseGem)
							{
								if (gem.IsBaseGem && dupeGem.Color == gem.Color)
								{
									break;
								}
							}
							else
							{
								if (dupeGem.Components[0] == gem.Components[0] && dupeGem.Components[1] == gem.Components[1])
								{
									break;
								}
							}
						}

						throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The gem at equation {0} is identical to the gem at equation {1}.", gemList.IndexOf(gem), duplicateIndex));
					}
				}
			}
		}

		public InstructionCollection GetInstructions()
		{
			var instructions = new InstructionCollection();
			var slot = 0;
			foreach (var gem in this.gems)
			{
				if (gem.IsBaseGem)
				{
					instructions.AddBaseGem(gem);
					gem.Slot = slot;
					slot++;
				}
				else
				{
					gem.Slot = NotSlotted;
				}
			}

			// No guarantee that base gems are all in the lowest equations, so start at 0.
			this.Gem.UseCount = 1;
			this.BuildGem(this.Gem, instructions, true);

			return instructions;
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
				if (num > 15)
				{
					throw new ArgumentException(match.Value + " is too high to be parsed via a recipe. Try converting your recipe to equations.");
				}

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
						if (replacements.Count == 0)
						{
							newColor = "*";
						}
						else
						{
							newColor = replacements[replacements.Count - 1];
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

		#region Private Methods
		// Be sure that the existing gem list is cleared before calling this.
		private void AddGemTree(GemNew gem)
		{
			this.gems.Add(gem);
			if (!gem.IsBaseGem)
			{
				this.AddGemTree(gem.Components[0]);
				this.AddGemTree(gem.Components[1]);
			}
		}

		// Select the costliest branch of a gem and build it.
		private void BuildGem(GemNew gem, InstructionCollection instructions, bool doPostScan)
		{
			if (!gem.IsNeeded)
			{
				return;
			}

			// Debug.WriteLine("(val={0}) {1}={2}+{3}{4}", gem.Cost, this.gems.IndexOf(gem), this.gems.IndexOf(gem.Components[0]), this.gems.IndexOf(gem.Components[1]), doPostScan ? string.Empty : " (by optimizer)");
			var gem1 = gem.Components[0];
			var gem2 = gem.Components[1];

			// Debug.Indent();
			this.BuildGem(gem1, instructions, true);
			this.BuildGem(gem2, instructions, true);
			// Debug.Unindent();

			// Re-check in case gem was already built during optimization routine for component gems.
			if (gem.IsNeeded)
			{
				gem.Components[0].UseCount--;
				gem.Components[1].UseCount--;
				var dupeHappened = gem.IsUpgrade ? instructions.Upgrade(gem) : instructions.Combine(gem);

				foreach (var component in gem.Components)
				{
					if (component.UseCount == 0)
					{
						component.Slot = NotSlotted;
					}
				}

				if (doPostScan && dupeHappened)
				{
					this.PostCreateScan(instructions);
				}
			}
		}

		private void PostCreateScan(InstructionCollection instructions)
		{
			bool repeatOptimization;
			do
			{
				repeatOptimization = false;
				bool foundOne;
				do
				{
					foundOne = false;
					foreach (var listGem in this.gems)
					{
						if (listGem.IsNeeded && listGem.LastCombine)
						{
							repeatOptimization = true;
							foundOne = true;
							this.BuildGem(listGem, instructions, false);
						}
					}
				}
				while (foundOne);

				do
				{
					foundOne = false;
					foreach (var listGem in this.gems)
					{
						if (listGem.IsNeeded && listGem.Components[0].Slot >= 0 && listGem.Components[1].Slot >= 0 && (listGem.Components[0].UseCount == 1 || listGem.Components[1].UseCount == 1))
						{
							repeatOptimization = true;
							foundOne = true;
							this.BuildGem(listGem, instructions, false);
						}
					}
				}
				while (foundOne);
			}
			while (repeatOptimization);
		}
		#endregion

		#region Private Classes
		private class GemSorter : IComparer<GemNew>
		{
			public int Compare(GemNew x, GemNew y)
			{
				ThrowNull(x, nameof(x));
				ThrowNull(y, nameof(y));
				if (y.Cost > x.Cost)
				{
					return -1;
				}
				else if (y.Cost < x.Cost)
				{
					return 1;
				}
				else
				{
					if (y.Grade > x.Grade)
					{
						return -1;
					}
					else if (y.Grade < x.Grade)
					{
						return 1;
					}
					else
					{
						if (y.Color > x.Color)
						{
							return -1;
						}
						else if (y.Color < x.Color)
						{
							return 1;
						}
						else
						{
							return 0;
						}
					}
				}
			}
		}
		#endregion
	}
}
