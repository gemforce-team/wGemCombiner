namespace WGemCombiner
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Text;
	using System.Text.RegularExpressions;

	// New class for combining gems; made distinct from CombinePerformer so that class can strictly be about interacting with GC while this one is for internal use
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Represents a concrete concept rather than a simply a GemCollection.")]
	internal class Combine
	{
		#region Static Fields
		private static Regex equationParser = new Regex(@"(?<index>\d+)\s*=\s*((?<lhs>\d+)\s*\+\s*(?<rhs>\d+)|g?\d*\s*(?<letter>[a-z]))");
		private static Regex gemPower = new Regex(@"g?(?<num>[0-9]+)\s*(?<color>([a-z]|\([a-z]+\)))");
		#endregion

		#region Fields
		private List<GemNew> gems = new List<GemNew>();
		#endregion

		#region Constructors
		public Combine(GemNew parentGem, List<GemNew> gemList, int condenserSlot)
		{
			this.Gem = parentGem;
			this.Instructions = new InstructionCollection(condenserSlot);
			this.SetupForSlotCondenser(gemList);
		}

		public Combine(string recipe)
		{
			if (string.IsNullOrWhiteSpace(recipe))
			{
				throw new ArgumentNullException(nameof(recipe), "Combine recipe was blank.");
			}

			this.Instructions = new InstructionCollection();
			if (!recipe.Contains("="))
			{
				recipe = EquationsFromParentheses(recipe);
			}

			this.AddRecipe(recipe);
		}
		#endregion

		#region Public Static Properties
		public static int NotSlotted { get; } = -1;

		public static int SlotLimit { get; set; } = 36; // Unlike previous combiner, this does not change as the slot condenser progresses.
		#endregion

		#region Public Properties
		public GemNew Gem { get; private set; }

		public InstructionCollection Instructions { get; private set; }
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
				else
				{
					throw new ArgumentException("Bracket mismatch in formula.");
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
			var dupeCheck = new HashSet<int>();
			this.gems.Clear();
			if (!string.IsNullOrWhiteSpace(equations))
			{
				foreach (var line in equations.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
				{
					var match = equationParser.Match(line);
					if (!match.Success)
					{
						throw new ArgumentException("Invalid equation: " + line);
					}

					var index = int.Parse(match.Groups["index"].Value, CultureInfo.InvariantCulture);
					if (index != this.gems.Count)
					{
						throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Index in equation {0} does not match current gem count of {1}.", index, this.gems.Count));
					}

					var letter = match.Groups["letter"].Value;

					if (letter.Length != 0)
					{
						this.gems.Add(new GemNew(letter[0]));
					}
					else
					{
						var lhs = int.Parse(match.Groups["lhs"].Value, CultureInfo.InvariantCulture);
						var rhs = int.Parse(match.Groups["rhs"].Value, CultureInfo.InvariantCulture);
						if (lhs > this.gems.Count - 1 || rhs > this.gems.Count - 1)
						{
							throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Gem values in equation {0} must be less than {1}.", line, this.gems.Count));
						}

						this.gems.Add(new GemNew(this.gems[lhs], this.gems[rhs]));
						if (!dupeCheck.Add((lhs << 16) + rhs))
						{
							throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The equation {0}+{1} appears more than once.", lhs, rhs));
						}
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

				foreach (var gem in this.gems)
				{
					if (gem.UseCount == 0)
					{
						var index = this.gems.IndexOf(gem);
						if (index != this.gems.Count - 1)
						{
							throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Equation {0} is unused.", index));
						}
					}
				}

				this.Gem = this.gems[this.gems.Count - 1];
			}
		}

		public void GetInstructions()
		{
			this.Instructions.Reset();
			this.SlotBaseGems();
			this.Gem.UseCount = 1;
			this.BuildGem(this.Gem, true);
			if (this.Instructions.SlotsRequired > SlotLimit)
			{
				this.CondenseSlots();
			}

			this.Instructions.Move1A(this.Gem);
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
		// Select the costliest branch of a gem and build it.
		private void BuildGem(GemNew gem, bool doPostScan)
		{
			if (!gem.IsNeeded)
			{
				return;
			}

			var gem1 = gem.Components[0];
			var gem2 = gem.Components[1];
			this.BuildGem(gem1, true);
			this.BuildGem(gem2, true);

			// Re-check if gem is needed in case gem was already built during optimization routine for component gems.
			if (gem.IsNeeded)
			{
				gem1.UseCount--;
				gem2.UseCount--;
				doPostScan &= gem.IsUpgrade ? this.Instructions.Upgrade(gem) : this.Instructions.Combine(gem);

				foreach (var component in gem.Components)
				{
					if (component.UseCount == 0)
					{
						component.Slot = NotSlotted;
					}
				}

				while (doPostScan)
				{
					doPostScan = this.OptimizeLastGems() || this.OptimizeSingleUseGems();
				}
			}
		}

		private void CondenseSlots()
		{
			var combine1 = new Combine(this.Gem.Components[0], this.gems, -1);
			combine1.Gem.UseCount++;
			combine1.BuildGem(combine1.Gem, true);
			if (combine1.Instructions.SlotsRequired > SlotLimit)
			{
				combine1.CondenseSlots();
			}

			this.Instructions = combine1.Instructions;

			var combine2 = new Combine(this.Gem.Components[1], this.gems, combine1.Gem.Slot);
			combine2.Gem.UseCount++;
			combine2.BuildGem(combine2.Gem, true);
			if (combine2.Instructions.SlotsRequired > SlotLimit)
			{
				combine2.CondenseSlots();
			}

			this.Instructions.AddRange(combine2.Instructions);
			this.Instructions.Combine(this.Gem);
			this.Instructions.SlotsRequired = combine1.Instructions.SlotsRequired > combine2.Instructions.SlotsRequired ? combine1.Instructions.SlotsRequired : combine2.Instructions.SlotsRequired;
		}

		private bool OptimizeLastGems()
		{
			var optimized = false;
			foreach (var gem in this.gems)
			{
				if (gem.IsNeeded && gem.Components[0].Slot >= 0 && gem.Components[1].Slot >= 0 && (gem.IsUpgrade && gem.Components[0].UseCount == 2 || gem.Components[0].UseCount == 1 && gem.Components[1].UseCount == 1))
				{
					optimized = true;
					this.BuildGem(gem, false);
				}
			}

			return optimized;
		}

		private bool OptimizeSingleUseGems()
		{
			var optimized = false;
			foreach (var gem in this.gems)
			{
				if (gem.IsNeeded && gem.Components[0].Slot >= 0 && gem.Components[1].Slot >= 0 && (gem.Components[0].UseCount == 1 || gem.Components[1].UseCount == 1))
				{
					optimized = true;
					this.BuildGem(gem, false);
				}
			}

			return optimized;
		}

		private void SetupForSlotCondenser(List<GemNew> gems)
		{
			// Only update UseCount for gems actually used in this combiner even though all gems are inherited from parent.
			// First, determine which gems are actually part of this combine. Hijacking UseCount as a marker, since we're going to be resetting it right after this anyway (and those in the base list don't matter).
			this.Gem.UseCount = -1;
			bool changesMade;
			do
			{
				changesMade = false;
				for (int i = gems.Count - 1; i >= 0; i--)
				{
					var gem = gems[i];
					if (gem.UseCount == -1)
					{
						foreach (var component in gem.Components)
						{
							if (component.UseCount != -1)
							{
								changesMade = true;
								component.UseCount = -1;
							}
						}
					}
				}
			}
			while (changesMade);

			this.gems = new List<GemNew>();
			foreach (var gem in gems)
			{
				if (gem.UseCount == -1)
				{
					this.gems.Add(gem);
				}
			}

			this.SlotBaseGems();
			foreach (var gem in this.gems)
			{
				gem.UseCount = 0;
			}

			foreach (var gem in this.gems)
			{
				if (gem.IsBaseGem)
				{
					gem.UseCount++;
				}
				else
				{
					gem.Components[0].UseCount++;
					gem.Components[1].UseCount++;
				}
			}
		}

		private void SlotBaseGems()
		{
			var slot = 0;
			foreach (var gem in this.gems)
			{
				if (gem.IsBaseGem)
				{
					gem.Slot = slot;
					this.Instructions.AddBaseGem(gem);
					slot++;
				}
				else
				{
					gem.Slot = NotSlotted;
				}
			}
		}
		#endregion
	}
}
