
using System.Collections;
using System.Collections.Generic;
using TestWebApp.Models;

namespace TestWebApp.ViewModels
{
	public class UslugeViewModel
	{
		public IEnumerable<Usluga> Usluge { get; set; } 
		public PagingInfo PagingInfo { get; set; }
	}
}