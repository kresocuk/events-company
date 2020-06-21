using System;
using System.Collections.Generic;

namespace TestWebApp.Models
{
    public partial class IznajmljivanjeOprema
    {
        public int Id { get; set; }
        public int OpremaId { get; set; }
        public string Detalji { get; set; }
        public double Cijena { get; set; }
        public DateTime Vrijeme { get; set; }
        public int? Trajanje { get; set; }
        public string Kontakt { get; set; }

        public virtual Oprema Oprema { get; set; }
    }
}
