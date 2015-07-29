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

        static public void convertFromFile(string importFilePath)
        {
            int recipeNumber = 0;
            string currentRecipe = "";
            StreamReader recipeReader = new StreamReader(importFilePath);

            while((currentRecipe = recipeReader.ReadLine())!=null)
            {
                Form1.logger.Write( "Converting Recipe # " + ++recipeNumber + ":\t");
                if(!CP.schemeIsValid(currentRecipe))
                {
                    Form1.logger.Write("Scheme is not valid, skipping...\n");
                    continue;
                }
                convertRecipe(currentRecipe);
            }

            recipeReader.Close();
            Form1.logger.WriteLine("\nFinished importing!");
            System.Windows.Forms.MessageBox.Show("Done, check the log in /Resources");
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
                    break;
                case gemType.kg:
                    if (recipe.Contains('k'))
                    {
                        outputPath += @"\kgComb\";
                        isCombine = true;
                    }
                    else
                        outputPath += @"\kgSpec\";
                    break;
                default:
                    {
                        outputPath += @"\" + type.ToString() + @"\";
                        isCombine=true;
                    }
                    break;
            }
            outputPath += type.ToString(); // Adding the beginning of the preset's filename
            writePreset(outputPath,isCombine);
        }

        static private void writePreset(string path, bool isCombine)
        {
            string writtenFilePath = path + CP.resultGem.Cost + (isCombine ? "C" : "");

            System.IO.File.WriteAllBytes(writtenFilePath, CP.GetSave());
            Form1.logger.Write( "Written recipe to " + writtenFilePath + "\n");
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
