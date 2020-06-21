
using System.Collections;
using System.Collections.Generic;
using TestWebApp.Models;

namespace TestWebApp.ViewModels
{
	public class PosloviViewModel
	{
		public IEnumerable<Posao> Poslovi { get; set; } 
		public PagingInfo PagingInfo { get; set; }
	}
}