using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TestWebApp.Extensions;
using TestWebApp.Models;
using TestWebApp.ViewModels;

namespace TestWebApp.Controllers
{
    /// <summary>
    /// Controller za iznajmljivanje opreme
    /// </summary>
    public class IznajmljivanjeOpremaController : Controller
    {

        private readonly PI03Context ctx;
        private readonly AppSettings appData;

        /// <summary>
        /// Controller za iznajmljivanje opreme
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="optionsSnapshot"></param>
        /// <param name="logger"></param>
        public IznajmljivanjeOpremaController(PI03Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot, ILogger<IznajmljivanjeOpremaController> logger)
        {
            this.ctx = ctx;
            appData = optionsSnapshot.Value;
        }
        /// <summary>
        /// Dohvati pogled za stvaranje nove stavke
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create()
        {
            IznajmljivanjeOpremaViewModel model = new IznajmljivanjeOpremaViewModel{};

            return View(model);
        }


        /// <summary>
        /// Stvara novu stavku
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Create(IznajmljivanjeOpremaViewModel model)
        {
            if (ModelState.IsValid)
            {
                IznajmljivanjeOprema i = new IznajmljivanjeOprema();
                CopyValues(i, model);
                try
                {
                    ctx.Add(i);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"{model.Detalji} uspješno dodan. Id opreme = {i.Id}";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
					//DohvatiNaziveOpreme(model);
                    
                    ModelState.AddModelError(string.Empty, e.CompleteExceptionMessage());
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }
        /// <summary>
        /// Dohvati pogled za uredivanje stavki
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int id) {
            var iznajmljivanjeoprema = await ctx.IznajmljivanjeOprema.FindAsync(id);
            if (iznajmljivanjeoprema != null)
            {
                return PartialView(iznajmljivanjeoprema);
            }
               else
            {
                return NotFound($"neispravan id opreme: {id}");
            }
        }
        /// <summary>
        /// Uredivanje odredene stavke
        /// </summary>
        /// <param name="iznajmljivanjeoprema"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(IznajmljivanjeOprema iznajmljivanjeoprema)
        {
            if (iznajmljivanjeoprema == null)
            {
                return NotFound("Nema poslanih podataka");
            }
            IznajmljivanjeOprema dbIznajmljivanjeOprema = ctx.IznajmljivanjeOprema.FirstOrDefault(a => a.Id == iznajmljivanjeoprema.Id);
            if (dbIznajmljivanjeOprema == null)
            {
                return NotFound($"Neispravan id iznajmljene opreme: {iznajmljivanjeoprema.Id}");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //ne možemo ići na varijantu ctx.Update(iznajmljivanjeoprema), jer nismo prenosili sliku, pa bi bila obrisana
                    dbIznajmljivanjeOprema.Cijena = iznajmljivanjeoprema.Cijena;
                    dbIznajmljivanjeOprema.Detalji = iznajmljivanjeoprema.Detalji;
                    dbIznajmljivanjeOprema.Kontakt = iznajmljivanjeoprema.Kontakt;
                    dbIznajmljivanjeOprema.OpremaId = iznajmljivanjeoprema.OpremaId;
                    dbIznajmljivanjeOprema.Trajanje = iznajmljivanjeoprema.Trajanje;
                    dbIznajmljivanjeOprema.Vrijeme = iznajmljivanjeoprema.Vrijeme;
                    dbIznajmljivanjeOprema.Id = iznajmljivanjeoprema.Id;


                    ctx.SaveChanges();
                    return StatusCode(302, Url.Action(nameof(Row), new { id = dbIznajmljivanjeOprema.Id }));
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return PartialView(iznajmljivanjeoprema);
                }
            }
            else
            {
                return PartialView(iznajmljivanjeoprema);
            }
        }
        /// <summary>
        /// Parcijalni pogled na redak
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PartialViewResult Row(int id)
        {
            var artikl = ctx.IznajmljivanjeOprema
                             .AsNoTracking()
                             .Where(o => o.Id == id)
                             .Select(o => new IznajmljivanjeOpremaViewModel
                             {
                                 Id = o.Id,
                                 Cijena = o.Cijena,
                                 Detalji = o.Detalji,
                                 Kontakt = o.Kontakt,
                                 OpremaId = o.OpremaId,
                                 Trajanje = o.Trajanje,
                                 Vrijeme = o.Vrijeme,
                             })
                             .SingleOrDefault();
            if (artikl != null)
            {
                return PartialView(artikl);
            }
            else
            {
                //vratiti prazan sadržaj?
                return PartialView("ErrorMessageRow", $"Neispravan id iznajmljene opreme: {id}");
            }
        }

        /// <summary>
        /// Brise odredenu stavku iz baze
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int Id, int page = 1, int sort = 1, bool ascending = true)
        {
            var iznajmljivanjeoprema = await ctx.IznajmljivanjeOprema.FindAsync(Id);
            if (iznajmljivanjeoprema != null)
            {
                try
                {
                    ctx.Remove(iznajmljivanjeoprema);
                    await ctx.SaveChangesAsync();
                    TempData[Constants.Message] = $"Iznajmljena oprema s id {iznajmljivanjeoprema.Id} uspjesno obrisana";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = $"Pogreska prilikom brisanja iznajmljene opreme: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
                return RedirectToAction(nameof(Index), new { page, sort, ascending });
            }
            else
            {
                return NotFound();
            }
        }
        /// <summary>
        /// Vrati index za trazeni argument
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="page"></param>
        /// <param name="sort"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        public IActionResult Index(string filter, int page = 1, int sort = 1, bool ascending = true)
        {

            int pagesize = appData.PageSize;
            var query = ctx.IznajmljivanjeOprema.AsQueryable();
            IznajmljivanjeOpremaFilter iof = new IznajmljivanjeOpremaFilter();
            if (!string.IsNullOrWhiteSpace(filter)){
                iof = IznajmljivanjeOpremaFilter.FromString(filter);
                if (!iof.IsEmpty())
                {
                    if (iof.OpremaId.HasValue)
                    {
                        iof.OpremaNaziv = ctx.Oprema
                                             .Where(o => o.Id == iof.OpremaId)
                                             .Select(oi => oi.Naziv)
                                             .FirstOrDefault();
                    }
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
            }
            else if (page > pagingInfo.TotalPages)
            {
                return RedirectToAction(nameof(Index), new { page = pagingInfo.TotalPages, sort, ascending });
            }

            

            query = ApplySort(sort, ascending, query);

            var iznajmljivanjeoprema = query
                              .Select(o => new IznajmljivanjeOpremaViewModel
                              {
                                  Id = o.Id,
                                  Cijena = o.Cijena,
                                  Detalji = o.Detalji,
                                  Kontakt = o.Kontakt,
                                  OpremaId = o.OpremaId,
                                  Trajanje = o.Trajanje,
                                  Vrijeme = o.Vrijeme,
                              })
                              .Skip((page - 1) * pagesize)
                              .Take(pagesize)
                              .ToList();

            var model = new IznajmljivanjeOpremeViewModel
            {
                IznajmljivanjeOprema = iznajmljivanjeoprema,
                PagingInfo = pagingInfo,
                Filter = iof
            };
            return View(model);
        }
        /// <summary>
        /// Primjeni sort na trazeni parametar
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="ascending"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private IQueryable<IznajmljivanjeOprema> ApplySort(int sort, bool ascending, IQueryable<IznajmljivanjeOprema> query)
        {
            System.Linq.Expressions.Expression<Func<IznajmljivanjeOprema, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.Id;
                    break;
                case 2:
                    orderSelector = d => d.Cijena;
                    break;
                case 3:
                    orderSelector = d => d.Detalji;
                    break;
                case 4:
                    orderSelector = d => d.OpremaId;
                    break;
                case 5:
                    orderSelector = d => d.Trajanje;
                    break;
                case 6:
                    orderSelector = d => d.Vrijeme;
                    break;
                case 7:
                    orderSelector = d => d.Kontakt;
                    break;
            }

            if (orderSelector != null)
            {
                query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
            }
            return query;
        }
        /// <summary>
        /// metoda koja kopira vrijednosti
        /// </summary>
        /// <param name="oprema"></param>
        /// <param name="model"></param>
        private void CopyValues(IznajmljivanjeOprema oprema, IznajmljivanjeOpremaViewModel model)
        {
            oprema.Cijena = model.Cijena;
            oprema.Detalji = model.Detalji;
            oprema.OpremaId = model.OpremaId;
            oprema.Kontakt = model.Kontakt;
            oprema.Trajanje = model.Trajanje;
            oprema.Vrijeme = model.Vrijeme;
        }
        /// <summary>
        /// filter metoda
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Filter(IznajmljivanjeOpremaFilter filter)
        {
            return RedirectToAction(nameof(Index), new { filter = filter.ToString() });
        }
       
    }
}