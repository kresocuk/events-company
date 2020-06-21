using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestWebApp.Models
{
    /// <summary>
    /// model za osobu
    /// </summary>
    public partial class Osoba
    {
        public Osoba()
        {
            OsobaCertifikat = new HashSet<OsobaCertifikat>();
            OsobaPosao = new HashSet<OsobaPosao>();
            SklopljeniPosaoOsoba = new HashSet<SklopljeniPosaoOsoba>();
        }

        public int Id { get; set; }
        [Display(Name = "Ime", Prompt = "Unesite ime")]
        [Required(ErrorMessage = "Ime je obavezno polje.")]
        [MaxLength(20, ErrorMessage = "Ime ne moze biti duzi od 20 znakova.")]
        public string Ime { get; set; }
        [Display(Name = "Prezime", Prompt = "Unesite prezime")]
        [Required(ErrorMessage = "Prezime je obavezno polje.")]
        [MaxLength(50, ErrorMessage = "Prezime ne moze biti duzi od 50 znakova.")]
        public string Prezime { get; set; }
        [Display(Name = "Datum rodjenja", Prompt = "Unesite datum rodjenja")]
        [Required(ErrorMessage = "Datum rodjenja je obavezno polje.")]
        public DateTime DatumRodjenja { get; set; }
        public string Email { get; set; }

        public virtual ICollection<OsobaCertifikat> OsobaCertifikat { get; set; }
        public virtual ICollection<OsobaPosao> OsobaPosao { get; set; }
        public virtual ICollection<SklopljeniPosaoOsoba> SklopljeniPosaoOsoba { get; set; }
    }
}
