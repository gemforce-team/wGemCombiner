namespace WGemCombiner
{
	using System;
	using System.Globalization;

	public static class Localization
	{
		#region Public Methods
		public static string CurrentCulture(FormattableString formattable) => formattable?.ToString(CultureInfo.CurrentCulture);
		#endregion
	}
}
