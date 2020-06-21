using System;
using System.Collections.Generic;

namespace TestWebApp.Models
{
    public partial class Korisnik
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Lozinka { get; set; }
        public string Kontakt { get; set; }
    }
}
