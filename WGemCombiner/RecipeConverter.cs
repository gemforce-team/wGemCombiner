using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WGemCombiner
{
    static class RecipeConverter
    {
        static CombinePerformer CP = new CombinePerformer();
        static string importListing = "";

        static public void convertFromFile(string importFilePath)
        {
            int recipeNumber = 0;
            string currentRecipe = "";
            StreamReader recipeReader = new StreamReader(importFilePath);
            while((currentRecipe = recipeReader.ReadLine())!=null)
            {
                importListing += "Converting Recipe # " + ++recipeNumber + ":\t";
                convertRecipe(currentRecipe);
            }
            recipeReader.Close();
            System.Windows.Forms.MessageBox.Show(importListing);
        }

        static public void convertRecipe(string recipe)
        {
            gemType type;
            string outputPath = @"..\..\..\Resources";
            bool isCombine = false;

            CP.SetMethod(recipe, false);
            type = (gemType)CP.resultGem.GetColor();
            switch (type)
            {
                case gemType.mg:
                    if (recipe.Contains('m'))
                    {
                        outputPath += @"\mgComb\";
                        isCombine = true;
                    }
                    else
                        outputPath += @"\mgSpec\";
                    outputPath += "mg";
                    break;
                case gemType.kg:
                    if (recipe.Contains('k'))
                    {
                        outputPath += @"\kgComb\";
                        isCombine = true;
                    }
                    else
                        outputPath += @"\kgSpec\";
                    outputPath += "kg";
                    break;
                default:
                    {
                        outputPath += @"\" + type.ToString() + @"\" + type.ToString();
                        isCombine=true;
                    }
                    break;
            }
            writePreset(outputPath,isCombine);
        }

        static private void writePreset(string path, bool isCombine)
        {
            System.IO.File.WriteAllBytes(path + CP.resultGem.Cost + (isCombine?"C":""), CP.GetSave());
            importListing += "Written recipe to " + path + CP.resultGem.Cost + (isCombine ? "C" : "") + "\n";
        }
    }

    enum gemType
    {
        leech=1,
        mg=3,
        yellow=4,
        kg=5,
    }
}
