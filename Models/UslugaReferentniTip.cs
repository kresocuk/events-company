using System;
using System.Collections.Generic;

namespace TestWebApp.Models
{
    public partial class UslugaReferentniTip
    {
        public int Id { get; set; }
        public int UslugaId { get; set; }
        public int ReferentniTipId { get; set; }

        public virtual ReferentniTip ReferentniTip { get; set; }
        public virtual Usluga Usluga { get; set; }
    }
}
