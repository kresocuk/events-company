using System.Collections;
using System.Collections.Generic;
using TestWebApp.Models;

namespace TestWebApp.ViewModels
{
	public class NatjecajiViewModel
	{
		public IEnumerable<Natjecaj> Natjecaji { get; set; } 
		public PagingInfo PagingInfo { get; set; }
	}
}