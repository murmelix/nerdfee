using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Nerdfee.Contracts;
using Newtonsoft.Json;
using System.IO;
using Nerdfee.Models;
using Nerdfee.Data;
using System.Net.Http;

namespace Nerdfee.Controllers
{
    public class HomeController : Controller
    {
        private static string Token = "EAACEdEose0cBAASsb6ZAL3G3hC3OH5ZCNf7IR3sUcZBvCwPN9d0CkxYMGxJ4OxRAwWO1oftfNSIGYoZC9tJZBZAVpOMEGHFdan6sQfKRgPTr7Kj4JUnTZCFAlBZCKE7D5iggvSs1ZB3a0KZCZCyUNqelvNe4ZCYS6s6f1Ub9MgB1D1DM7jupJXS3ZBGzARcUpDvcEAim91qwlI58P3AZDZD";

        private readonly NerdfeeContext _context;

        public HomeController(NerdfeeContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {            
            return View(_context.Articles.OrderByDescending(x=>x.Erstellt).Take(5));
        }

        public IActionResult Sync()
        {
            var items = new List<Article>();
            
            //_context.ArticleImages.RemoveRange(_context.ArticleImages);
            //_context.SaveChanges();

            var feed = GetData<FeedResponse>("https://graph.facebook.com/v2.9/nerdfee/feed?from=Nerdfee&access_token={0}&format=json&method=get");
            while(feed != null && feed.Data.Count > 0)
            {
                items.AddRange(feed.Data);
                if (feed.Paging == null || string.IsNullOrEmpty(feed.Paging.Next))
                    break;
                feed = GetData<FeedResponse>(feed.Paging.Next);
            }

            foreach (var item in items)
            {
                if (item.Text == null) continue;
                if (item.Text.Length > 200)
                {
                    var parts = item.Text.Split('\n');
                    item.Titel = parts[0];
                    var i = 1;
                    var teaser = "";
                    while (teaser.Length < 150)
                    {
                        if (parts.Length <= i)
                            break;
                        teaser += parts[i++] + "\n";
                    }
                    item.Teaser = teaser;
                    item.Veroeffentlicht = item.Erstellt;
                    var update = _context.Articles.FirstOrDefault(x => x.FacebookId == item.FacebookId);
                    if (update != null)
                    {
                        update.Titel = item.Titel;
                        update.Teaser = item.Teaser;
                        update.Veroeffentlicht = item.Erstellt;
                    }
                    else
                    {
                        _context.Add(item);
                    }
                }
            }
            _context.SaveChanges();

            foreach (var article in _context.Articles)
            {
                //if (_context.ArticleImages.Any(x => x.ArticleId == article.Id))
                //    continue;
                string url = GetPictureUrl(article);
                if (!string.IsNullOrEmpty(url))
                {
                    try
                    {
                        var img = new ArticleImage();
                        img.ArticleId = article.Id;
                        img.OrderBy = 0;
                        img.Data = DownloadImage(url);
                        _context.ArticleImages.Add(img);
                    }catch(Exception ex) { }
                }
            }

            _context.SaveChanges();

            return null;
        }

        private byte[] DownloadImage(string url)
        {
            var httpClient = new HttpClient();
            var r = httpClient.GetStreamAsync(url);
            r.Wait();            
            var memoryStream = new MemoryStream();
            using (memoryStream)
            {
                var t = r.Result.CopyToAsync(memoryStream);
                t.Wait();
                memoryStream.Seek(0,SeekOrigin.Begin);
                return memoryStream.ToArray();
            }
        }

        private T GetData<T>(string url)
        {
            var r = WebRequest.Create(string.Format(url, Token));
            var re = r.GetResponseAsync();
            re.Wait();
            var s = re.Result.GetResponseStream();
            var serializor = JsonSerializer.Create();
            var feed = serializor.Deserialize<T>(new JsonTextReader(new StreamReader(s)));
            return feed;
        }

        private string GetPictureUrl(Article item)
        {
            try
            {
                var feed = GetData<PictureResponse>("https://graph.facebook.com/v2.9/" + item.FacebookId + "?fields=picture&access_token={0}&format=json&method=get");
                if(feed != null && feed.PictureUrl != null)
                {
                    return string.Format("https://graph.facebook.com/v2.9/{1}/picture?type=normal&access_token={0}", Token, feed.Id.Split('_')[1]);
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Impressum()
        {

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
