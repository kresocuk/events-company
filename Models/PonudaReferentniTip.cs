using System;
using System.Collections.Generic;

namespace TestWebApp.Models
{
    public partial class PonudaReferentniTip
    {
        public int Id { get; set; }
        public int PonudaId { get; set; }
        public int ReferentniTipId { get; set; }

        public virtual Ponuda Ponuda { get; set; }
        public virtual ReferentniTip ReferentniTip { get; set; }
    }
}
