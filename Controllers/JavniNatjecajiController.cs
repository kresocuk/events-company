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

    public class JavniNatjecajiController : Controller{

        private readonly PI03Context ctx;
        private readonly AppSettings appData;
        private readonly ILogger <JavniNatjecajiController> logger;

        public JavniNatjecajiController(PI03Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot , ILogger <JavniNatjecajiController> logger)

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
        public IActionResult Create(JavniNatjecaji javninatjecaji) {
            logger.LogTrace(JsonSerializer.Serialize(javninatjecaji), new JsonSerializerOptions {IgnoreNullValues =true });
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(javninatjecaji);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"JavniNatjecaji {javninatjecaji.Id} uspješno dodan.";
                    TempData[Constants.ErrorOccurred] = false;
                    logger.LogInformation($"JavniNatjecaji {javninatjecaji.Id } dodan ");
                    return RedirectToAction(nameof(Index));

                    //samo return View(); ostavi nas na stranici, pa mozemo ponovo dodavati podatke
                }
                catch (Exception e)
                {
                    logger.LogError($"Pogreška prilikom dodavanja novog javninatjecajia {e.CompleteExceptionMessage()}");
                    ModelState.AddModelError(string.Empty, e.CompleteExceptionMessage());
                    return View(javninatjecaji);
                }
            }
            else
            {
                return View(javninatjecaji);
            }
        }

        [HttpGet]
        public IActionResult Edit(int Id, int page = 1, int sort = 1, bool ascending = true)
        {
            var javninatjecaji = ctx.JavniNatjecaji
                                    .AsNoTracking()
                                    .Where(d => d.Id == Id)
                                    //.SingleOrDefault();
                                    .FirstOrDefault();
            if (javninatjecaji == null)
            {
                return NotFound($"ne postoji javni natjecaj s oznakom {Id}");
            }
            else {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascendig = ascending;
                return View(javninatjecaji);
            }
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int Id, int page = 1, int sort = 1, bool ascending = true) {
            try {
                JavniNatjecaji javninatjecaji = await ctx.JavniNatjecaji.FindAsync(Id);
                if (javninatjecaji == null) {
                    return NotFound($"Ne postoji javni natjecaj s tom oznakom {Id}");
                        }
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                bool ok = await TryUpdateModelAsync<JavniNatjecaji>(javninatjecaji, "", d => d.Id, d => d.Detalji, d => d.DobitnaPonuda);
                if (ok)
                {
                    try
                    {
                        TempData[Constants.Message] = $"Javni Natjecaj {javninatjecaji.Id} uspješno azuriran.";
                        TempData[Constants.ErrorOccurred] = false;
                        await ctx.SaveChangesAsync();
                        return RedirectToAction(nameof(Index), new { page, sort, ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(javninatjecaji);
                    }
                }
                else {
                    ModelState.AddModelError(string.Empty,"Podatke o javnom natjecaju nije moguce povezati s forme");
                    return View(javninatjecaji);
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
            var javninatjecaji = ctx.JavniNatjecaji.Find(Id);
            if (javninatjecaji == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    ctx.Remove(javninatjecaji);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"JavniNatjecaji {javninatjecaji.Id} uspješno obrisan.";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception e)
                {
                    TempData[Constants.Message] = $"Pogreška prilikom brisanja javnog natjecaja." + e.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
                return RedirectToAction(nameof(Index), new { page, sort, ascending });
            }

        }

        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true) {

            int pagesize = appData.PageSize;
            var query = ctx.JavniNatjecaji.AsNoTracking();

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

            System.Linq.Expressions.Expression<Func<JavniNatjecaji, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.Id;
                    break;
                case 2:
                    orderSelector = d => d.Vrijeme;
                    break;
                case 3:
                    orderSelector = d => d.NasaPonuda;
                    break;
                case 4:
                    orderSelector = d => d.DobitnaPonuda;
                    break;
            }

            if (orderSelector != null)
            {
                query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
            }

            var javninatjecaji = query
                              .Skip((page - 1) * pagesize)
                              .Take(pagesize)
                              .ToList();

            var model = new JavniNatjecajiViewModel
            {
                JavniNatjecaji = javninatjecaji,
                PagingInfo = pagingInfo
            };
            return View(model);
        }
        public IActionResult Show (int id, int page = 1, int sort = 1, bool ascending = true)
        {
            ViewBag.Page = page;
            ViewBag.Sort = sort;
            ViewBag.Ascending = ascending;

            var javninatjecaj = ctx.JavniNatjecaji
                                            .Where(j => j.Id == id)
                                            .Select(j => new JavniNatjecajViewModel
                                            {
                                                Broj = j.Id, 
                                                Vrijeme = j.Vrijeme,
                                                NasaPonuda = j.NasaPonuda,
                                                DobitnaPonuda = j.DobitnaPonuda,
                                                Detalji = j.Detalji,
                                                                                          })
                                            .FirstOrDefault();
            if (javninatjecaj == null)
            {
                return NotFound($"Javni Natjecaj {id} ne postoji");

            }
            else
            {
                return View(javninatjecaj);
            }
        }
    }
}