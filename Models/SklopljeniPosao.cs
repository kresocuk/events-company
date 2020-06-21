using System;
using System.Collections.Generic;

namespace TestWebApp.Models
{
    public partial class SklopljeniPosao
    {
        public SklopljeniPosao()
        {
            SklopljeniPosaoOprema = new HashSet<SklopljeniPosaoOprema>();
            SklopljeniPosaoOsoba = new HashSet<SklopljeniPosaoOsoba>();
        }

        public int Id { get; set; }
        public double Cijena { get; set; }
        public string Detalji { get; set; }
        public DateTime Vrijeme { get; set; }
        public string Lokacija { get; set; }
        public string Kontakt { get; set; }

        public virtual ICollection<SklopljeniPosaoOprema> SklopljeniPosaoOprema { get; set; }
        public virtual ICollection<SklopljeniPosaoOsoba> SklopljeniPosaoOsoba { get; set; }
    }
}
