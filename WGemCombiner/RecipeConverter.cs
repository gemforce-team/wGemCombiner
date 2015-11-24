namespace WGemCombiner
{
	using System.Collections.Generic;
	using System.IO;

	internal static class RecipeConverter
	{
		#region Static Fields
		private static CombinePerformer combinePerformer = new CombinePerformer(true);
		#endregion

		#region Public Methods
		public static void ConvertFromFile(string importFilePath)
		{
			if (string.IsNullOrEmpty(importFilePath))
			{
				return;
			}

			using (StreamReader recipeReader = new StreamReader(importFilePath))
			{
				int recipeNumber = 0;
				string currentRecipe;
				do
				{
					recipeNumber++;
					currentRecipe = recipeReader.ReadLine()?.Trim();
					if (!string.IsNullOrEmpty(currentRecipe))
					{
						/* if (!CombinePerformer.SchemeIsValid(currentRecipe))
						{
							continue;
						} */

						ConvertRecipe(currentRecipe);
					}
				}
				while (currentRecipe != null);
			}

			System.Windows.Forms.MessageBox.Show("Done, check the log in /Resources");
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "English-only filename.")]
		public static void ConvertRecipe(string recipe)
		{
			string outputPath = @"..\..\..\Resources\";
			combinePerformer.Parse(recipe);
			var gem = combinePerformer.ResultGem;
			var isCombine = new List<Gem>(combinePerformer.BaseGems).Count == 1;
			switch (gem.Color)
			{
				case GemColor.Mana:
					if (isCombine)
					{
						outputPath += @"mgComb\mg";
					}
					else
					{
						outputPath += @"mgSpec\mg";
					}

					break;
				case GemColor.Kill:
					if (isCombine)
					{
						outputPath += @"kgComb\kg";
					}
					else
					{
						outputPath += @"kgSpec\kg";
					}

					break;
				default:
					{
						var colorName = combinePerformer.ResultGem.ColorName.ToLowerInvariant();
						outputPath += colorName + @"\" + colorName;
						isCombine = true;
					}

					break;
			}

			outputPath += combinePerformer.ResultGem.Cost + (isCombine ? "C" : string.Empty);
			combinePerformer.Save(outputPath);
		}
		#endregion
	}
}