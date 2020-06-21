using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TestWebApp.Extensions;
using TestWebApp.Models;
using TestWebApp.ViewModels;

namespace TestWebApp.Controllers {

    public class OsobaController : Controller{

        private readonly PI03Context ctx;
        private readonly AppSettings appData;

        public OsobaController(PI03Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot )
        {
            this.ctx = ctx;
            appData = optionsSnapshot.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
			await PrepareDropDownLists();
            return View();
        }

		private async Task PrepareDropDownLists()
		{
            var certifikati = await ctx.Certifikat
                                 .Where(d => d.Id != 0)
                                 .OrderBy(d => d.Naziv)
                                 .Select(d => new { d.Id, d.Naziv})
                                 .ToListAsync();


            ViewBag.Certifikati = new SelectList(certifikati, nameof(Certifikat.Id), nameof(Certifikat.Naziv));
		}

		[HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateAsync(Osoba osoba)
		{
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(osoba);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"{osoba.Ime} {osoba.Prezime} uspješno dodan. Id osobe = {osoba.Id}";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));

                    //samo return View(); ostavi nas na stranici, pa mozemo ponovo dodavati podatke
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.CompleteExceptionMessage());
					await PrepareDropDownLists();
                    return View(osoba);
                }
            }
            else
            {
				await PrepareDropDownLists();
                return View(osoba);
            }
        }

        [HttpGet]
        public IActionResult Edit(int Id, int page = 1, int sort = 1, bool ascending = true)
        {
            var osoba = ctx.Osoba
                                    .AsNoTracking()
                                    .Where(d => d.Id == Id)
                                    //.SingleOrDefault();
                                    .FirstOrDefault();
            if (osoba == null)
            {
                return NotFound($"ne postoji osoba s oznakom {Id}");
            }
            else {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascendig = ascending;
                return View(osoba);
            }
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int Id, int page = 1, int sort = 1, bool ascending = true) {
            try {
                Osoba osoba = await ctx.Osoba.FindAsync(Id);
                if (osoba == null) {
                    return NotFound($"Ne postoji osoba s tom oznakom {Id}");
                        }
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                bool ok = await TryUpdateModelAsync<Osoba>(osoba, "", d => d.Id, d => d.Ime, d => d.Prezime, d => d.DatumRodjenja, d => d.Email);
                if (ok)
                {
                    try
                    {
                        TempData[Constants.Message] = $"Osoba {osoba.Ime} uspješno azuriran.";
                        TempData[Constants.ErrorOccurred] = false;
                        await ctx.SaveChangesAsync();
                        return RedirectToAction(nameof(Index), new { page, sort, ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(osoba);
                    }
                }
                else {
                    ModelState.AddModelError(string.Empty,"Podatke o osobi nije moguce povezati s forme");
                    return View(osoba);
                }
            }
            catch (Exception exc) {
                TempData[Constants.Message] = exc.CompleteExceptionMessage();
                TempData[Constants.ErrorOccurred] = true;
                return RedirectToAction(nameof(Edit), new { Id, page, sort, ascending });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int Id)
        {
            var osoba = await ctx.Osoba.FindAsync(Id);
            if (osoba != null)
            {
                try
                {
                    string naziv = osoba.Ime + " " + osoba.Prezime;
                    ctx.Remove(osoba);
                    await ctx.SaveChangesAsync();
                    var result = new
                    {
                        message = $"Osoba {naziv} sa sifrom {Id} obrisano.",
                        successful = true
                    };
                    return Json(result);
                }
                catch (Exception exc)
                {
                    var result = new
                    {
                        message = $"Greska prilikom brisanja osobe {exc.CompleteExceptionMessage()}",
                        successful = false
                    };
                    return Json(result);
                }
            }
            else {
                return NotFound($"Osoba sa sifrom {Id} ne postoji");
            }
        }

        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true) {

            int pagesize = appData.PageSize;
            var query = ctx.Osoba.AsNoTracking();

            int count = query.Count();

            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                Sort = sort,
                Ascending = ascending,
                ItemsPerPage = pagesize,
                TotalItems = count
            };

            if (page < 1)
            {
                page = 1;
            } else if (page > pagingInfo.TotalPages) {
                return RedirectToAction(nameof(Index), new { page = pagingInfo.TotalPages, sort, ascending });
            }

            System.Linq.Expressions.Expression<Func<Osoba, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.Id;
                    break;
                case 2:
                    orderSelector = d => d.Ime;
                    break;
                case 3:
                    orderSelector = d => d.Prezime;
                    break;
                case 4:
                    orderSelector = d => d.Email;
                    break;

            }

            if (orderSelector != null)
            {
                query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
            }

            var osobe = query
                              .Select(o => new OsobaViewModel {
                                  Id = o.Id,
                                  Ime = o.Ime,
                                  Prezime = o.Prezime,
                                  Email = o.Email,
                                  

                              })
                              .Skip((page - 1) * pagesize)
                              .Take(pagesize)
                              .ToList();

            var model = new OsobeViewModel
            {
                Osobe = osobe,
                PagingInfo = pagingInfo
            };
            return View(model);
        }
    }
}