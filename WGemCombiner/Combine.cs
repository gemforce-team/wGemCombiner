namespace WGemCombiner
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Globalization;
	using System.Text;
	using System.Text.RegularExpressions;

	// New class for combining gems; made distinct from CombinePerformer so that class can strictly be about interacting with GC while this one is for internal use
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Represents a concrete concept rather than a simply a GemCollection.")]
	internal class Combine : List<GemNew>
	{
		#region Static Fields
		private static Regex equationParser = new Regex(@"(?<index>\d+)\s*=\s*((?<lhs>\d+)\s*\+\s*(?<rhs>\d+)|g?\d*\s*(?<letter>[bkmory]))");
		private static Regex gemPower = new Regex(@"g?(?<num>[0-9]+)\s*(?<color>([a-z]|\([a-z]+\)))");
		#endregion

		#region Constructors
		public Combine()
		{
		}

		public Combine(string recipe)
		{
			if (recipe != null)
			{
				if (!recipe.Contains("="))
				{
					recipe = EquationsFromParentheses(recipe);
				}

				this.AddRange(recipe);
			}
		}
		#endregion

		#region Public Static Properties
		public static int NotSlotted { get; } = -1;
		#endregion

		#region Public Properties
		public GemNew Result
		{
			get
			{
				return this[this.Count - 1];
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
			recipe = recipe.Replace(" ", string.Empty); // Remove spaces, whitespace is for human readers.
			recipe = recipe.Replace("\n", string.Empty); // Remove newlines or the parser crashes
			recipe = LeveledPreparser(recipe);

			var id = 0;
			foreach (var c in "oybrkm")
			{
				if (recipe.IndexOf(c) > -1)
				{
					recipe = recipe.Replace(c, id.ToString(CultureInfo.InvariantCulture)[0]);
					equations.Add(string.Format(CultureInfo.InvariantCulture, "{0}={1}", id, c));
					id++;
				}
			}

			// Scan for plus signs within the formula and add gems together appropriately.
			int plus = recipe.IndexOf('+');
			while (plus > -1)
			{
				string thisCombine;
				var close = recipe.IndexOf(')', plus);
				if (close == -1)
				{
					thisCombine = recipe;
					recipe = "(" + recipe + ")"; // Add brackets so the final replace will work
				}
				else
				{
					var open = recipe.LastIndexOf('(', close);
					thisCombine = recipe.Substring(open + 1, close - open - 1);
				}

				string[] combineGems = thisCombine.Split('+');
				if (combineGems.Length != 2)
				{
					throw new ArgumentException("The formula provided contains more than a single plus sign within a pair of brackets. This is not currently supported.", nameof(recipe));
				}

				var newNum = id.ToString(CultureInfo.InvariantCulture);
				recipe = recipe.Replace("(" + thisCombine + ")", newNum);
				equations.Add(string.Format(CultureInfo.InvariantCulture, "{0}={1}+{2}", id, combineGems[0], combineGems[1]));

				plus = recipe.IndexOf('+');
				id++;
			}

			return string.Join(Environment.NewLine, equations);
		}

		public static Combine FromParentheses(string recipe)
		{
			var retval = new Combine();
			var equations = EquationsFromParentheses(recipe);
			retval.AddRange(equations);
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

			var index = int.Parse(match.Groups["index"].Value, CultureInfo.InvariantCulture);
			if (index != this.Count)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Index in equation {0} does not match current gem count of {1}.", index, this.Count));
			}

			var letter = match.Groups["letter"].Value;
			if (letter.Length != 0)
			{
				this.Add(new GemNew(letter[0]));
			}
			else
			{
				var lhs = int.Parse(match.Groups["lhs"].Value, CultureInfo.InvariantCulture);
				var rhs = int.Parse(match.Groups["rhs"].Value, CultureInfo.InvariantCulture);

				if (lhs > this.Count - 1 || rhs > this.Count - 1)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Gem values in equation {0} are higher than the gem count of {1}.", equation, this.Count));
				}

				this.Add(new GemNew(this[lhs], this[rhs]));
			}
		}

		public void AddRange(string equations)
		{
			if (!string.IsNullOrWhiteSpace(equations))
			{
				foreach (var line in equations.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
				{
					this.Add(line);
				}
			}
		}

		public InstructionCollection GetInstructions()
		{
			var instructions = new InstructionCollection();
			for (int i = 0; i < this.Count; i++)
			{
				var gem = this[i];
				if (gem.IsBaseGem)
				{
					instructions.AddBaseGem(gem);
					gem.Slot = i;
				}
				else
				{
					gem.Slot = NotSlotted;
				}
			}

			// No guarantee that base gems are all in the lowest equations, so start at 0.
			// this.Result.UseCount = 1;
			this.BuildGem(this.Result, instructions);

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

		#region Private Methods
		// Select the costliest branch of a gem and build it.
		private void BuildGem(GemNew gem, InstructionCollection instructions)
		{
			var gem1 = gem.Components[0];
			var gem2 = gem.Components[1];
			if (gem2.Cost > gem1.Cost)
			{
				var temp = gem1;
				gem1 = gem2;
				gem2 = temp;
			}

			if (gem1.IsNeeded)
			{
				this.BuildGem(gem1, instructions);
			}

			if (gem2.IsNeeded)
			{
				this.BuildGem(gem2, instructions);
			}

			// Gem may have already been pre-built due to optimizations after dupeHappened.
			if (!gem.IsNeeded)
			{
				return;
			}

			Debug.WriteLine("(val={0}) {1}={2}+{3}", gem.Cost, this.IndexOf(gem), this.IndexOf(gem.Components[0]), this.IndexOf(gem.Components[1]));
			gem.Components[0].UseCount--;
			gem.Components[1].UseCount--;
			var dupeHappened = gem.IsUpgrade ? instructions.Upgrade(gem) : instructions.Combine(gem);
			if (dupeHappened)
			{
				// While either of these optimizations happened, try again until all optimizations have been handled.
				// bool repeatOptimization;
				// do
				// {
					// repeatOptimization = false;
					bool foundOne;
					do
					{
						foundOne = false;
						foreach (var listGem in this)
						{
							if (listGem.IsNeeded && listGem.LastTwo)
							{
								// repeatOptimization = true;
								foundOne = true;
								this.BuildGem(listGem, instructions);
							}
						}
					}
					while (foundOne);

					do
					{
						foundOne = false;
						foreach (var listGem in this)
						{
							if (listGem.IsNeeded && (listGem.Components[0].Slot >= 0 && listGem.Components[0].UseCount == 1 || listGem.Components[1].Slot >= 0 && listGem.Components[1].UseCount == 1))
							{
								// repeatOptimization = true;
								foundOne = true;
								this.BuildGem(listGem, instructions);
							}
						}
					}
					while (foundOne);
				// }
				// while (repeatOptimization);
			}

			foreach (var component in gem.Components)
			{
				if (component.UseCount == 0)
				{
					component.Slot = NotSlotted;
				}
			}
		}

		/*
		// Logic was too primitive and resulted in very unoptimal builds. First portion might have some use in the future, though, since it establishes what we have no choice but to build during the first steps.
		private int GetPreferredGemIndex()
		{
			var canCreate = new List<int>();

			// TODO: Consider not having i start at 0 every time, but updating it as you go to start at the lowest gem not already created.
			for (int i = 0; i < this.Count; i++)
			{
				var gem = this[i];
				if (gem.UseCount > 0 && gem.Slot < 0 && gem.Components[0].Slot >= 0 && gem.Components[1].Slot >= 0)
				{
					canCreate.Add(i);
				}
			}

			// If there's only one possible gem that can be created, choose that one.
			if (canCreate.Count == 1)
			{
				Debug.WriteLine(canCreate[0]);
				return canCreate[0];
			}
		}
		*/
		#endregion
	}
}
