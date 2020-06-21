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

    /// <summary>
    /// controller za posao
    /// </summary>
    public class PosaoController : Controller{

        private readonly PI03Context ctx;
        private readonly AppSettings appData;
        private readonly ILogger <PosaoController> logger;

        public PosaoController(PI03Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot , ILogger <PosaoController> logger)

        {
            this.ctx = ctx;
            this.logger = logger;
            appData = optionsSnapshot.Value;
        }
        /// <summary>
        /// Dohvaca pogled za stvaranje stavki
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// stvara novu stavku
        /// </summary>
        /// <param name="posao"></param>
        /// <returns></returns>

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Posao posao) {
            logger.LogTrace(JsonSerializer.Serialize(posao), new JsonSerializerOptions {IgnoreNullValues =true });
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(posao);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Posao {posao.Naziv} uspješno dodan.";
                    TempData[Constants.ErrorOccurred] = false;
                    logger.LogInformation($"Posao {posao.Naziv } dodan ");
                    return RedirectToAction(nameof(Index));

                    //samo return View(); ostavi nas na stranici, pa mozemo ponovo dodavati podatke
                }
                catch (Exception e)
                {
                    logger.LogError($"Pogreška prilikom dodavanja novog posaoa {e.CompleteExceptionMessage()}");
                    ModelState.AddModelError(string.Empty, e.CompleteExceptionMessage());
                    return View(posao);
                }
            }
            else
            {
                return View(posao);
            }
        }
        /// <summary>
        /// dohvaca pogled za uredivanje stavki
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>

        [HttpGet]
        public IActionResult Edit(int Id, int page = 1, int sort = 1, bool ascending = true)
        {
            var posao = ctx.Posao
                                    .AsNoTracking()
                                    .Where(d => d.Id == Id)
                                    //.SingleOrDefault();
                                    .FirstOrDefault();
            if (posao == null)
            {
                return NotFound($"ne postoji posao s oznakom {Id}");
            }
            else {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascendig = ascending;
                return View(posao);
            }
        }
        /// <summary>
        /// ureduje odredenu stavku
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int Id, int page = 1, int sort = 1, bool ascending = true) {
            try {
                Posao posao = await ctx.Posao.FindAsync(Id);
                if (posao == null) {
                    return NotFound($"Ne postoji posao s tom oznakom {Id}");
                        }
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                bool ok = await TryUpdateModelAsync<Posao>(posao, "", d => d.Id, d => d.Naziv, d => d.Opis);
                if (ok)
                {
                    try
                    {
                        TempData[Constants.Message] = $"Posao {posao.Naziv} uspješno azuriran.";
                        TempData[Constants.ErrorOccurred] = false;
                        await ctx.SaveChangesAsync();
                        return RedirectToAction(nameof(Index), new { page, sort, ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(posao);
                    }
                }
                else {
                    ModelState.AddModelError(string.Empty,"Podatke o posaou nije moguce povezati s forme");
                    return View(posao);
                }
            }
            catch (Exception exc) {
                TempData[Constants.Message] = exc.CompleteExceptionMessage();
                TempData[Constants.ErrorOccurred] = true;
                return RedirectToAction(nameof(Edit), new { Id, page, sort, ascending });
            }
        }
        /// <summary>
        /// brise odredenu stavku
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int Id, int page=1, int sort=1, bool ascending=true)
        {
            var posao = ctx.Posao.Find(Id);
            if (posao == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    string naziv = posao.Naziv;
                    ctx.Remove(posao);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Posao {posao.Naziv} uspješno obrisan.";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception e)
                {
                    TempData[Constants.Message] = $"Pogreška prilikom brisanja posla." + e.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
                return RedirectToAction(nameof(Index), new { page, sort, ascending });
            }

        }

        /// <summary>
        /// dohvaca index pogled 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true) {

            int pagesize = appData.PageSize;
            var query = ctx.Posao.AsNoTracking();

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

            System.Linq.Expressions.Expression<Func<Posao, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.Naziv;
                    break;
                case 2:
                    orderSelector = d => d.Satnica;
                    break;
            }

            if (orderSelector != null)
            {
                query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
            }

            var poslovi = query
                              .Skip((page - 1) * pagesize)
                              .Take(pagesize)
                              .ToList();

            var model = new PosloviViewModel
            {
                Poslovi = poslovi,
                PagingInfo = pagingInfo
            };
            return View(model);
        }
    }
}