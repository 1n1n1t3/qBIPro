using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using qBI.Models;
using qBIPro.Data;


namespace qBIPro.Controllers
{
    [Authorize]
    public class WebScrapeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public WebScrapeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WebScrape
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Scrape(string Url)
        {
            if (Url.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
            {
                Url = Url.Remove(0, 7);

            }
            else if(Url.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
            {
                Url = Url.Remove(0, 8);

            }
            if (Url.StartsWith("www.", StringComparison.InvariantCultureIgnoreCase))
            {
                Url = Url.Remove(0, 4);
            }



            HttpClient hc = new HttpClient();
            HttpResponseMessage result = await hc.GetAsync($"http://{Url}/");

            Stream stream = await result.Content.ReadAsStreamAsync();

            HtmlDocument doc = new HtmlDocument();

            doc.Load(stream);

            HtmlNodeCollection links = doc.DocumentNode.SelectNodes("//a[@href]");//the parameter is use xpath see: https://www.w3schools.com/xml/xml_xpath.asp 
            HtmlNodeCollection iframes = doc.DocumentNode.SelectNodes("//iframe[@src]");//the parameter is use xpath see: https://www.w3schools.com/xml/xml_xpath.asp 

            //return View(links);
            List<String> html = new List<String>();
            if(links != null)
            foreach (var link in links)
            {
                    string linkTxt = link.GetAttributeValue("href", string.Empty);
                
                if(linkTxt.StartsWith("http") && !linkTxt.Contains(Url))
                {
                        if(linkTxt != null)
                            html.Add(linkTxt);
                }
            }
            if(iframes != null)
            foreach (var iframe in iframes)
            {
                string linkTxt = iframe.GetAttributeValue("src", string.Empty);

                if ((linkTxt.StartsWith("http") || linkTxt.StartsWith("//www")) && !linkTxt.Contains(Url))
                {
                        if (linkTxt != null)
                            html.Add(linkTxt);
                }
            }

            ViewData["Result"] = html;

            return View("Index");
        }


    }
}
