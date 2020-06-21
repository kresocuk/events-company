using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TestWebApp.Models;

namespace TestWebApp.ViewModels
{
    public class IznajmljivanjeOpremaFilter : IPageFilter
    {
        public int? OpremaId { get; set; }

        public string OpremaNaziv { get; set; }

        public double? IznosOd { get; set; }
        public double? IznosDo { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: dd.MM.yyyy.}", ApplyFormatInEditMode = false)]
        public DateTime? DatumOd { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: dd.MM.yyyy.}", ApplyFormatInEditMode = false)]
        public DateTime? DatumDo { get; set; }

        public bool IsEmpty()
        {
            bool active = OpremaId.HasValue
                        || DatumOd.HasValue
                        || DatumDo.HasValue
                        || IznosOd.HasValue
                        || IznosDo.HasValue;
            return !active;
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}-{2}-{3}-{4}",
                  OpremaId,
                  DatumOd?.ToString("dd.MM.yyyy"),
                  DatumDo?.ToString("dd.MM.yyyy"),
                  IznosOd,
                  IznosDo
        );
        }

        public static IznajmljivanjeOpremaFilter FromString(string s, ILogger logger = null )
        {
            var filter = new IznajmljivanjeOpremaFilter();
            try
            {
                var arr = s.Split('-', StringSplitOptions.None);
                filter.OpremaId = string.IsNullOrWhiteSpace(arr[0]) ? new int?() : int.Parse(arr[0]);
                
                filter.DatumOd= string.IsNullOrWhiteSpace(arr[1]) ? new DateTime?() : DateTime.ParseExact(arr[1], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                
                filter.DatumDo= string.IsNullOrWhiteSpace(arr[2]) ? new DateTime?() : DateTime.ParseExact(arr[2], "dd.MM.yyyy", CultureInfo.InvariantCulture);

                filter.IznosOd= string.IsNullOrWhiteSpace(arr[3]) ? new double?() : double.Parse(arr[3]);
                
                filter.IznosDo= string.IsNullOrWhiteSpace(arr[4]) ? new double?() : double.Parse(arr[4]);
            }
            catch
            {

            }
            return filter;
        }

        public IQueryable<IznajmljivanjeOprema> Apply(IQueryable<IznajmljivanjeOprema> query)
        {
            if (OpremaId.HasValue)
            {
                query = query.Where(d => d.OpremaId == OpremaId.Value);
            }
            if (DatumOd.HasValue)
            {
                query = query.Where(d => d.Vrijeme >= DatumOd.Value);
            }
            if (DatumDo.HasValue)
            {
                query = query.Where(d => d.Vrijeme <= DatumDo.Value);
            }
            if (IznosOd.HasValue)
            {
                query = query.Where(d => d.Cijena >= IznosOd.Value);
            }
            if (IznosDo.HasValue)
            {
                query = query.Where(d => d.Cijena <= IznosDo.Value);
            }
            return query;
        }
    }

}
