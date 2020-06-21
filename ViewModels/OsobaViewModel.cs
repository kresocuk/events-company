using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestWebApp.Models;

namespace TestWebApp.ViewModels
{
	public class OsobaViewModel
	{
        public int Id { get; set; }
        
        public string Ime { get; set; }
        
        public string Prezime { get; set; }
      
        public DateTime DatumRodjenja { get; set; }
        public string Email { get; set; }

        public virtual ICollection<OsobaCertifikat> OsobaCertifikat { get; set; }
    }
}