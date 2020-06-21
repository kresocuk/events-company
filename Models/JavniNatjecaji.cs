using System;
using System.Collections.Generic;

namespace TestWebApp.Models
{
    public partial class JavniNatjecaji
    {
        public int Id { get; set; }
        public double NasaPonuda { get; set; }
        public double DobitnaPonuda { get; set; }
        public string Detalji { get; set; }
        public DateTime Vrijeme { get; set; }
    }
}
