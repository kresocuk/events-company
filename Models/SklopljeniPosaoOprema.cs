using System;
using System.Collections.Generic;

namespace TestWebApp.Models
{
    public partial class SklopljeniPosaoOprema
    {
        public int Id { get; set; }
        public int OpremaId { get; set; }
        public int Trajanje { get; set; }
        public int SklopljeniPosaoId { get; set; }

        public virtual Oprema Oprema { get; set; }
        public virtual SklopljeniPosao SklopljeniPosao { get; set; }
    }
}
