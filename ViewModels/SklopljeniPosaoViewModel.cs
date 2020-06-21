using System;

namespace TestWebApp.ViewModels
{
    internal class SklopljeniPosaoViewModel
    {
        public int Broj { get; set; }
        public string Kontakt { get; set; }
        public DateTime Vrijeme { get; set; }
        public double Cijena { get; set; }
        public string Lokacija { get; set; }
        public string Detalji { get; set; }
    }
}