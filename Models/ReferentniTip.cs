using System;
using System.Collections.Generic;

namespace TestWebApp.Models
{
    public partial class ReferentniTip
    {
        public ReferentniTip()
        {
            Oprema = new HashSet<Oprema>();
            PonudaReferentniTip = new HashSet<PonudaReferentniTip>();
            UnajmljivanjeOprema = new HashSet<UnajmljivanjeOprema>();
            UslugaReferentniTip = new HashSet<UslugaReferentniTip>();
        }

        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }

        public virtual ICollection<Oprema> Oprema { get; set; }
        public virtual ICollection<PonudaReferentniTip> PonudaReferentniTip { get; set; }
        public virtual ICollection<UnajmljivanjeOprema> UnajmljivanjeOprema { get; set; }
        public virtual ICollection<UslugaReferentniTip> UslugaReferentniTip { get; set; }
    }
}
