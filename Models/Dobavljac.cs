using System;
using System.Collections.Generic;

namespace TestWebApp.Models
{
    public partial class Dobavljac
    {
        public Dobavljac()
        {
            OpremaDobavljac = new HashSet<OpremaDobavljac>();
        }

        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Lokacija { get; set; }
        public string Detalji { get; set; }

        public virtual ICollection<OpremaDobavljac> OpremaDobavljac { get; set; }
    }
}
