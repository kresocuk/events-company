using System;
using System.Collections.Generic;

namespace TestWebApp.Models
{
    public partial class Usluga
    {
        public Usluga()
        {
            UslugaReferentniTip = new HashSet<UslugaReferentniTip>();
        }

        public int Id { get; set; }
        public string Detalji { get; set; }
        public double Cijena { get; set; }
        public DateTime Vrijeme { get; set; }
        public string Kontakt { get; set; }
        public string Lokacija { get; set; }

        public virtual ICollection<UslugaReferentniTip> UslugaReferentniTip { get; set; }
    }
}
