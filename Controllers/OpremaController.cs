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

    public class OpremaController : Controller
    {

        private readonly PI03Context ctx;
        private readonly AppSettings appData;

        public OpremaController(PI03Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot, ILogger<OpremaController> logger)
        {
            this.ctx = ctx;
            appData = optionsSnapshot.Value;
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            await PrepareDropDownLists();
            return View();
        }


        public async Task<FileContentResult> GetImage(string invetarnibroj)
        {
            byte[] image = await ctx.Oprema
                                    .Where(a => a.InventarniBroj == invetarnibroj)
                                    .Select(a => a.Slika)
                                    .SingleOrDefaultAsync();
            if (image != null)
            {
                return File(image, "image/jpeg");
            }
            else
            {
                return null;
            }
        }
        private async Task PrepareDropDownLists()
        {
            var tipovi = await ctx.Tip
                                 .Where(d => d.Id != 0)
                                 .OrderBy(d => d.Naziv)
                                 .Select(d => new { d.Id, d.Naziv})
                                 .ToListAsync();


            ViewBag.Tipovi = new SelectList(tipovi, nameof(Tip.Id), nameof(Tip.Naziv));

            var referentnitipovi = await ctx.ReferentniTip
                                .Where(d => d.Id != 0)
                                .OrderBy(d => d.Naziv)
                                .Select(d => new { d.Id, d.Naziv})
                                .ToListAsync();


            ViewBag.ReferentniTipovi = new SelectList(referentnitipovi, nameof(ReferentniTip.Id), nameof(ReferentniTip.Naziv));
        }

        public async Task<bool> ProvjeriInventarniBroj(string inventarnibroj) {
            bool exists = await ctx.Oprema.AnyAsync(a => a.InventarniBroj == inventarnibroj);
            return !exists;
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Oprema oprema, IFormFile slika)
        {
            bool exists = await ctx.Oprema.AnyAsync(a => a.InventarniBroj == oprema.InventarniBroj);
            if (exists)
            {
                ModelState.AddModelError(nameof(Oprema.InventarniBroj), "Oprema s navedenim inventarnim brojem već postoji");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (slika != null && slika.Length > 0)
                    {
                        using (MemoryStream stream = new MemoryStream())
                        {
                            await slika.CopyToAsync(stream);
                            oprema.Slika = stream.ToArray();
                        }
                    }
                    ctx.Add(oprema);
                    await ctx.SaveChangesAsync();
                    TempData[Constants.Message] = $"{oprema.Naziv} uspješno dodan. Id opreme = {oprema.Id}";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));

                    //samo return View(); ostavi nas na stranici, pa mozemo ponovo dodavati podatke
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.CompleteExceptionMessage());
                    await PrepareDropDownLists();
                    return View(oprema);
                }
            }
            else
            {
                await PrepareDropDownLists();
                return View(oprema);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id) {
            var oprema = await ctx.Oprema.FindAsync(id);
            if (oprema != null)
            {
                return PartialView(oprema);
            }
        
               else
            {
                return NotFound($"neispravan id opreme: {id}");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Oprema oprema, IFormFile slika, bool obrisiSliku)
        {
            if (oprema == null)
            {
                return NotFound("Nema poslanih podataka");
            }
            Oprema dbOprema = ctx.Oprema.FirstOrDefault(a => a.Id == oprema.Id);
            if (dbOprema == null)
            {
                return NotFound($"Neispravan id opreme: {oprema.Id}");
            }

            if (oprema.KnjigovodstvenaVrijednost <= 0)
            {
                ModelState.AddModelError(nameof(Oprema.KnjigovodstvenaVrijednost), "Cijena mora biti pozitivni broj");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //ne možemo ići na varijantu ctx.Update(oprema), jer nismo prenosili sliku, pa bi bila obrisana
                    dbOprema.Naziv = oprema.Naziv;
                    dbOprema.KnjigovodstvenaVrijednost = oprema.KnjigovodstvenaVrijednost;
                    dbOprema.KupovnaVrijednost = oprema.KupovnaVrijednost;
                    dbOprema.VrijemeAmortizacije = oprema.VrijemeAmortizacije;
                    dbOprema.VrijemeKupnje = oprema.VrijemeKupnje;
                    dbOprema.Opis = oprema.Opis;
                    dbOprema.TipId = oprema.TipId;
                    dbOprema.ReferentniTipId = oprema.ReferentniTipId;
                    dbOprema.SkladisteId = oprema.SkladisteId;
                    dbOprema.InventarniBroj = oprema.InventarniBroj;

                    if (slika != null && slika.Length > 0)
                    {
                        using (MemoryStream stream = new MemoryStream())
                        {
                            slika.CopyTo(stream);
                            dbOprema.Slika = stream.ToArray();
                        }
                    }
                    else if (obrisiSliku)
                    {
                        dbOprema.Slika = null;
                    }

                    ctx.SaveChanges();
                    return StatusCode(302, Url.Action(nameof(Row), new { id = dbOprema.Id }));
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return PartialView(oprema);
                }
            }
            else
            {
                return PartialView(oprema);
            }
        }
       
        public PartialViewResult Row(int id)
        {
            var artikl = ctx.Oprema
                             .AsNoTracking()
                             .Where(o => o.Id == id)
                             .Select(o => new OpremaViewModel
                             {
                                 InventarniBroj = o.InventarniBroj,
                                 Naziv = o.Naziv,
                                 TipId = o.TipId,
                                 ReferentniTipId = o.ReferentniTipId,
                                 SkladisteId = o.SkladisteId,
                                 KupovnaVrijednost = o.KupovnaVrijednost,
                                 KnjigovodstvenaVrijednost = o.KnjigovodstvenaVrijednost,
                                 VrijemeKupnje = o.VrijemeKupnje,
                                 ImaSliku = o.Slika != null,
                                 Opis = o.Opis,
                                 Id = o.Id,
                                 VrijemeAmortizacije = o.VrijemeAmortizacije
                             })
                             .SingleOrDefault();
            if (artikl != null)
            {
                return PartialView(artikl);
            }
            else
            {
                //vratiti prazan sadržaj?
                return PartialView("ErrorMessageRow", $"Neispravan id opreme: {id}");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int Id)
        {
            var oprema = await ctx.Oprema.FindAsync(Id);
            if (oprema != null)
            {
                try
                {
                    string naziv = oprema.Naziv;
                    ctx.Remove(oprema);
                    await ctx.SaveChangesAsync();
                    var result = new
                    {
                        message = $"Oprema {naziv} sa sifrom {Id} obrisana.",
                        successful = true
                    };
                    return Json(result);
                }
                catch (Exception exc)
                {
                    var result = new
                    {
                        message = $"Greska prilikom brisanja opreme {exc.CompleteExceptionMessage()}",
                        successful = false
                    };
                    return Json(result);
                }
            }
            else
            {
                return NotFound($"Osoba sa sifrom {Id} ne postoji");
            }
        }

        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {

            int pagesize = appData.PageSize;
            var query = ctx.Oprema.AsNoTracking();

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

            System.Linq.Expressions.Expression<Func<Oprema, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = d => d.Id;
                    break;
                case 2:
                    orderSelector = d => d.Slika;
                    break;
                case 3:
                    orderSelector = d => d.InventarniBroj;
                    break;
                case 4:
                    orderSelector = d => d.Naziv;
                    break;
                case 5:
                    orderSelector = d => d.TipId;
                    break;
                case 6:
                    orderSelector = d => d.ReferentniTipId;
                    break;
                case 7:
                    orderSelector = d => d.SkladisteId;
                    break;
                case 8:
                    orderSelector = d => d.KnjigovodstvenaVrijednost;
                    break;
                case 9:
                    orderSelector = d => d.KupovnaVrijednost;
                    break;
                case 10:
                    orderSelector = d => d.VrijemeKupnje;
                    break;
            }

            if (orderSelector != null)
            {
                query = ascending ? query.OrderBy(orderSelector) : query.OrderByDescending(orderSelector);
            }

            var oprema = query
                              .Select(o => new OpremaViewModel
                              {
                                  InventarniBroj = o.InventarniBroj,
                                  Naziv = o.Naziv,
                                  TipId = o.TipId,
                                  ReferentniTipId = o.ReferentniTipId,
                                  SkladisteId = o.SkladisteId,
                                  KupovnaVrijednost = o.KupovnaVrijednost,
                                  KnjigovodstvenaVrijednost = o.KnjigovodstvenaVrijednost,
                                  VrijemeKupnje = o.VrijemeKupnje,
                                  ImaSliku = o.Slika != null,
                                  Opis = o.Opis,
                                  Id = o.Id,
                                  VrijemeAmortizacije = o.VrijemeAmortizacije
                              })
                              .Skip((page - 1) * pagesize)
                              .Take(pagesize)
                              .ToList();

            var model = new OpremeViewModel
            {
                Oprema = oprema,
                PagingInfo = pagingInfo
            };
            return View(model);
        }
    }
}