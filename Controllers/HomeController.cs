using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ResAktWebb.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace ResAktWebb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        static readonly HttpClient client = new HttpClient();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.LoginMessage = "Välkommen " + User.Identity.Name;
                //User.Identity.Name
            }
            return View();
        }

        //För att testa att webbsidan kan hämta information från API
        //Denna ska finnas i en riktig controller senare
        public async Task<IActionResult> Test()
        {
            //Models.Activity används istället för bara Activity eftersom det blir en conflict med system.diagnostics.activity annars
            List<Models.Activity> activities = new List<Models.Activity>();
            try
            {
                var response = await client.GetAsync("http://193.10.202.82/grupp5/api/activities");
                string jsonresponse = await response.Content.ReadAsStringAsync();
                activities = JsonConvert.DeserializeObject<List<Models.Activity>>(jsonresponse);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return View(activities);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
