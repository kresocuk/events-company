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

    public class UnajmljivanjeOpremaController : Controller{

        private readonly PI03Context ctx;
        private readonly AppSettings appData;
        private readonly ILogger <UnajmljivanjeOpremaController> logger;

        public UnajmljivanjeOpremaController(PI03Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot , ILogger <UnajmljivanjeOpremaController> logger)

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
        public IActionResult Create(UnajmljivanjeOprema unajmljivanjeoprema) {
            logger.LogTrace(JsonSerializer.Serialize(unajmljivanjeoprema), new JsonSerializerOptions {IgnoreNullValues =true });
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(unajmljivanjeoprema);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"UnajmljivanjeOprema {unajmljivanjeoprema.Naziv} uspješno dodan.";
                    TempData[Constants.ErrorOccurred] = false;
                    logger.LogInformation($"UnajmljivanjeOprema {unajmljivanjeoprema.Naziv } dodan ");
                    return RedirectToAction(nameof(Index));

                    //samo return View(); ostavi nas na stranici, pa mozemo ponovo dodavati podatke
                }
                catch (Exception e)
                {
                    logger.LogError($"Pogreška prilikom dodavanja novog unajmljivanjeopremaa {e.CompleteExceptionMessage()}");
                    ModelState.AddModelError(string.Empty, e.CompleteExceptionMessage());
                    return View(unajmljivanjeoprema);
                }
            }
            else
            {
                return View(unajmljivanjeoprema);
            }
        }

        [HttpGet]
        public IActionResult Edit(int Id, int page = 1, int sort = 1, bool ascending = true)
        {
            var unajmljivanjeoprema = ctx.UnajmljivanjeOprema
                                    .AsNoTracking()
                                    .Where(d => d.Id == Id)
                                    //.SingleOrDefault();
                                    .FirstOrDefault();
            if (unajmljivanjeoprema == null)
            {
                return NotFound($"ne postoji unajmljivanjeoprema s oznakom {Id}");
            }
            else {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascendig = ascending;
                return View(unajmljivanjeoprema);
            }
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int Id, int page = 1, int sort = 1, bool ascending = true) {
            try {
                UnajmljivanjeOprema unajmljivanjeoprema = await ctx.UnajmljivanjeOprema.FindAsync(Id);
                if (unajmljivanjeoprema == null) {
                    return NotFound($"Ne postoji unajmljivanjeoprema s tom oznakom {Id}");
                        }
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                bool ok = await TryUpdateModelAsync<UnajmljivanjeOprema>(unajmljivanjeoprema, "", d => d.Id, d => d.Naziv, d => d.Vrijeme, d => d.Trajanje, d => d.Cijena, d => d.Detalji, d => d.ReferentniTipId);
                if (ok)
                {
                    try
                    {
                        TempData[Constants.Message] = $"UnajmljivanjeOprema {unajmljivanjeoprema.Naziv} uspješno azuriran.";
                        TempData[Constants.ErrorOccurred] = false;
                        await ctx.SaveChangesAsync();
                        return RedirectToAction(nameof(Index), new { page, sort, ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(unajmljivanjeoprema);
                    }
                }
                else {
                    ModelState.AddModelError(string.Empty,"Podatke o unajmljivanjeopremau nije moguce povezati s forme");
                    return View(unajmljivanjeoprema);
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
            var unajmljivanjeoprema = ctx.UnajmljivanjeOprema.Find(Id);
            if (unajmljivanjeoprema == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    string naziv = unajmljivanjeoprema.Naziv;
                    ctx.Remove(unajmljivanjeoprema);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"UnajmljivanjeOprema {unajmljivanjeoprema.Naziv} uspješno obrisan.";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception e)
                {
                    TempData[Constants.Message] = $"Pogreška prilikom brisanja unajmljivanjeopremaa." + e.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
                return RedirectToAction(nameof(Index), new { page, sort, ascending });
            }

        }

        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true) {

            int pagesize = appData.PageSize;
            var query = ctx.UnajmljivanjeOprema.AsNoTracking();

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

            System.Linq.Expressions.Expression<Func<UnajmljivanjeOprema, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.Naziv;
                    break;
                case 2:
                    orderSelector = d => d.Vrijeme;
                    break;
                case 3:
                    orderSelector = d => d.Trajanje;
                    break;
                case 4:
                    orderSelector = d => d.Cijena;
                    break;
                case 5:
                    orderSelector = d => d.Detalji;
                    break;
                case 6:
                    orderSelector = d => d.ReferentniTipId;
                    break;
            }

            if (orderSelector != null)
            {
                query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
            }

            var unajmljivanjeopreme = query
                              .Skip((page - 1) * pagesize)
                              .Take(pagesize)
                              .ToList();

            var model = new UnajmljivanjeOpremeViewModel
            {
                UnajmljivanjeOpreme = unajmljivanjeopreme,
                PagingInfo = pagingInfo
            };
            return View(model);
        }
    }
}