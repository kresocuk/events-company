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

    public class ReferentniTipController : Controller{

        private readonly PI03Context ctx;
        private readonly AppSettings appData;
        private readonly ILogger <ReferentniTipController> logger;

        public ReferentniTipController(PI03Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot , ILogger <ReferentniTipController> logger)

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
        public IActionResult Create(ReferentniTip tip) {
            logger.LogTrace(JsonSerializer.Serialize(tip), new JsonSerializerOptions {IgnoreNullValues =true });
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(tip);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"ReferentniTip {tip.Naziv} uspje�no dodan.";
                    TempData[Constants.ErrorOccurred] = false;
                    logger.LogInformation($"ReferentniTip {tip.Naziv } dodan ");
                    return RedirectToAction(nameof(Index));

                    //samo return View(); ostavi nas na stranici, pa mozemo ponovo dodavati podatke
                }
                catch (Exception e)
                {
                    logger.LogError($"Pogre�ka prilikom dodavanja novog ref tipa {e.CompleteExceptionMessage()}");
                    ModelState.AddModelError(string.Empty, e.CompleteExceptionMessage());
                    return View(tip);
                }
            }
            else
            {
                return View(tip);
            }
        }

        [HttpGet]
        public IActionResult Edit(int Id, int page = 1, int sort = 1, bool ascending = true)
        {
            var tip = ctx.ReferentniTip
                                    .AsNoTracking()
                                    .Where(d => d.Id == Id)
                                    //.SingleOrDefault();
                                    .FirstOrDefault();
            if (tip == null)
            {
                return NotFound($"ne postoji ref tip s oznakom {Id}");
            }
            else {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascendig = ascending;
                return View(tip);
            }
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int Id, int page = 1, int sort = 1, bool ascending = true) {
            try {
                ReferentniTip tip = await ctx.ReferentniTip.FindAsync(Id);
                if (tip == null) {
                    return NotFound($"Ne postoji tip s tom oznakom {Id}");
                        }
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                bool ok = await TryUpdateModelAsync<ReferentniTip>(tip, "", d => d.Id, d => d.Naziv, d=>d.Opis);
                if (ok)
                {
                    try
                    {
                        TempData[Constants.Message] = $"ReferentniTip {tip.Naziv} uspje�no azuriran.";
                        TempData[Constants.ErrorOccurred] = false;
                        await ctx.SaveChangesAsync();
                        return RedirectToAction(nameof(Index), new { page, sort, ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(tip);
                    }
                }
                else {
                    ModelState.AddModelError(string.Empty,"Podatke o ref tipu nije moguce povezati s forme");
                    return View(tip);
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
            var tip = ctx.ReferentniTip.Find(Id);
            if (tip == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    string naziv = tip.Naziv;
                    ctx.Remove(tip);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"ReferentniTip {tip.Naziv} uspje�no obrisan.";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception e)
                {
                    TempData[Constants.Message] = $"Pogre�ka prilikom brisanja tipa." + e.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
                return RedirectToAction(nameof(Index), new { page, sort, ascending });
            }

        }

        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true) {

            int pagesize = appData.PageSize;
            var query = ctx.ReferentniTip.AsNoTracking();

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

            System.Linq.Expressions.Expression<Func<ReferentniTip, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.Id;
                    break;
                case 2:
                    orderSelector = d => d.Naziv;
                    break;
                case 3:
                    orderSelector = d => d.Opis;
                    break;
            }

            if (orderSelector != null)
            {
                query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
            }

            var tipovi = query
                              .Skip((page - 1) * pagesize)
                              .Take(pagesize)
                              .ToList();

            var model = new ReferentniTipoviViewModel
            {
                ReferentniTipovi = tipovi,
                PagingInfo = pagingInfo
            };
            return View(model);
        }
    }
}