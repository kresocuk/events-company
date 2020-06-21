using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestWebApp.Models;

namespace TestWebApp.ViewModels
{
    public class ComplexViewModel
    {
        public IEnumerable<SklopljeniPosaoOsoba> sposobe { get; set; }
        public IEnumerable<SklopljeniPosaoOprema> spopreme { get; set; }
        public SklopljeniPosao sklopljeniposao { get; set; }
    }
}
