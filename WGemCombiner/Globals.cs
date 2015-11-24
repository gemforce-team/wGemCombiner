namespace WGemCombiner
{
	using System;

	public static class Globals
	{
		#region Public Methods
		public static void ThrowNull(object value, string name)
		{
			if (ReferenceEquals(value, null))
			{
				throw new ArgumentNullException(name);
			}
		}
		#endregion
	}
}
