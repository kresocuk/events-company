using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestWebApp.Models;

namespace TestWebApp.ViewModels
{
    /// <summary>
    ///  ViewModel za ponudu
    /// </summary>
	public class PonudaViewModel
	{
        public int Id { get; set; }
        
        public string Ime { get; set; }
        
        public string Prezime { get; set; }
      
        public DateTime DatumRodjenja { get; set; }
        public string Email { get; set; }

        public virtual ICollection<OsobaCertifikat> OsobaCertifikat { get; set; }
    }
}