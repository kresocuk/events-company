using System.Collections;
using System.Collections.Generic;
using TestWebApp.Models;

namespace TestWebApp.ViewModels
{
	public class JavniNatjecajiViewModel
	{
		public IEnumerable<JavniNatjecaji> JavniNatjecaji { get; set; } 
		public PagingInfo PagingInfo { get; set; }
	}
}