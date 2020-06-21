
using System.Collections;
using System.Collections.Generic;
using TestWebApp.Models;

namespace TestWebApp.ViewModels
{
	public class CertifikatiViewModel
	{
		public IEnumerable<Certifikat> Certifikati { get; set; } 
		public PagingInfo PagingInfo { get; set; }
	}
}