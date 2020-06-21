using System;
using System.Collections.Generic;

namespace TestWebApp.Models
{
    public partial class Tip
    {
        public Tip()
        {
            Oprema = new HashSet<Oprema>();
        }

        public int Id { get; set; }
        public string Naziv { get; set; }

        public virtual ICollection<Oprema> Oprema { get; set; }
    }
}
