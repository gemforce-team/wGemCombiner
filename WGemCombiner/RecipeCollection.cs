namespace WGemCombiner
{
	using System.Collections.ObjectModel;
	using static Globals;

	public class RecipeCollection : KeyedCollection<string, Combiner>
	{
		protected override string GetKeyForItem(Combiner item)
		{
			ThrowNull(item, nameof(item));
			return item.Gem.CombineTitle;
		}
	}
}
