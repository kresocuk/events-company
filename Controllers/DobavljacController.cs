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

    public class DobavljacController : Controller{

        private readonly PI03Context ctx;
        private readonly AppSettings appData;
        private readonly ILogger <DobavljacController> logger;

        public DobavljacController(PI03Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot , ILogger <DobavljacController> logger)

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
        public IActionResult Create(Dobavljac dobavljac) {
            logger.LogTrace(JsonSerializer.Serialize(dobavljac), new JsonSerializerOptions {IgnoreNullValues =true });
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(dobavljac);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Dobavljac {dobavljac.Naziv} uspješno dodan.";
                    TempData[Constants.ErrorOccurred] = false;
                    logger.LogInformation($"Dobavljac {dobavljac.Naziv } dodan ");
                    return RedirectToAction(nameof(Index));

                    //samo return View(); ostavi nas na stranici, pa mozemo ponovo dodavati podatke
                }
                catch (Exception e)
                {
                    logger.LogError($"Pogreška prilikom dodavanja novog dobavljaca {e.CompleteExceptionMessage()}");
                    ModelState.AddModelError(string.Empty, e.CompleteExceptionMessage());
                    return View(dobavljac);
                }
            }
            else
            {
                return View(dobavljac);
            }
        }

        [HttpGet]
        public IActionResult Edit(int Id, int page = 1, int sort = 1, bool ascending = true)
        {
            var dobavljac = ctx.Dobavljac
                                    .AsNoTracking()
                                    .Where(d => d.Id == Id)
                                    //.SingleOrDefault();
                                    .FirstOrDefault();
            if (dobavljac == null)
            {
                return NotFound($"ne postoji dobavljac s oznakom {Id}");
            }
            else {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascendig = ascending;
                return View(dobavljac);
            }
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int Id, int page = 1, int sort = 1, bool ascending = true) {
            try {
                Dobavljac dobavljac = await ctx.Dobavljac.FindAsync(Id);
                if (dobavljac == null) {
                    return NotFound($"Ne postoji dobavljac s tom oznakom {Id}");
                        }
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                bool ok = await TryUpdateModelAsync<Dobavljac>(dobavljac, "", d => d.Id, d => d.Naziv, d => d.Lokacija, d => d.Detalji);
                if (ok)
                {
                    try
                    {
                        TempData[Constants.Message] = $"Dobavljac {dobavljac.Naziv} uspješno azuriran.";
                        TempData[Constants.ErrorOccurred] = false;
                        await ctx.SaveChangesAsync();
                        return RedirectToAction(nameof(Index), new { page, sort, ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(dobavljac);
                    }
                }
                else {
                    ModelState.AddModelError(string.Empty,"Podatke o dobavljacu nije moguce povezati s forme");
                    return View(dobavljac);
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
            var dobavljac = ctx.Dobavljac.Find(Id);
            if (dobavljac == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    string naziv = dobavljac.Naziv;
                    ctx.Remove(dobavljac);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Dobavljac {dobavljac.Naziv} uspješno obrisan.";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception e)
                {
                    TempData[Constants.Message] = $"Pogreška prilikom brisanja dobavljaca." + e.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
                return RedirectToAction(nameof(Index), new { page, sort, ascending });
            }

        }

        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true) {

            int pagesize = appData.PageSize;
            var query = ctx.Dobavljac.AsNoTracking();

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

            System.Linq.Expressions.Expression<Func<Dobavljac, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.Naziv;
                    break;
                case 2:
                    orderSelector = d => d.Lokacija;
                    break;
            
            }

            if (orderSelector != null)
            {
                query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
            }

            var dobavljaci = query
                              .Skip((page - 1) * pagesize)
                              .Take(pagesize)
                              .ToList();

            var model = new DobavljaciViewModel
            {
                Dobavljaci = dobavljaci,
                PagingInfo = pagingInfo
            };
            return View(model);
        }
    }
}