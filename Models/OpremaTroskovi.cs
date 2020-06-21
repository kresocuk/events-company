using System;
using System.Collections.Generic;

namespace TestWebApp.Models
{
    public partial class OpremaTroskovi
    {
        public int Id { get; set; }
        public int OpremaId { get; set; }
        public double Cijena { get; set; }
        public string Opis { get; set; }

        public virtual Oprema Oprema { get; set; }
    }
}
