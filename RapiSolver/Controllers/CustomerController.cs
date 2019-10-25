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
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customer
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.customers.Include(c => c.Location).Include(c => c.Usuario);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.customers
                .Include(c => c.Location)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            ViewData["LocationId"] = new SelectList(_context.locations, "LocationId", "LocationId");
            ViewData["UsuarioId"] = new SelectList(_context.usuarios, "UsuarioId", "UsuarioId");
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int CustomerId, string Name, string LastName,
            string Email, string Phone, string Age, string Gender, string Country, 
            string State, string City, string Address, string Contraseña)
        {
            
            Usuario u1 = new Usuario();
            u1.UserName = Email;
            u1.UserPassword = Contraseña;
            u1.RolId = 1;
            u1.Rol=_context.roles.Find(u1.RolId);

            _context.Add(u1);
            _context.SaveChanges();

            Location l1 = new Location();
            l1.Country = Country;
            l1.City = City;
            l1.State = State;
            l1.Address =Address;

            _context.Add(l1);
            _context.SaveChanges();

            Customer c1 = new Customer();
           
            c1.Location=l1;
            c1.LocationId=l1.LocationId;
            c1.Usuario=_context.usuarios.Find(c1.UsuarioId);
            c1.UsuarioId = u1.UsuarioId;
            c1.Name=Name;
            c1.LastName=LastName;
            c1.Email = Email;
            c1.Phone = Phone;
            c1.Age = Age;
            c1.Gender = Gender;
            c1.Country = Country;
            c1.State=State;
            c1.City = City;
            c1.Address = Address;
            c1.Contraseña = Contraseña;
                
            _context.Add(c1);
            _context.SaveChanges();



            return Redirect("/Servicio/Index");
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["LocationId"] = new SelectList(_context.locations, "LocationId", "LocationId", customer.LocationId);
            ViewData["UsuarioId"] = new SelectList(_context.usuarios, "UsuarioId", "UsuarioId", customer.UsuarioId);
            return View(customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,Name,LastName,Email,Phone,Age,Gender,UsuarioId,LocationId,Country,State,City,Address,Contraseña")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            ViewData["LocationId"] = new SelectList(_context.locations, "LocationId", "LocationId", customer.LocationId);
            ViewData["UsuarioId"] = new SelectList(_context.usuarios, "UsuarioId", "UsuarioId", customer.UsuarioId);
            return View(customer);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.customers
                .Include(c => c.Location)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.customers.FindAsync(id);
            _context.customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.customers.Any(e => e.CustomerId == id);
        }
    }
}
