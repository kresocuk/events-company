using System;
using System.Collections.Generic;

namespace TestWebApp.Models
{
    public partial class OpremaDobavljac
    {
        public int Id { get; set; }
        public int OpremaId { get; set; }
        public int DobavljacId { get; set; }

        public virtual Dobavljac Dobavljac { get; set; }
        public virtual Oprema Oprema { get; set; }
    }
}
