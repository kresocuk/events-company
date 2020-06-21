using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestWebApp.Models;

namespace TestWebApp.ViewModels
{
	public class OpremaViewModel
	{
        public int Id { get; set; }
        public string InventarniBroj { get; set; }
        public string Naziv { get; set; }
        public int TipId { get; set; }
        public int SkladisteId { get; set; }
        public double KupovnaVrijednost { get; set; }
        public double KnjigovodstvenaVrijednost { get; set; }
        public int VrijemeAmortizacije { get; set; }
        public DateTime VrijemeKupnje { get; set; }
        public int ReferentniTipId { get; set; }
        public bool ImaSliku { get; set; }
        public string Opis { get; set; }
    }
}