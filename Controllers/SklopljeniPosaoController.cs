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
using System.Dynamic;

namespace TestWebApp.Controllers {

    public class SklopljeniPosaoController : Controller{

        private readonly PI03Context ctx;
        private readonly AppSettings appData;
        private readonly ILogger <SklopljeniPosaoController> logger;

        public SklopljeniPosaoController(PI03Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot , ILogger <SklopljeniPosaoController> logger)

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
        public IActionResult Create(SklopljeniPosao sklopljeniposao) {
            logger.LogTrace(JsonSerializer.Serialize(sklopljeniposao), new JsonSerializerOptions {IgnoreNullValues =true });
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(sklopljeniposao);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"SklopljeniPosao {sklopljeniposao.Id} uspješno dodan.";
                    TempData[Constants.ErrorOccurred] = false;
                    logger.LogInformation($"SklopljeniPosao {sklopljeniposao.Id } dodan ");
                    return RedirectToAction(nameof(Index));

                    //samo return View(); ostavi nas na stranici, pa mozemo ponovo dodavati podatke
                }
                catch (Exception e)
                {
                    logger.LogError($"Pogreška prilikom dodavanja novog sklopljenog posla {e.CompleteExceptionMessage()}");
                    ModelState.AddModelError(string.Empty, e.CompleteExceptionMessage());
                    return View(sklopljeniposao);
                }
            }
            else
            {
                return View(sklopljeniposao);
            }
        }

        [HttpGet]
        public IActionResult Edit(int Id, int page = 1, int sort = 1, bool ascending = true)
        {
            var sklopljeniposao = ctx.SklopljeniPosao
                                    .AsNoTracking()
                                    .Where(d => d.Id == Id)
                                    //.SingleOrDefault();
                                    .FirstOrDefault();
            if (sklopljeniposao == null)
            {
                return NotFound($"ne postoji sklopljeniposao s oznakom {Id}");
            }
            else {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascendig = ascending;
                return View(sklopljeniposao);
            }
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int Id, int page = 1, int sort = 1, bool ascending = true) {
            try {
                SklopljeniPosao sklopljeniposao = await ctx.SklopljeniPosao.FindAsync(Id);
                if (sklopljeniposao == null) {
                    return NotFound($"Ne postoji sklopljeniposao s tom oznakom {Id}");
                        }
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                bool ok = await TryUpdateModelAsync<SklopljeniPosao>(sklopljeniposao, "", d => d.Id, d => d.Cijena, d => d.Vrijeme);
                if (ok)
                {
                    try
                    {
                        TempData[Constants.Message] = $"SklopljeniPosao {sklopljeniposao.Id} uspješno azuriran.";
                        TempData[Constants.ErrorOccurred] = false;
                        await ctx.SaveChangesAsync();
                        return RedirectToAction(nameof(Index), new { page, sort, ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(sklopljeniposao);
                    }
                }
                else {
                    ModelState.AddModelError(string.Empty,"Podatke o sklopljeniposaou nije moguce povezati s forme");
                    return View(sklopljeniposao);
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
            var sklopljeniposao = ctx.SklopljeniPosao.Find(Id);
            if (sklopljeniposao == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
					int id = sklopljeniposao.Id;
                    ctx.Remove(sklopljeniposao);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"SklopljeniPosao {sklopljeniposao.Id} uspješno obrisan.";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception e)
                {
                    TempData[Constants.Message] = $"Pogreška prilikom brisanja sklopljeniposaoa." + e.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
                return RedirectToAction(nameof(Index), new { page, sort, ascending });
            }

        }

        public IActionResult Index(string filter, int page = 1, int sort = 1, bool ascending = true) {

            int pagesize = appData.PageSize;
            var query = ctx.SklopljeniPosao.AsNoTracking();
            SklopljeniPosaoFilter iof = new SklopljeniPosaoFilter();
            if (!string.IsNullOrWhiteSpace(filter))
            {
                iof = SklopljeniPosaoFilter.FromString(filter);
                if (!iof.IsEmpty())
                {
                   
                    query = iof.Apply(query);
                }
            }

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

            System.Linq.Expressions.Expression<Func<SklopljeniPosao, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.Id;
                    break;
                case 2:
                    orderSelector = d => d.Vrijeme;
                    break;
                case 3:
                    orderSelector = d => d.Cijena;
                    break;
                case 4:
                    orderSelector = d => d.Lokacija;
                    break;
                case 5:
                    orderSelector = d => d.Kontakt;
                    break;

            }

            if (orderSelector != null)
            {
                query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
            }

            var sklopljeniposlovi = query
                              .Skip((page - 1) * pagesize)
                              .Take(pagesize)
                              .ToList();

            var model = new SklopljeniPosloviViewModel
            {
                SklopljeniPoslovi = sklopljeniposlovi,
                PagingInfo = pagingInfo,
                Filter = iof
            };
            return View(model);
        }
        [HttpPost]

        public IActionResult Filter(SklopljeniPosaoFilter filter)
        {
            return RedirectToAction(nameof(Index), new { filter = filter.ToString() });
        }

        public IActionResult Show(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            ViewBag.Page = page;
            ViewBag.Sort = sort;
            ViewBag.Ascending = ascending;

            var sklopljeniposao = ctx.SklopljeniPosao
                                            .Where(j => j.Id == id)
                                            .Select(j => new SklopljeniPosao
                                            {
                                                Id = j.Id,
                                                Vrijeme = j.Vrijeme,
                                                Kontakt = j.Kontakt,
                                                Lokacija = j.Lokacija,
                                                Detalji = j.Detalji,
                                                Cijena = j.Cijena,
                                            })
                                            .FirstOrDefault();
            var query1 = ctx.SklopljeniPosaoOsoba.AsNoTracking();
            var sklopljeniposaoosoba = query1.Where(d => d.SklopljeniPosaoId == id)
                                             .ToList();
            var query2 = ctx.SklopljeniPosaoOprema.AsNoTracking();
            var sklopljeniposaooprema = query2.Where(d => d.SklopljeniPosaoId == id)
                                              .ToList();

            var model1 = new SklopljeniPosaoOsobeViewModel
            {
                SklopljeniPosaoOsobe = sklopljeniposaoosoba,

            };
            var model2 = new SklopljeniPosaoOpremeViewModel
            {
                SklopljeniPosaoOpreme = sklopljeniposaooprema,

            };
            var cmodel = new ComplexViewModel
            {
                sposobe = sklopljeniposaoosoba,
                spopreme = sklopljeniposaooprema,
                sklopljeniposao = sklopljeniposao,
            };

            dynamic mymodel = new ExpandoObject();
            mymodel.Model1 = model1;



            if (sklopljeniposao == null)
            {
                return NotFound($"s p {id} ne postoji");
            }
            else
            {
                return View(cmodel);
            }
        }
    }
}