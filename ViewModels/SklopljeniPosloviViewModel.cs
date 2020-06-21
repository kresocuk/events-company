
using System.Collections;
using System.Collections.Generic;
using TestWebApp.Models;

namespace TestWebApp.ViewModels
{
	public class SklopljeniPosloviViewModel
	{
		public IEnumerable<SklopljeniPosao> SklopljeniPoslovi { get; set; } 
		public PagingInfo PagingInfo { get; set; }

		public SklopljeniPosaoFilter Filter { get; set; }
	}
}