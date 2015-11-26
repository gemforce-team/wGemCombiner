namespace WGemCombiner
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using static Globals;

	public class RecipeCollection : KeyedCollection<string, Gem>
	{
		public RecipeCollection(IEnumerable<Gem> gems)
		{
			foreach (var gem in gems)
			{
				this.Add(gem);
			}
		}

		protected override string GetKeyForItem(Gem item)
		{
			ThrowNull(item, nameof(item));
			return item.CombineTitle;
		}
	}
}
