using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestWebApp.Models;

namespace TestWebApp.ViewModels
{
	public class IznajmljivanjeOpremaViewModel
	{
        public int Id { get; set; }
        [Display(Name = "Oprema")]
        public int OpremaId { get; set; }
        public string OpremaNaziv { get; set; }
        public string Detalji { get; set; }
        public double Cijena { get; set; }
        public DateTime Vrijeme { get; set; }
        public int? Trajanje { get; set; }
        public string Kontakt { get; set; }

    }
}