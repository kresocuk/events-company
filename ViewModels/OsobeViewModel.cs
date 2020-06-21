using System.Collections;
using System.Collections.Generic;
using TestWebApp.Models;

namespace TestWebApp.ViewModels
{
	public class OsobeViewModel
	{
		public IEnumerable<OsobaViewModel> Osobe { get; set; } 
		public PagingInfo PagingInfo { get; set; }
	}
}