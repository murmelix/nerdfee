using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Nerdfee.Contracts;
using Newtonsoft.Json;
using System.IO;

namespace Nerdfee.Controllers
{
    public class ArtikelController : Controller
    {
        public IActionResult Index()
        {
            var r = WebRequest.Create("https://graph.facebook.com/v2.9/nerdfee/feed?from=Nerdfee&access_token=EAACEdEose0cBAAtyxS9yZBaCGDMpBgnYFrae4poevt17Iejpw5HaKWNyTzn9p81XRUg8OkGEwLEOdNNdw5Jb4ImxjtEUts04YUcFXBInPh5ZCEuoZABrAnyzoaNQe7DZCER1vXyJ5rZAG6mIR9Cu75P2mTGIsfx4AwOdf0E4GLBYaEU8efEkWW9MU27ACy8gZD&format=json&method=get");
            var re = r.GetResponseAsync();
            re.Wait();
            var s = re.Result.GetResponseStream();
            var serializor = JsonSerializer.Create();
            var feed = serializor.Deserialize<FeedResponse>(new JsonTextReader(new StreamReader(s)));
            return View();
        }
    }
}