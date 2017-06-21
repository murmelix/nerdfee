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
    public class HomeController : Controller
    {
        private static string Token = "EAACEdEose0cBAKTWpdv4pqC6wULNKWzR9wF15OTLpr9YQBXfCw9170ZAoGZBRqe4ZBEaL87RRoRuPNktBmt7yjcdZAOjPPCq2hNNokZBB0DP6ZBbVNSqKtQSRZBVGFM9C3bpDfGxTrJdjXsVZC4Q4j0L5gIYnftJei73VsM6TqYlS937uatRmYMUT15kIsq1SAgZD";

        public IActionResult Index()
        {
            //var feed= GetData<FeedResponse>("https://graph.facebook.com/v2.9/nerdfee/feed?from=Nerdfee&access_token={0}&format=json&method=get");
            //foreach(var item in feed.Data)
            //{
            //    GetAdditionalData(item);
            //}
            //return View(feed.Data);
            return View(new List<Article>());
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

        private void GetAdditionalData(Article item)
        {
            try
            {
                var feed = GetData<PictureResponse>("https://graph.facebook.com/v2.9/" + item.Id + "?fields=picture&access_token={0}&format=json&method=get");
                if(feed != null && feed.PictureUrl != null)
                {
                    item.PictureUrl = string.Format("https://graph.facebook.com/v2.9/{1}/picture?type=normal&access_token={0}", Token, feed.Id.Split('_')[1]);
                }
            }
            catch (Exception ex)
            {
            }
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

        public IActionResult Error()
        {
            return View();
        }
    }
}
