
using System.Collections;
using System.Collections.Generic;
using TestWebApp.Models;

namespace TestWebApp.ViewModels
{
	public class DobavljaciViewModel
	{
		public IEnumerable<Dobavljac> Dobavljaci { get; set; } 
		public PagingInfo PagingInfo { get; set; }
	}
}