using System.Collections;
using System.Collections.Generic;
using TestWebApp.Models;

namespace TestWebApp.ViewModels
{
	/// <summary>
	/// viewmodel za ponudu
	/// </summary>
	public class PonudeViewModel
	{
		public IEnumerable<Ponuda> Ponude { get; set; } 
		public PagingInfo PagingInfo { get; set; }
	}
}
