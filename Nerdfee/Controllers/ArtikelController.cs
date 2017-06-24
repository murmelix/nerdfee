using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Nerdfee.Contracts;
using Newtonsoft.Json;
using System.IO;
using Nerdfee.Data;

namespace Nerdfee.Controllers
{
    public class ArtikelController : Controller
    {
        private readonly NerdfeeContext _context;

        public ArtikelController(NerdfeeContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Show(Guid id)
        {
            var hasImage = _context.ArticleImages.Any(x => x.ArticleId == id);
            ViewBag.HasImage = hasImage;
            return View(_context.Articles.First(x => x.Id == id));
        }

        public FileStreamResult GetPicture(Guid id)
        {
            var pic = _context.ArticleImages.FirstOrDefault(x => x.ArticleId == id);
            if (pic == null)
                return null;
            Stream stream = new MemoryStream(pic.Data);
            return new FileStreamResult(stream, "images/png");
        }
    }
}