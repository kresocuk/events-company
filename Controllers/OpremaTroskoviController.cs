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

    public class OpremaTroskoviController : Controller{

        private readonly PI03Context ctx;
        private readonly AppSettings appData;
        private readonly ILogger <OpremaTroskoviController> logger;

        public OpremaTroskoviController(PI03Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot , ILogger <OpremaTroskoviController> logger)

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
        public IActionResult Create(OpremaTroskovi oprematroskovi) {
            logger.LogTrace(JsonSerializer.Serialize(oprematroskovi), new JsonSerializerOptions {IgnoreNullValues =true });
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(oprematroskovi);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"OpremaTroskovi {oprematroskovi.Id} uspješno dodan.";
                    TempData[Constants.ErrorOccurred] = false;
                    logger.LogInformation($"OpremaTroskovi {oprematroskovi.Id } dodan ");
                    return RedirectToAction(nameof(Index));

                    //samo return View(); ostavi nas na stranici, pa mozemo ponovo dodavati podatke
                }
                catch (Exception e)
                {
                    logger.LogError($"Pogreška prilikom dodavanja novog ref oprematroskovia {e.CompleteExceptionMessage()}");
                    ModelState.AddModelError(string.Empty, e.CompleteExceptionMessage());
                    return View(oprematroskovi);
                }
            }
            else
            {
                return View(oprematroskovi);
            }
        }

        [HttpGet]
        public IActionResult Edit(int Id, int page = 1, int sort = 1, bool ascending = true)
        {
            var oprematroskovi = ctx.OpremaTroskovi
                                    .AsNoTracking()
                                    .Where(d => d.Id == Id)
                                    //.SingleOrDefault();
                                    .FirstOrDefault();
            if (oprematroskovi == null)
            {
                return NotFound($"ne postoji ref oprematroskovi s oznakom {Id}");
            }
            else {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascendig = ascending;
                return View(oprematroskovi);
            }
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int Id, int page = 1, int sort = 1, bool ascending = true) {
            try {
                OpremaTroskovi oprematroskovi = await ctx.OpremaTroskovi.FindAsync(Id);
                if (oprematroskovi == null) {
                    return NotFound($"Ne postoji oprematroskovi s tom oznakom {Id}");
                        }
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                bool ok = await TryUpdateModelAsync<OpremaTroskovi>(oprematroskovi, "", d => d.Id, d => d.OpremaId, d=> d.Cijena, d=>d.Opis);
                if (ok)
                {
                    try
                    {
                        TempData[Constants.Message] = $"OpremaTroskovi {oprematroskovi.Id} uspješno azuriran.";
                        TempData[Constants.ErrorOccurred] = false;
                        await ctx.SaveChangesAsync();
                        return RedirectToAction(nameof(Index), new { page, sort, ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(oprematroskovi);
                    }
                }
                else {
                    ModelState.AddModelError(string.Empty,"Podatke o oprematroskoviu nije moguce povezati s forme");
                    return View(oprematroskovi);
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
            var oprematroskovi = ctx.OpremaTroskovi.Find(Id);
            if (oprematroskovi == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    ctx.Remove(oprematroskovi);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"OpremaTroskovi {oprematroskovi.Id} uspješno obrisan.";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception e)
                {
                    TempData[Constants.Message] = $"Pogreška prilikom brisanja oprematroskovia." + e.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
                return RedirectToAction(nameof(Index), new { page, sort, ascending });
            }

        }

        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true) {

            int pagesize = appData.PageSize;
            var query = ctx.OpremaTroskovi.AsNoTracking();

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

            System.Linq.Expressions.Expression<Func<OpremaTroskovi, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.Id;
                    break;
                case 2:
                    orderSelector = d => d.OpremaId;
                    break;
                case 3:
                    orderSelector = d => d.Cijena;
                    break;
            }

            if (orderSelector != null)
            {
                query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
            }

            var oprematroskovi = query
                              .Skip((page - 1) * pagesize)
                              .Take(pagesize)
                              .ToList();

            var model = new OpremaTroskoviViewModel
            {
                OpremaTroskovi = oprematroskovi,
                PagingInfo = pagingInfo
            };
            return View(model);
        }
    }
}