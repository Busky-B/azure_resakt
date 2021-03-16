using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ResAktWebb.Data;
using ResAktWebb.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace ResAktWebb.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Admin admin)
        {
            //verifiedUser ska användas för att lagra loginAPI response som skickar en bool, och en string[]
            AdminResponse verifiedUser = null;
            using (HttpClient client = new HttpClient())
            {
                
                var content = new StringContent(JsonConvert.SerializeObject(admin), Encoding.UTF8, "application/json");

                //Skickar iväg serialized inmatat username och password till grupp 3 loginAPI
                using (var response = await client.PostAsync("http://informatik10.ei.hv.se/UserService/Login", content))
                {
                    string jR = await response.Content.ReadAsStringAsync();
                    //Tar emot och lägger converterade json svaret i en AdminResponse object
                    verifiedUser = JsonConvert.DeserializeObject<AdminResponse>(jR);
                }
            }

            //Om status är true ska den authenticate user
            if (verifiedUser.Status != false)
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                for(int i = 0; i < verifiedUser.Role.Length; i++)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, verifiedUser.Role[i]));
                }
                identity.AddClaim(new Claim(ClaimTypes.Name, admin.Username));


                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity));
                /*Temp fix: Den redirectar till reservations när man loggar in för jag får 
                 * inte den att redirecta vart man klickade innan inloggningssidan */
                return Redirect("~/Home/Index/");
            }
            else
            {
                //Annars skriver den ut detta
                ModelState.AddModelError("", "Inloggning ej godkänd");
                return View();
            }
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }




        //private async Task AuthenticateUser(Admin validatedLogin)
        //{
        //    //Lokal authentification

        //    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        //    identity.AddClaim(new Claim(ClaimTypes.Name, validatedLogin.Username));

        //    await HttpContext.SignInAsync(
        //        CookieAuthenticationDefaults.AuthenticationScheme,
        //        new ClaimsPrincipal(identity));
        //}


        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,StartTime,EndTime,CustomerName,NumParticipants,TableNum")] Reservation reservation)
        //{
        //    var a = new Reservation();
        //    using (HttpClient c = new HttpClient())
        //    {
        //        string dataAsJson = JsonConvert.SerializeObject(reservation);
        //        var r = await c.PostAsync("http://193.10.202.82/grupp5/api/reservations/", new StringContent(dataAsJson, Encoding.UTF8, "application/json"));
        //    }
        //    return View(a);

        //____________________

        //    var a = new Reservation();
        //    using (HttpClient c = new HttpClient())
        //    {
        //        var r = await c.GetAsync("http://193.10.202.82/grupp5/api/reservations/" + id);
        //        string jsonResponse = await r.Content.ReadAsStringAsync();
        //        try
        //        {
        //            a = JsonConvert.DeserializeObject<Reservation>(jsonResponse);
        //        }
        //        catch (Exception)
        //        {

        //            throw;
        //        }
        //    }
        //    return View(a);


    }
}
