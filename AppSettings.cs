namespace TestWebApp
{
	public class AppSettings{
		public static object ImageSettings { get; internal set; }
		public int PageSize{get; set;} = 10;

		public int PageOffset{get; set;} = 5;
		public int AutoCompleteCount { get; internal set; } = 50;
	}
}