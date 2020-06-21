
using System.Collections;
using System.Collections.Generic;
using TestWebApp.Models;

namespace TestWebApp.ViewModels
{
	public class UnajmljivanjeOpremeViewModel
	{
		public IEnumerable<UnajmljivanjeOprema> UnajmljivanjeOpreme { get; set; } 
		public PagingInfo PagingInfo { get; set; }
	}
}