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
        static StreamWriter logger;

        static public void convertFromFile(string importFilePath)
        {
            int recipeNumber = 0;
            string currentRecipe = "";
            StreamReader recipeReader = new StreamReader(importFilePath);
            logger = new StreamWriter(@"..\..\..\Resources\importLog.txt");
            logger.AutoFlush = true;

            while((currentRecipe = recipeReader.ReadLine())!=null)
            {
                logger.Write( "Converting Recipe # " + ++recipeNumber + ":\t");
                convertRecipe(currentRecipe);
            }

            recipeReader.Close();
            logger.WriteLine("\nFinished importing!");
            logger.Close();
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
            logger.Write( "Written recipe to " + writtenFilePath + "\n");
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
