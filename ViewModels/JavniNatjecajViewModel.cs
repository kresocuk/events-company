using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestWebApp.Models;

namespace TestWebApp.ViewModels
{
    /// <summary>
    /// viewmodel za javni natjecaj
    /// </summary>
    public class JavniNatjecajViewModel
    {
        public int Broj { get;  set; }
        public DateTime Vrijeme { get; set; }
        public double NasaPonuda { get; set; }
        public double DobitnaPonuda { get; set; }
        public string Detalji { get; set; }
    }
}