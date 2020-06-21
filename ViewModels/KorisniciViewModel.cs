using System.Collections;
using System.Collections.Generic;
using TestWebApp.Models;

namespace TestWebApp.ViewModels
{
	public class KorisniciViewModel
	{
		public IEnumerable<Korisnik> Korisnici { get; set; } 
		public PagingInfo PagingInfo { get; set; }
	}
}