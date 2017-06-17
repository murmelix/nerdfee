using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Nerdfee.Controllers
{
    public class ArtikelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}