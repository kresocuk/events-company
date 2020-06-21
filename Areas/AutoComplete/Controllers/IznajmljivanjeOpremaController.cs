using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TestWebApp.Areas.AutoComplete.Models;
using TestWebApp.Models;

namespace TestWebApp.Areas.AutoComplete.Controllers
{
	/// <summary>
	/// Controller za autocomplete
	/// </summary>
	[Area("AutoComplete")]
	public class IznajmljivanjeOpremaController : Controller
	{
		private readonly PI03Context context;
		private readonly AppSettings appData;

		public IznajmljivanjeOpremaController(PI03Context context, IOptionsSnapshot<AppSettings> options) {
			this.context = context;
			appData = options.Value;
		}

		public IEnumerable<IdLabel> Get(string term) {
			var query = context.Oprema
							   .Select(o => new IdLabel
							   {
								   Id = o.Id,
								   Label = o.Naziv
							   })
							   .Where(l => l.Label.Contains(term));
			var list = query.OrderBy(l => l.Label)
							.ThenBy(l => l.Id)
							.Take(appData.AutoCompleteCount)
							.ToList();

			return list;
		}

	}
}
