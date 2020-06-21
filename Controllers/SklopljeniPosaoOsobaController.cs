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

namespace TestWebApp.Controllers
{

    public class SklopljeniPosaoOsobaController : Controller
    {

        private readonly PI03Context ctx;
        private readonly AppSettings appData;
        private readonly ILogger<SklopljeniPosaoOsobaController> logger;

        public SklopljeniPosaoOsobaController(PI03Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot, ILogger<SklopljeniPosaoOsobaController> logger)

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
        public IActionResult Create(SklopljeniPosaoOsoba sklopljeniposaoosoba)
        {
            logger.LogTrace(JsonSerializer.Serialize(sklopljeniposaoosoba), new JsonSerializerOptions { IgnoreNullValues = true });
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(sklopljeniposaoosoba);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"SklopljeniPosaoOsoba {sklopljeniposaoosoba.Id} uspješno dodan.";
                    TempData[Constants.ErrorOccurred] = false;
                    logger.LogInformation($"SklopljeniPosaoOsoba {sklopljeniposaoosoba.Id } dodan ");
                    return RedirectToAction(nameof(Index));

                    //samo return View(); ostavi nas na stranici, pa mozemo ponovo dodavati podatke
                }
                catch (Exception e)
                {
                    logger.LogError($"Pogreška prilikom dodavanja novog sklopljeniposaoosobaa {e.CompleteExceptionMessage()}");
                    ModelState.AddModelError(string.Empty, e.CompleteExceptionMessage());
                    return View(sklopljeniposaoosoba);
                }
            }
            else
            {
                return View(sklopljeniposaoosoba);
            }
        }

        [HttpGet]
        public IActionResult Edit(int Id, int page = 1, int sort = 1, bool ascending = true)
        {
            var sklopljeniposaoosoba = ctx.SklopljeniPosaoOsoba
                                    .AsNoTracking()
                                    .Where(d => d.Id == Id)
                                    //.SingleOrDefault();
                                    .FirstOrDefault();
            if (sklopljeniposaoosoba == null)
            {
                return NotFound($"ne postoji sklopljeniposaoosoba s oznakom {Id}");
            }
            else
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascendig = ascending;
                return View(sklopljeniposaoosoba);
            }
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int Id, int page = 1, int sort = 1, bool ascending = true)
        {
            try
            {
                SklopljeniPosaoOsoba sklopljeniposaoosoba = await ctx.SklopljeniPosaoOsoba.FindAsync(Id);
                if (sklopljeniposaoosoba == null)
                {
                    return NotFound($"Ne postoji sklopljeniposaoosoba s tom oznakom {Id}");
                }
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                bool ok = await TryUpdateModelAsync<SklopljeniPosaoOsoba>(sklopljeniposaoosoba, "", d => d.Id, d => d.SklopljeniPosaoId, d => d.PosaoId, d => d.OsobaId, d => d.Trajanje);
                if (ok)
                {
                    try
                    {
                        TempData[Constants.Message] = $"SklopljeniPosaoOsoba {sklopljeniposaoosoba.Id} uspješno azuriran.";
                        TempData[Constants.ErrorOccurred] = false;
                        await ctx.SaveChangesAsync();
                        return RedirectToAction(nameof(Index), new { page, sort, ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(sklopljeniposaoosoba);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Podatke o sklopljeniposaoosobau nije moguce povezati s forme");
                    return View(sklopljeniposaoosoba);
                }
            }
            catch (Exception exc)
            {
                TempData[Constants.Message] = exc.CompleteExceptionMessage();
                TempData[Constants.ErrorOccurred] = true;
                return RedirectToAction(nameof(Edit), new { Id, page, sort, ascending });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int Id, int page = 1, int sort = 1, bool ascending = true)
        {
            var sklopljeniposaoosoba = ctx.SklopljeniPosaoOsoba.Find(Id);
            if (sklopljeniposaoosoba == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    ctx.Remove(sklopljeniposaoosoba);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"SklopljeniPosaoOsoba {sklopljeniposaoosoba.Id} uspješno obrisan.";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception e)
                {
                    TempData[Constants.Message] = $"Pogreška prilikom brisanja sklopljeniposaoosobaa." + e.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
                return RedirectToAction(nameof(Index), new { page, sort, ascending });
            }

        }

        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {

            int pagesize = appData.PageSize;
            var query = ctx.SklopljeniPosaoOsoba.AsNoTracking();

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
            }
            else if (page > pagingInfo.TotalPages)
            {
                return RedirectToAction(nameof(Index), new { page = pagingInfo.TotalPages, sort, ascending });
            }

            System.Linq.Expressions.Expression<Func<SklopljeniPosaoOsoba, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.Id;
                    break;
                case 2:
                    orderSelector = d => d.SklopljeniPosaoId;
                    break;
                case 3:
                    orderSelector = d => d.PosaoId;
                    break;
                case 4:
                    orderSelector = d => d.OsobaId;
                    break;
                case 5:
                    orderSelector = d => d.Trajanje;
                    break;
            }

            if (orderSelector != null)
            {
                query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
            }

            var sklopljeniposaoosobe = query
                              .Skip((page - 1) * pagesize)
                              .Take(pagesize)
                              .ToList();

            var model = new SklopljeniPosaoOsobeViewModel
            {
                SklopljeniPosaoOsobe = sklopljeniposaoosobe,
                PagingInfo = pagingInfo
            };
            return View(model);
        }
    }
}