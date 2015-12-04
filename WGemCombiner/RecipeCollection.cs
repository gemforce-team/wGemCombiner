namespace WGemCombiner
{
	using System.Collections.ObjectModel;
	using static Globals;

	public class RecipeCollection : KeyedCollection<string, Combiner>
	{
		#region Protected Override Methods
		protected override string GetKeyForItem(Combiner item)
		{
			ThrowNull(item, nameof(item));
			return item.Gem.Title;
		}
		#endregion
	}
}
