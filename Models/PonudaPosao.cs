using System;
using System.Collections.Generic;

namespace TestWebApp.Models
{
    public partial class PonudaPosao
    {
        public int Id { get; set; }
        public int PosaoId { get; set; }
        public int PonudaId { get; set; }

        public virtual Ponuda Ponuda { get; set; }
        public virtual Posao Posao { get; set; }
    }
}
