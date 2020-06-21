using System.Collections;
using System.Collections.Generic;
using TestWebApp.Models;

namespace TestWebApp.ViewModels
{
	public class OpremeViewModel
	{
		public IEnumerable<OpremaViewModel> Oprema { get; set; } 
		public PagingInfo PagingInfo { get; set; }
	}
}