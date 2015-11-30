namespace WGemCombiner
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Windows.Forms;
	// New class for combining gems; made distinct from CombinePerformer so that class can strictly be about interacting with GC while this one is for internal use
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Represents a concrete concept rather than a simply a GemCollection.")]
	public class Combiner
	{
		#region Static Fields
		private static Regex equationParser = new Regex(@"(?<index>\d+)\s*=\s*((?<lhs>\d+)\s*\+\s*(?<rhs>\d+)|g?\d*\s*(?<letter>[a-z]))", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
		private static Regex gemPower = new Regex(@"g?(?<num>[0-9]+)\s*(?<color>([a-z]|\([a-z]+\)))", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
		#endregion

		#region Fields
		private List<Gem> gems = new List<Gem>();
		#endregion

		#region Constructors
		public Combiner(string recipe)
		{
			this.AddRecipe(recipe);
		}

		public Combiner(IEnumerable<string> equations)
		{
			this.AddEquations(equations);
		}

		public Combiner(Gem parentGem, IList<Gem> baseGems)
		{
			this.Gem = parentGem;
			this.BaseGems = baseGems;
		}
		#endregion

		#region Public Static Properties
		public static int NotSlotted { get; } = -1;

		public static int SlotLimit { get; set; } = 36; // Unlike previous combiner, this does not change as the slot condenser progresses.
		#endregion

		#region Public Properties
		public IList<Gem> BaseGems { get; } = new List<Gem>(1);

		public Gem Gem { get; private set; }
		#endregion

		#region Public Static Methods
		public static IList<string> EquationsFromParentheses(string recipe)
		{
			var equations = new List<string>();
			if (string.IsNullOrWhiteSpace(recipe))
			{
				return equations;
			}

			recipe = "(" + recipe + ")"; // If this is a duplication of effort, we'll silently ignore it later.
			recipe = new Regex(@"\s+").Replace(recipe, string.Empty); // Remove all whitespace.
			recipe = LeveledPreparser(recipe);

			var multiLetterMatch = new Regex("[a-z]{2,}").Match(recipe);
			if (multiLetterMatch.Success)
			{
				throw new ArgumentException("Gems must be a single letter. Gem combinations like \"" + multiLetterMatch.Value + "\" must be expressed as separate components separated with plus signs and brackets, as needed.");
			}

			var id = 0;
			foreach (var c in Gem.GemInitializer)
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
						throw new ArgumentException("The formula provided contains more than a single plus sign within a pair of brackets or a term that is over-bracketed (e.g., \"((o+o))\"). These are not currently supported.");
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

			return equations;
		}
		#endregion

		#region Public Methods
		public InstructionCollection GetInstructions()
		{
			this.ResetUseCount(false);
			var instructions = new InstructionCollection(this.BaseGems);
			this.Gem.UseCount = 1;
			this.BuildGem(this.Gem, instructions, true);
			if (instructions.SlotsRequired > SlotLimit)
			{
				instructions = this.CondenseSlots();
				instructions.OptimizeCondensedBaseGems(this.BaseGems);
			}

			instructions.Move1A(this.Gem);

#if DEBUG
			try
			{
				this.SlotBaseGems();
				instructions.Verify(this.BaseGems);
			}
			catch (InvalidOperationException e)
			{
				MessageBox.Show(e.Message, "Verification failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
#endif

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
							newColor = replacements[replacements.Count + 1]; // Plus one because key values start at 2, not 0.
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
		private void AddEquations(string equations)
		{
			if (!string.IsNullOrWhiteSpace(equations))
			{
				this.AddEquations(equations.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
			}
		}

		private void AddEquations(IEnumerable<string> equations)
		{
			this.Clear();
			var dupeCheck = new HashSet<int>();
			foreach (var line in equations)
			{
				if (string.IsNullOrWhiteSpace(line))
				{
					// Allow blank lines at this level, even though at file level, a blank line indicates a break between recipes. Allows that someone else might not want to conform to this particular standard.
					continue;
				}

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
					var baseGem = new BaseGem(letter[0]);
					this.gems.Add(baseGem);
					this.BaseGems.Add(baseGem);

				}
				else
				{
					var lhs = int.Parse(match.Groups["lhs"].Value, CultureInfo.InvariantCulture);
					var rhs = int.Parse(match.Groups["rhs"].Value, CultureInfo.InvariantCulture);
					if (lhs > this.gems.Count - 1 || rhs > this.gems.Count - 1)
					{
						throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Gem values in equation {0} must be less than {1}.", line, this.gems.Count));
					}

					this.gems.Add(new Gem(this.gems[lhs], this.gems[rhs]));
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

		private void AddRecipe(string recipe)
		{
			if (!recipe.Contains("="))
			{
				this.AddEquations(EquationsFromParentheses(recipe));
			}
			else
			{
				this.AddEquations(recipe);
			}
		}

		private void BuildGem(Gem gem, InstructionCollection instructions, bool doPostScan)
		{
			if (!gem.IsNeeded)
			{
				return;
			}

			var gem1 = gem.Components[0];
			var gem2 = gem.Components[1];
			this.BuildGem(gem1, instructions, true);
			this.BuildGem(gem2, instructions, true);

			// Re-check if gem is needed in case gem was already built during optimization routine for component gems.
			if (gem.IsNeeded)
			{
				gem1.UseCount--;
				gem2.UseCount--;
				doPostScan &= gem.IsUpgrade ? instructions.Upgrade(gem) : instructions.Combine(gem);

				while (doPostScan)
				{
					doPostScan = this.OptimizeLastGems(instructions) || this.OptimizeSingleUseGems(instructions);
				}
			}
		}

		private void Clear()
		{
			this.gems.Clear();
			this.BaseGems.Clear();
		}

		private InstructionCollection CondenseSlots()
		{
			var combine1 = new Combiner(this.Gem.Components[0], this.BaseGems);
			combine1.SetupForSlotCondenser(this.gems);
			combine1.Gem.UseCount++;
			var instructions1 = new InstructionCollection(this.BaseGems);
			combine1.BuildGem(combine1.Gem, instructions1, true);
			if (instructions1.SlotsRequired > SlotLimit)
			{
				instructions1 = combine1.CondenseSlots();
			}

			var combine2 = new Combiner(this.Gem.Components[1], this.BaseGems);
			combine2.SetupForSlotCondenser(this.gems);
			combine2.Gem.UseCount++;
			var instructions2 = new InstructionCollection(this.BaseGems, combine1.Gem.Slot);
			combine2.BuildGem(combine2.Gem, instructions2, true);
			if (instructions2.SlotsRequired > SlotLimit)
			{
				instructions2 = combine2.CondenseSlots();
			}

			instructions1.AddRange(instructions2);
			instructions1.Combine(this.Gem);
			if (instructions2.SlotsRequired > instructions1.SlotsRequired)
			{
				instructions1.SlotsRequired = instructions2.SlotsRequired;
			}

			return instructions1;
		}

		private bool OptimizeLastGems(InstructionCollection instructions)
		{
			var optimized = false;
			foreach (var gem in this.gems)
			{
				if (gem.IsNeeded && gem.Components[0].Slot >= 0 && gem.Components[1].Slot >= 0 && (gem.IsUpgrade && gem.Components[0].UseCount == 2 || gem.Components[0].UseCount == 1 && gem.Components[1].UseCount == 1))
				{
					optimized = true;
					this.BuildGem(gem, instructions, false);
				}
			}

			return optimized;
		}

		private bool OptimizeSingleUseGems(InstructionCollection instructions)
		{
			var optimized = false;
			foreach (var gem in this.gems)
			{
				if (gem.IsNeeded && gem.Components[0].Slot >= 0 && gem.Components[1].Slot >= 0 && (gem.Components[0].UseCount == 1 || gem.Components[1].UseCount == 1))
				{
					optimized = true;
					this.BuildGem(gem, instructions, false);
				}
			}

			return optimized;
		}

		private void ResetUseCount(bool preserveBaseGems)
		{
			this.SlotBaseGems();
			foreach (var gem in this.gems)
			{
				gem.UseCount = 0;
			}

			foreach (var gem in this.gems)
			{
				if (gem.IsBaseGem)
				{
					if (preserveBaseGems)
					{
						gem.UseCount++;
					}
				}
				else
				{
					gem.Components[0].UseCount++;
					gem.Components[1].UseCount++;
				}
			}
		}

		private void SetupForSlotCondenser(List<Gem> gemList)
		{
			foreach (var gem in this.gems)
			{
				gem.UseCount = 0;
			}

			// Only update UseCount for gems actually used in this combiner even though all gems are inherited from parent.
			// First, determine which gems are actually part of this combine. Hijacking UseCount as a marker, since we're going to be resetting it right after this anyway (and those in the base list don't matter).
			this.Gem.UseCount = -1;
			bool changesMade;
			do
			{
				changesMade = false;
				for (int i = gemList.Count - 1; i >= 0; i--)
				{
					var gem = gemList[i];
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

			this.gems = new List<Gem>();
			foreach (var gem in gemList)
			{
				if (gem.UseCount == -1)
				{
					this.gems.Add(gem);
				}
			}

			this.ResetUseCount(true);
		}

		private void SlotBaseGems()
		{
			var slot = 0;
			foreach (var gem in this.gems)
			{
				if (gem.IsBaseGem)
				{
					gem.Slot = slot;
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