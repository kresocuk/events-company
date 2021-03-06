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

    public class SklopljeniPosaoOpremaController : Controller
    {

        private readonly PI03Context ctx;
        private readonly AppSettings appData;
        private readonly ILogger<SklopljeniPosaoOpremaController> logger;

        public SklopljeniPosaoOpremaController(PI03Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot, ILogger<SklopljeniPosaoOpremaController> logger)

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
        public IActionResult Create(SklopljeniPosaoOprema sklopljeniposaoprema)
        {
            logger.LogTrace(JsonSerializer.Serialize(sklopljeniposaoprema), new JsonSerializerOptions { IgnoreNullValues = true });
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(sklopljeniposaoprema);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"SklopljeniPosaoOprema {sklopljeniposaoprema.Id} uspješno dodan.";
                    TempData[Constants.ErrorOccurred] = false;
                    logger.LogInformation($"SklopljeniPosaoOprema {sklopljeniposaoprema.Id } dodan ");
                    return RedirectToAction(nameof(Index));

                    //samo return View(); ostavi nas na stranici, pa mozemo ponovo dodavati podatke
                }
                catch (Exception e)
                {
                    logger.LogError($"Pogreška prilikom dodavanja novog sklopljeniposaopremaa {e.CompleteExceptionMessage()}");
                    ModelState.AddModelError(string.Empty, e.CompleteExceptionMessage());
                    return View(sklopljeniposaoprema);
                }
            }
            else
            {
                return View(sklopljeniposaoprema);
            }
        }

        [HttpGet]
        public IActionResult Edit(int Id, int page = 1, int sort = 1, bool ascending = true)
        {
            var sklopljeniposaoprema = ctx.SklopljeniPosaoOprema
                                    .AsNoTracking()
                                    .Where(d => d.Id == Id)
                                    //.SingleOrDefault();
                                    .FirstOrDefault();
            if (sklopljeniposaoprema == null)
            {
                return NotFound($"ne postoji sklopljeniposaoprema s oznakom {Id}");
            }
            else
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascendig = ascending;
                return View(sklopljeniposaoprema);
            }
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int Id, int page = 1, int sort = 1, bool ascending = true)
        {
            try
            {
                SklopljeniPosaoOprema sklopljeniposaoprema = await ctx.SklopljeniPosaoOprema.FindAsync(Id);
                if (sklopljeniposaoprema == null)
                {
                    return NotFound($"Ne postoji sklopljeniposaoprema s tom oznakom {Id}");
                }
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                bool ok = await TryUpdateModelAsync<SklopljeniPosaoOprema>(sklopljeniposaoprema, "", d => d.Id, d => d.SklopljeniPosaoId, d => d.OpremaId, d => d.Trajanje);
                if (ok)
                {
                    try
                    {
                        TempData[Constants.Message] = $"SklopljeniPosaoOprema {sklopljeniposaoprema.Id} uspješno azuriran.";
                        TempData[Constants.ErrorOccurred] = false;
                        await ctx.SaveChangesAsync();
                        return RedirectToAction(nameof(Index), new { page, sort, ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(sklopljeniposaoprema);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Podatke o sklopljeniposaopremau nije moguce povezati s forme");
                    return View(sklopljeniposaoprema);
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
            var sklopljeniposaoprema = ctx.SklopljeniPosaoOprema.Find(Id);
            if (sklopljeniposaoprema == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    ctx.Remove(sklopljeniposaoprema);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"SklopljeniPosaoOprema {sklopljeniposaoprema.Id} uspješno obrisan.";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception e)
                {
                    TempData[Constants.Message] = $"Pogreška prilikom brisanja sklopljeniposaopremaa." + e.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
                return RedirectToAction(nameof(Index), new { page, sort, ascending });
            }

        }

        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {

            int pagesize = appData.PageSize;
            var query = ctx.SklopljeniPosaoOprema.AsNoTracking();

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

            System.Linq.Expressions.Expression<Func<SklopljeniPosaoOprema, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.Id;
                    break;
                case 2:
                    orderSelector = d => d.SklopljeniPosaoId;
                    break;
                case 3:
                    orderSelector = d => d.OpremaId;
                    break;
                case 4:
                    orderSelector = d => d.Trajanje;
                    break;
            }

            if (orderSelector != null)
            {
                query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
            }

            var sklopljeniposaoopreme = query
                              .Skip((page - 1) * pagesize)
                              .Take(pagesize)
                              .ToList();

            var model = new SklopljeniPosaoOpremeViewModel
            {
                SklopljeniPosaoOpreme = sklopljeniposaoopreme,
                PagingInfo = pagingInfo
            };
            return View(model);
        }
    }
}