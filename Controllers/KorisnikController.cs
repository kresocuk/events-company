using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Targets;
using System;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using TestWebApp.Extensions;
using TestWebApp.Models;
using System.Text.Json;

using TestWebApp.ViewModels;

namespace TestWebApp.Controllers {

    public class KorisnikController : Controller{

        private readonly PI03Context ctx;
        private readonly AppSettings appData;
        private readonly ILogger <KorisnikController> logger;

        public KorisnikController(PI03Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot , ILogger <KorisnikController> logger)

        {
            this.ctx = ctx;
            this.logger = logger;
            appData = optionsSnapshot.Value;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Korisnik korisnik) {
            logger.LogTrace(JsonSerializer.Serialize(korisnik), new JsonSerializerOptions {IgnoreNullValues =true });
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(korisnik);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Korisnik {korisnik.Username} uspješno dodan.";
                    TempData[Constants.ErrorOccurred] = false;
                    logger.LogInformation($"Korisnik {korisnik.Username } dodan ");
                    return RedirectToAction(nameof(Index));

                    //samo return View(); ostavi nas na stranici, pa mozemo ponovo dodavati podatke
                }
                catch (Exception e)
                {
                    logger.LogError($"Pogreška prilikom dodavanja novog korisnika {e.CompleteExceptionMessage()}");
                    ModelState.AddModelError(string.Empty, e.CompleteExceptionMessage());
                    return View(korisnik);
                }
            }
            else
            {
                return View(korisnik);
            }
        }

        [HttpGet]
        public IActionResult Edit(int Id, int page = 1, int sort = 1, bool ascending = true)
        {
            var korisnik = ctx.Korisnik
                                    .AsNoTracking()
                                    .Where(d => d.Id == Id)
                                    //.SingleOrDefault();
                                    .FirstOrDefault();
            if (korisnik == null)
            {
                return NotFound($"ne postoji korisnik s oznakom {Id}");
            }
            else {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascendig = ascending;
                return View(korisnik);
            }
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int Id, int page = 1, int sort = 1, bool ascending = true) {
            try {
                Korisnik korisnik = await ctx.Korisnik.FindAsync(Id);
                if (korisnik == null) {
                    return NotFound($"Ne postoji korisnik s tom oznakom {Id}");
                        }
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                bool ok = await TryUpdateModelAsync<Korisnik>(korisnik, "", d => d.Id, d => d.Username,  d => d.Email, d => d.Lozinka, d => d.Kontakt);
                if (ok)
                {
                    try
                    {
                        TempData[Constants.Message] = $"Korisnik {korisnik.Username} uspješno azuriran.";
                        TempData[Constants.ErrorOccurred] = false;
                        await ctx.SaveChangesAsync();
                        return RedirectToAction(nameof(Index), new { page, sort, ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(korisnik);
                    }
                }
                else {
                    ModelState.AddModelError(string.Empty,"Podatke o korisniku nije moguce povezati s forme");
                    return View(korisnik);
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
        public IActionResult Delete(int Id, int page=1, int sort=1, bool ascending=true)
        {
            var korisnik = ctx.Korisnik.Find(Id);
            if (korisnik == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    string naziv = korisnik.Username;
                    ctx.Remove(korisnik);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Korisnik {korisnik.Username} uspješno obrisan.";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception e)
                {
                    TempData[Constants.Message] = $"Pogreška prilikom brisanja korisnika." + e.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
                return RedirectToAction(nameof(Index), new { page, sort, ascending });
            }

        }

        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true) {

            int pagesize = appData.PageSize;
            var query = ctx.Korisnik.AsNoTracking();

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

            System.Linq.Expressions.Expression<Func<Korisnik, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.Id;
                    break;
                case 2:
                    orderSelector = d => d.Username;
                    break;
                case 3:
                    orderSelector = d => d.Email;
                    break;
                case 4:
                    orderSelector = d => d.Kontakt;
                    break;

            }
                    if (orderSelector != null)
            {
                query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
            }

            var korisnici = query
                              .Skip((page - 1) * pagesize)
                              .Take(pagesize)
                              .ToList();

            var model = new KorisniciViewModel
            {
                Korisnici = korisnici,
                PagingInfo = pagingInfo
            };
            return View(model);
        }
    }
}