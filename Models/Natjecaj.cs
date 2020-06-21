using System;
using System.Collections.Generic;

namespace TestWebApp.Models
{
    public partial class Natjecaj
    {
        public int Id { get; set; }
        public string Detalji { get; set; }
        public int PonudaId { get; set; }

        public virtual Ponuda Ponuda { get; set; }
    }
}
