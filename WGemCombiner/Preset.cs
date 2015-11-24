namespace WGemCombiner
{
	using System.Collections.Generic;
	using System.IO;
	using System.Reflection;
	using System.Resources;
	using static Globals;

	public class Preset
	{
		#region Static Fields
		private static Assembly assembly = Assembly.GetExecutingAssembly();
		#endregion

		#region Constructors
		/// <summary>Initializes a new instance of the <see cref="Preset"/> class from the files specified in the passed resource.resx, and adds the information to the presets.</summary>
		/// <param name="letters">The list of letters that go into the gem (e.g., "obr").</param>
		/// <param name="name">The friendly name of the group, for display purposes.</param>
		/// <param name="embeddedResourceFullName">Full path to the embedded assembly resource file containing the schemes</param>
		public Preset(string letters, string name, string embeddedResourceFullName)
		{
			ThrowNull(letters, nameof(letters));
			ThrowNull(name, nameof(name));
			ThrowNull(embeddedResourceFullName, nameof(embeddedResourceFullName));

			this.Name = name;
			using (ResourceReader resourceReader = new ResourceReader(assembly.GetManifestResourceStream(embeddedResourceFullName)))
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

					this.Entries.Add(lastGem);
				}
			}
		}
		#endregion

		#region Public Properties
		public SortedSet<Gem> Entries { get; } = new SortedSet<Gem>();

		public string Name { get; set; }
		#endregion

		#region Public Override Properties
		public override string ToString() => this.Name;
		#endregion
	}
}