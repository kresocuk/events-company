using System;
using System.Text;

namespace TestWebApp.Extensions
{
	/// <summary>
	/// Ekstenzija za iznimku
	/// </summary>
	public static class ExceptionExtensions
	{
		/// <summary>
		/// Metoda za ispis pogreske
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		public static String CompleteExceptionMessage(this Exception e)
		{
			StringBuilder sb = new StringBuilder();
			while (e != null)
			{
				sb.AppendLine(e.Message);
				e = e.InnerException;
			}
			return sb.ToString();
		}
	}
}
