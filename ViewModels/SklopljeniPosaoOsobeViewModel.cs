using System.Collections.Generic;
using TestWebApp.Models;
using TestWebApp.ViewModels;

namespace TestWebApp.ViewModels
{
    public class SklopljeniPosaoOsobeViewModel
    {
        public List<SklopljeniPosaoOsoba> SklopljeniPosaoOsobe { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}