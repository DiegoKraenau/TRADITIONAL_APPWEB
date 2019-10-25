using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RapiSolver.Models;
using RapiSolver.Data;
using Microsoft.EntityFrameworkCore;

namespace RapiSolver.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;


        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Layout()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Inicio()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Inicio(string username, string password)
        {   

            var usuario = _context.usuarios
            .Include(o => o.Rol)
            .Where(o => o.UserName == username && o.UserPassword == password)
            .ToList();
            
            

            if(usuario.First() != null)
            {
                return Redirect("/Servicio/Index");
                
            }

            return NotFound();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
