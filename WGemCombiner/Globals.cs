namespace WGemCombiner
{
	using System;
	using System.Reflection;
	public static class Globals
	{
		#region Public Properties
		public static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
		#endregion

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
