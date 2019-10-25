using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RapiSolver.Data;
using RapiSolver.Models;

namespace RapiSolver.Controllers
{
    public class ServicioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServicioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Servicio
        public async Task<IActionResult> Index(string searchString)
        {
            var applicationDbContext = _context.servicios.Include(s => s.ServiceCategory);

            var servicios = from s in _context.servicios select s;
            servicios = servicios.Include(s => s.ServiceCategory);

            if (!String.IsNullOrEmpty(searchString))
            {
                servicios = servicios.Where(s => s.Name.Contains(searchString));
            }

            return View(await servicios.ToListAsync());
        }


        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        // GET: Servicio/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var details = _context.serviceDetails
                .Include(o => o.Supplier)
                .Include(o => o.Supplier.Location)
                .Include(o => o.Supplier.Usuario)
                .Include(o => o.Servicio)
                .Include(o => o.Servicio.ServiceCategory)
                .OrderByDescending(o => o.ServiceDetailsId)
                .Take(10)
                .Where(o => o.ServicioId == id)
                .ToList();


            IEnumerable<ServiceDetailsViewModel> enumerable = details.Select(o => new ServiceDetailsViewModel
            {
                ServiceDetailsId = o.ServiceDetailsId,
                SupplierId = o.SupplierId,
                ServicioId = o.ServicioId,
                Name = o.Supplier.Name,
                LastName = o.Supplier.LastName,
                Email = o.Supplier.Email,
                Phone = o.Supplier.Phone,
                Age = o.Supplier.Age,
                Gender = o.Supplier.Gender,
                UsuarioId = o.Supplier.Usuario.UsuarioId,
                LocationId = o.Supplier.Location.LocationId,
                UserName = o.Supplier.Usuario.UserName,
                Country = o.Supplier.Location.Country,
                ServiceName = _context.serviceDetails.Find(o.ServiceDetailsId).Servicio.Name,
                Description = _context.serviceDetails.Find(o.ServiceDetailsId).Servicio.Description,
                Cost = _context.serviceDetails.Find(o.ServiceDetailsId).Servicio.Cost,
                ServiceCategoryId = _context.serviceDetails.Find(o.ServiceDetailsId).Servicio.ServiceCategoryId,
                CategoryName = _context.serviceDetails.Find(o.ServiceDetailsId).Servicio.ServiceCategory.CategoryName
            });
            ServiceDetailsViewModel detalle = new ServiceDetailsViewModel();
            detalle = enumerable.First();


            if (enumerable == null)
            {
                return NotFound();
            }
            //ServiceDetails sd = _context.servicios.Find()
            //ViewBag.Supplier = _context.suppliers.Find(servicio.ServicioId)

            ViewBag.Detalle = detalle;
            return View();



        }

        // GET: Servicio/Create
        public IActionResult Create()
        {
            ViewData["ServiceCategoryId"] = new SelectList(_context.categories, "ServiceCategoryId", "CategoryName");
            return View();
        }

        // POST: Servicio/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int ServicioId, string Name, string Description, string Cost, int ServiceCategoryId)
        {
            Servicio servicio = new Servicio();
            servicio.ServicioId = ServicioId;
            servicio.Name = Name;
            servicio.Description = Description;
            servicio.Cost = Cost;
            servicio.ServiceCategoryId = ServiceCategoryId;
            
            if (ModelState.IsValid)
            {
                _context.Add(servicio);
                _context.SaveChanges();
            }
            ViewData["ServiceCategoryId"] = new SelectList(_context.categories, "ServiceCategoryId", "ServiceCategoryId", servicio.ServiceCategoryId);

            ServiceDetails sd1 = new ServiceDetails();
            sd1.Servicio = _context.servicios.Find(ServicioId);
            sd1.ServicioId = servicio.ServicioId;
            sd1.Supplier = _context.suppliers.Find(1);
            sd1.SupplierId = 1;
            _context.Add(sd1);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: Servicio/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicio = await _context.servicios.FindAsync(id);
            if (servicio == null)
            {
                return NotFound();
            }
            ViewData["ServiceCategoryId"] = new SelectList(_context.categories, "ServiceCategoryId", "ServiceCategoryId", servicio.ServiceCategoryId);
            return View(servicio);
        }

        // POST: Servicio/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServicioId,Name,Description,Cost,ServiceCategoryId")] Servicio servicio)
        {
            if (id != servicio.ServicioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(servicio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicioExists(servicio.ServicioId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ServiceCategoryId"] = new SelectList(_context.categories, "ServiceCategoryId", "ServiceCategoryId", servicio.ServiceCategoryId);
            return View(servicio);
        }

        // GET: Servicio/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicio = await _context.servicios
                .Include(s => s.ServiceCategory)
                .FirstOrDefaultAsync(m => m.ServicioId == id);
            if (servicio == null)
            {
                return NotFound();
            }

            return View(servicio);
        }

        // POST: Servicio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servicio = await _context.servicios.FindAsync(id);
            _context.servicios.Remove(servicio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServicioExists(int id)
        {
            return _context.servicios.Any(e => e.ServicioId == id);
        }
    }
}
