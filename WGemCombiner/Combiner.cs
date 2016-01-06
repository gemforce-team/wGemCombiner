namespace WGemCombiner
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Text;
	using System.Text.RegularExpressions;
	using static Globals;

	/// <summary>The Combiner class represents a single recipe as a whole. This is normally equivalent to an entire set of equations, though the slot condenser uses multiple combiners to create intermediate sets of instructions that are then combined into a larger set of instructions later on.</summary>
	// TODO: Consider whether CreateInstructions/CondenseSlots belong here or in InstructionCollection, or whether the two should be entirely combined.
	public class Combiner
	{
		#region Static Fields
		private static Regex equationParser = new Regex(@"((\(val=\d+\))?(?<index>\d+)=)?((?<lhs>\d+)\+(?<rhs>\d+)|g?\d*(?<letter>[a-z]))", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
		private static Regex gemPower = new Regex(@"g?(?<num>[0-9]+)\s*(?<color>([a-z]|\([a-z]+\)))", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
		private static Regex invalidLetterFinder = new Regex(@"[a-z][^\+\)]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		#endregion

		#region Fields
		private List<Gem> gems = new List<Gem>();
		#endregion

		#region Constructors
		public Combiner(IEnumerable<string> equations, bool doGesFixup)
		{
			ThrowNull(equations, nameof(equations));
			var dupeCheck = new HashSet<int>();
			var redGemIndex = -1;
			foreach (var spacedLine in equations)
			{
				var line = new Regex(@"\s+").Replace(spacedLine, string.Empty); // Remove all whitespace.
				if (string.IsNullOrEmpty(line))
				{
					// Allow blank lines at this level, even though at file level, a blank line indicates a break between recipes. Allows that someone else might not want to conform to this particular standard.
					continue;
				}

				var match = equationParser.Match(line);
				if (!match.Success)
				{
					throw new ArgumentException("Invalid equation: " + line);
				}

				var indexGroup = match.Groups["index"];
				if (indexGroup.Success)
				{
					var index = int.Parse(indexGroup.Value, CultureInfo.InvariantCulture);
					if (index != this.gems.Count)
					{
						throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Index in equation {0} does not match current gem count of {1}.", index, this.gems.Count));
					}
				}

				var letter = match.Groups["letter"].Value;
				if (letter.Length != 0)
				{
					if (doGesFixup && letter[0] == 'r')
					{
						redGemIndex = this.gems.Count;
						this.gems.Add(new BaseGem('h'));
					}
					else
					{
						this.gems.Add(new BaseGem(letter[0]));
					}
				}
				else
				{
					var lhs = int.Parse(match.Groups["lhs"].Value, CultureInfo.InvariantCulture);
					var rhs = int.Parse(match.Groups["rhs"].Value, CultureInfo.InvariantCulture);
					bool parseLine = true;
					if (doGesFixup && redGemIndex >= 0)
					{
						if (lhs == redGemIndex)
						{
							parseLine = false;
						}
						else if (lhs > redGemIndex)
						{
							lhs--;
						}

						if (rhs == redGemIndex)
						{
							parseLine = false;
						}
						else if (rhs > redGemIndex)
						{
							rhs--;
						}
					}

					if (parseLine)
					{
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
			}

			if (this.gems.Count > 0)
			{
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
			else
			{
				this.Gem = null;
			}
		}

		private Combiner(Gem parentGem, IReadOnlyList<Gem> gemList, bool lastRun)
		{
			ThrowNull(parentGem, nameof(parentGem));
			ThrowNull(gemList, nameof(gemList));
			this.Gem = parentGem;

			// Only update UseCount for gems actually used in this combiner.
			// First, determine which gems are actually part of this combine. Hijacking UseCount as a marker, since we're going to be resetting it right after this anyway (and those in the base list don't matter anymore).
			foreach (var gem in gemList)
			{
				gem.UseCount = 0;
			}

			this.Gem.UseCount = -1;
			bool changesMade;
			do
			{
				changesMade = false;
				for (int i = gemList.Count - 1; i >= 0; i--)
				{
					var gem = gemList[i];
					if (gem.UseCount == -1 && !(gem is BaseGem))
					{
						foreach (var component in gem)
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

			this.ResetUseCount(!lastRun);
		}
		#endregion

		#region Public Static Properties
		public static int NotSlotted { get; } = -1;

		public static int SlotLimit { get; set; } = 36;
		#endregion

		#region Public Properties
		public IEnumerable<BaseGem> BaseGems
		{
			get
			{
				foreach (var gem in this.gems)
				{
					var baseGem = gem as BaseGem;
					if (baseGem != null)
					{
						yield return baseGem;
					}
				}
			}
		}

		public Gem Gem { get; private set; }

		public string Title { get; set; }
		#endregion

		#region Public Static Methods
		public static IEnumerable<string> EquationsFromParentheses(string recipe)
		{
			var equations = new List<string>();
			if (string.IsNullOrWhiteSpace(recipe))
			{
				return equations;
			}

			recipe = "(" + recipe + ")"; // If this is a duplication of effort, we'll silently ignore it later.
			recipe = new Regex(@"\s+").Replace(recipe, string.Empty); // Remove all whitespace.
			recipe = LeveledPreparser(recipe);

			var multiLetterMatch = invalidLetterFinder.Match(recipe);
			if (multiLetterMatch.Success)
			{
				throw new ArgumentException("The gem \"" + multiLetterMatch.Value + "\" must be a single letter. Gem combinations must be expressed as individual components separated with plus signs and brackets, as needed. For example:\nob → o+b\nob+o → (o+b)+o");
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
		public InstructionCollection CreateInstructions()
		{
			this.ResetUseCount(false);
			var instructions = new InstructionCollection(this.BaseGems);
			if (this.Gem != null)
			{
				this.Gem.UseCount = 1;
			}

			this.BuildGem(this.Gem, instructions, true);
			if (instructions.SlotsRequired > SlotLimit)
			{
				instructions = this.CondenseSlots(new List<Gem>(this.BaseGems), true);
				instructions.WasCondensed = true;
			}
			else
			{
				instructions.WasCondensed = false;
			}

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
					throw new ArgumentException(match.Value + " is too high to be parsed via a parenthesis recipe. Try converting your recipe to equations.");
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
		private void BuildGem(Gem gem, InstructionCollection instructions, bool doPostScan)
		{
			if (gem == null || !gem.IsNeeded || instructions.SlotsRequired > SlotLimit)
			{
				return;
			}

			var gem1 = gem.Component1;
			var gem2 = gem.Component2;
			this.BuildGem(gem1, instructions, true);
			this.BuildGem(gem2, instructions, true);

			// Re-check if gem is needed in case gem was already built during optimization routine for component gems.
			if (gem.IsNeeded && instructions.SlotsRequired <= SlotLimit)
			{
				gem1.UseCount--;
				gem2.UseCount--;
				doPostScan &= gem.IsUpgrade ? instructions.Upgrade(gem) : instructions.Combine(gem);

				while (doPostScan && instructions.SlotsRequired <= SlotLimit)
				{
					doPostScan = this.OptimizeLastGems(instructions) || this.OptimizeSingleUseGems(instructions);
				}
			}
		}

		/// <summary>Attempts to create individual gems in the tree from the base gem, then combines them into the larger one. This allows combining larger gems that won't fit into 36 slots all at once, at the cost of increasing the number of instructions necessary to do so.</summary>
		/// <param name="gemsToIgnore">A list of gems that are already slotted from previous runs. The base call to CondenseSlots should pass the base gems only.</param>
		/// <param name="lastRun">Indicates whether or not this is the last node in the tree. The base call to CondenseSlots should always pass true. Internally, this is always passed as false for the first node of a gem, and passed as the parent value for the second node. Thus, it will be true only for the second node of the second node of the second node...etc.</param>
		/// <returns>The list of instructions to create the costliest possible gem in the number of slots allowed.</returns>
		private InstructionCollection CondenseSlots(ICollection<Gem> gemsToIgnore, bool lastRun)
		{
			var combine1 = new Combiner(this.Gem.Component1, this.gems, false);
			combine1.Gem.UseCount++;
			var instructions1 = new InstructionCollection(gemsToIgnore);
			combine1.BuildGem(combine1.Gem, instructions1, true);
			combine1.Gem.UseCount--;
			if (gemsToIgnore.Count >= SlotLimit)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "You cannot build this gem with {0} slots.", SlotLimit));
			}

			if (instructions1.SlotsRequired > SlotLimit)
			{
				instructions1 = combine1.CondenseSlots(gemsToIgnore, false);
			}

			gemsToIgnore.Add(combine1.Gem);
			var combine2 = new Combiner(this.Gem.Component2, this.gems, lastRun);
			combine2.Gem.UseCount++;
			var instructions2 = new InstructionCollection(gemsToIgnore);
			combine2.BuildGem(combine2.Gem, instructions2, lastRun);
			combine2.Gem.UseCount--;
			if (gemsToIgnore.Count >= SlotLimit)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "You cannot build this gem with {0} slots.", SlotLimit));
			}

			if (instructions2.SlotsRequired > SlotLimit)
			{
				instructions2 = combine2.CondenseSlots(gemsToIgnore, lastRun);
			}

			gemsToIgnore.Remove(combine1.Gem);

			return new InstructionCollection(instructions1, instructions2, this.Gem);
		}

		private bool OptimizeLastGems(InstructionCollection instructions)
		{
			var optimized = false;
			foreach (var gem in this.gems)
			{
				if (gem.IsNeeded && gem.Component1.Slot != Combiner.NotSlotted && gem.Component2.Slot != Combiner.NotSlotted && (gem.IsUpgrade && gem.Component1.UseCount == 2 || gem.Component1.UseCount == 1 && gem.Component2.UseCount == 1))
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
				if (gem.IsNeeded && gem.Component1.Slot != Combiner.NotSlotted && gem.Component2.Slot != Combiner.NotSlotted && (gem.Component1.UseCount == 1 || gem.Component2.UseCount == 1))
				{
					optimized = true;
					this.BuildGem(gem, instructions, false);
				}
			}

			return optimized;
		}

		private void ResetUseCount(bool preserveBaseGems)
		{
			// Put base gems into slots; reset all others to not slotted
			var baseGems = new List<BaseGem>();
			foreach (var gem in this.gems)
			{
				var baseGem = gem as BaseGem;
				if (baseGem != null)
				{
					baseGems.Add(baseGem);
				}
				else
				{
					gem.Slot = NotSlotted;
				}
			}

			// Ensure slot order consistency whether we get 0=r; 1=y; 2=0+1 or 0=y; 1=r; 2=0+1.
			baseGems.Sort((g1, g2) => g1.Color.CompareTo(g2.Color));
			for (int slot = 0; slot < baseGems.Count; slot++)
			{
				var gem = baseGems[slot];
				gem.Slot = slot;
				gem.OriginalSlot = slot;
			}

			foreach (var gem in this.gems)
			{
				gem.UseCount = gem is BaseGem && preserveBaseGems ? 1 : 0;
			}

			foreach (var gem in this.gems)
			{
				if (!(gem is BaseGem))
				{
					gem.Component1.UseCount++;
					gem.Component2.UseCount++;
				}
			}
		}
		#endregion
	}
}