
using System.Collections;
using System.Collections.Generic;
using TestWebApp.Models;

namespace TestWebApp.ViewModels
{
	public class SkladistaViewModel
	{
		public IEnumerable<Skladiste> Skladista { get; set; } 
		public PagingInfo PagingInfo { get; set; }
	}
}