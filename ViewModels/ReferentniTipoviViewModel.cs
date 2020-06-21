using System.Collections;
using System.Collections.Generic;
using TestWebApp.Models;

namespace TestWebApp.ViewModels
{
	public class ReferentniTipoviViewModel
	{
		public IEnumerable<ReferentniTip> ReferentniTipovi { get; set; } 
		public PagingInfo PagingInfo { get; set; }
	}
}