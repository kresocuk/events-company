using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestWebApp.Models
{
    public partial class SklopljeniPosaoOsoba
    {
        public int Id { get; set; }
        
        public int SklopljeniPosaoId { get; set; }
        
        public int PosaoId { get; set; }

        public int OsobaId { get; set; }
        public int Trajanje { get; set; }

        public virtual Osoba Osoba { get; set; }
        public virtual Posao Posao { get; set; }
        public virtual SklopljeniPosao SklopljeniPosao { get; set; }
    }
}
