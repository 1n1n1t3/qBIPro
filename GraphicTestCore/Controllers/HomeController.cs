using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using qBIPro.Models;
using Microsoft.AspNetCore.Authorization;

namespace qBIPro.Controllers
{
    
    public class HomeController : Controller
    {

        
        public IActionResult Index()
        {

            return View();

        }

        public IActionResult About()
        {
            ViewData["Message"] = "Quick BI Professional";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Quick BI Professional.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
