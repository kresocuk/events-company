using System;
using System.Collections.Generic;

namespace TestWebApp.Models
{
    /// <summary>
    /// model za skladiste
    /// </summary>
    public partial class Skladiste
    {
        public Skladiste()
        {
            Oprema = new HashSet<Oprema>();
        }

        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Lokacija { get; set; }

        public virtual ICollection<Oprema> Oprema { get; set; }
    }
}
