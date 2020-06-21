using System;
using System.Collections.Generic;

namespace TestWebApp.Models
{
    public partial class Posao
    {
        /// <summary>
        /// model za posao
        /// </summary>
        public Posao()
        {
            OsobaPosao = new HashSet<OsobaPosao>();
            PonudaPosao = new HashSet<PonudaPosao>();
            SklopljeniPosaoOsoba = new HashSet<SklopljeniPosaoOsoba>();
        }

        public int Id { get; set; }
        public string Naziv { get; set; }
        public double Satnica { get; set; }
        public string Opis { get; set; }

        public virtual ICollection<OsobaPosao> OsobaPosao { get; set; }
        public virtual ICollection<PonudaPosao> PonudaPosao { get; set; }
        public virtual ICollection<SklopljeniPosaoOsoba> SklopljeniPosaoOsoba { get; set; }
    }
}
