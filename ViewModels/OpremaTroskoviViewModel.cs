using System.Collections;
using System.Collections.Generic;
using TestWebApp.Models;

namespace TestWebApp.ViewModels
{
	public class OpremaTroskoviViewModel
	{
		public IEnumerable<OpremaTroskovi> OpremaTroskovi { get; set; } 
		public PagingInfo PagingInfo { get; set; }
	}
}