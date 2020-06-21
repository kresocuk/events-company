using System;
using System.Collections.Generic;

namespace TestWebApp.Models
{
    public partial class UnajmljivanjeOprema
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public int ReferentniTipId { get; set; }
        public double Cijena { get; set; }
        public string Detalji { get; set; }
        public DateTime Vrijeme { get; set; }
        public int? Trajanje { get; set; }

        public virtual ReferentniTip ReferentniTip { get; set; }
    }
}
