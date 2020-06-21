using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TestWebApp.Controllers;

namespace TestWebApp.Models
{
    public partial class Oprema
    {
        public Oprema()
        {
            IznajmljivanjeOprema = new HashSet<IznajmljivanjeOprema>();
            OpremaDobavljac = new HashSet<OpremaDobavljac>();
            OpremaTroskovi = new HashSet<OpremaTroskovi>();
            SklopljeniPosaoOprema = new HashSet<SklopljeniPosaoOprema>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Potrebno je unijeti inventarni broj")]
        [MaxLength(20, ErrorMessage = "Inventarni broj ne moze biti duzi od 20 znakova.")]
        [Remote(action: nameof(OpremaController.ProvjeriInventarniBroj), controller: "Oprema", ErrorMessage = "Oprema s navedenim inventarnim brojem već postoji")]


        public string InventarniBroj { get; set; }
        public string Naziv { get; set; }
        public int TipId { get; set; }
        public int SkladisteId { get; set; }
        public double KupovnaVrijednost { get; set; }
        public double KnjigovodstvenaVrijednost { get; set; }
        public int VrijemeAmortizacije { get; set; }
        public DateTime VrijemeKupnje { get; set; }
        public int ReferentniTipId { get; set; }
        public byte[] Slika { get; set; }
        public string Opis { get; set; }

        public virtual ReferentniTip ReferentniTip { get; set; }
        public virtual Skladiste Skladiste { get; set; }
        public virtual Tip Tip { get; set; }
        public virtual ICollection<IznajmljivanjeOprema> IznajmljivanjeOprema { get; set; }
        public virtual ICollection<OpremaDobavljac> OpremaDobavljac { get; set; }
        public virtual ICollection<OpremaTroskovi> OpremaTroskovi { get; set; }
        public virtual ICollection<SklopljeniPosaoOprema> SklopljeniPosaoOprema { get; set; }
    }
}
