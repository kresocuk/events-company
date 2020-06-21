using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestWebApp.Models
{
    public partial class Certifikat
    {
        public Certifikat()
        {
            OsobaCertifikat = new HashSet<OsobaCertifikat>();
        }

        public int Id { get; set; }
        [Display(Name = "Naziv certifikata", Prompt = "Unesite naziv")]
        [Required(ErrorMessage = "Naziv je obavezno polje.")]
        [MaxLength(50, ErrorMessage = "Naziv ne moze biti duzi od 50 znakova.")]
        public string Naziv { get; set; }
        [Display(Name = "Opis certifikata")]
        [Required(ErrorMessage = "Opis je obavezno polje.")]
        public string Opis { get; set; }

        public virtual ICollection<OsobaCertifikat> OsobaCertifikat { get; set; }
    }
}
