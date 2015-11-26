namespace WGemCombiner
{
	using System.Collections.Generic;
	using System.IO;
	using System.Reflection;
	using System.Resources;
	using static Globals;

	// Deprecated
	internal static class Preset
	{
		#region Static Fields
		private static Assembly assembly = Assembly.GetExecutingAssembly();
		#endregion

		#region Public Methods
		public static IEnumerable<Gem> ReadResources(string embeddedResourceName, string letters)
		{
			var entries = new List<Gem>();
			ThrowNull(embeddedResourceName, nameof(embeddedResourceName));
			ThrowNull(letters, nameof(letters));
			using (ResourceReader resourceReader = new ResourceReader(assembly.GetManifestResourceStream(embeddedResourceName)))
			{
				var dict = resourceReader.GetEnumerator();
				while (dict.MoveNext())
				{
					var gemCombines = new List<Instruction>();
					using (MemoryStream memoryStream = new MemoryStream((byte[])dict.Value))
					using (BinaryReader binaryStream = new BinaryReader(memoryStream))
					{
						while (memoryStream.Position < memoryStream.Length)
						{
							gemCombines.Add(Instruction.NewFromStream(binaryStream));
						}
					}

					List<Gem> gems = new List<Gem>();
					gems.Add(null);
					for (int i = 0; i < gemCombines[0].From; i++)
					{
						gems.Add(new Gem(letters[i]));
					}

					Gem lastGem = null;
					for (int i = 1; i < gemCombines.Count; i++)
					{
						var combine = gemCombines[i];
						Gem c1 = gems[combine.From];
						Gem c2 = gems[combine.To];
						lastGem = new Gem(c1, c2);
						gems.Add(lastGem);
					}

					entries.Add(lastGem);
				}
			}

			return entries;
		}
		#endregion
	}
}