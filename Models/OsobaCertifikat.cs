using System;
using System.Collections.Generic;

namespace TestWebApp.Models
{
    public partial class OsobaCertifikat
    {
        public int Id { get; set; }
        public int CertifikatId { get; set; }
        public int OsobaId { get; set; }

        public virtual Certifikat Certifikat { get; set; }
        public virtual Osoba Osoba { get; set; }
    }
}
