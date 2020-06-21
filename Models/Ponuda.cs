using System;
using System.Collections.Generic;

namespace TestWebApp.Models
{
    /// <summary>
    /// Model za ponudu
    /// </summary>
    public partial class Ponuda
    {
        public Ponuda()
        {
            Natjecaj = new HashSet<Natjecaj>();
            PonudaPosao = new HashSet<PonudaPosao>();
            PonudaReferentniTip = new HashSet<PonudaReferentniTip>();
        }

        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public double Cijena { get; set; }
        public DateTime? Vrijeme { get; set; }
        public int? Trajanje { get; set; }

        public virtual ICollection<Natjecaj> Natjecaj { get; set; }
        public virtual ICollection<PonudaPosao> PonudaPosao { get; set; }
        public virtual ICollection<PonudaReferentniTip> PonudaReferentniTip { get; set; }
    }
}
