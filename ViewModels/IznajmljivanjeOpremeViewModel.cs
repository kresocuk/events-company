using System.Collections;
using System.Collections.Generic;
using TestWebApp.Models;

namespace TestWebApp.ViewModels
{
	public class IznajmljivanjeOpremeViewModel
	{

		public IEnumerable<IznajmljivanjeOpremaViewModel> IznajmljivanjeOprema { get; set; } 
		public PagingInfo PagingInfo { get; set; }

		public IznajmljivanjeOpremaFilter Filter { get; set; }
	}
}