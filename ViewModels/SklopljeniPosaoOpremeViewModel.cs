using System.Collections.Generic;
using TestWebApp.Models;
using TestWebApp.ViewModels;

namespace TestWebApp.ViewModels
{
    public class SklopljeniPosaoOpremeViewModel
    {
        public List<SklopljeniPosaoOprema> SklopljeniPosaoOpreme { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}