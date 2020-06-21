using System;
using System.Collections.Generic;

namespace TestWebApp.Models
{
    public partial class OsobaPosao
    {
        public int Id { get; set; }
        public int PosaoId { get; set; }
        public int OsobaId { get; set; }

        public virtual Osoba Osoba { get; set; }
        public virtual Posao Posao { get; set; }
    }
}
