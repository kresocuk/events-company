using System.Collections;
using System.Collections.Generic;
using TestWebApp.Models;

namespace TestWebApp.ViewModels
{
	public class TipoviViewModel
	{
		public IEnumerable<Tip> Tipovi { get; set; } 
		public PagingInfo PagingInfo { get; set; }
	}
}