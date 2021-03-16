﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ResAktWebb.Data;
using ResAktWebb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace ResAktWebb.Controllers
{
    
    public class ActivityBookingsController : Controller
    {
        private readonly ResAktWebbContext _context; // Dbcontext. Används inte för att hämta data.
        private readonly string api = "ActivityBookings/"; // Connectionstring till api

        // för loggning
        private readonly ILogger<ActivityBookingsController> _logger;


        public ActivityBookingsController(ResAktWebbContext context, ILogger<ActivityBookingsController> logger)
        {
            _context = context;
           _logger = logger;
        }

        // GET: ActivityBookings
        [Authorize(Roles = "ActAdmin")]
        public async Task<IActionResult> Index()
        {
            return View(await RestHelper.ApiGet<ActivityBooking>(api));
        }

        // GET: ActivityBookings/Details/5
        [Authorize(Roles = "ActAdmin")]
        public async Task<IActionResult> Details(int? id)
        {
            return View(await RestHelper.ApiGet<ActivityBooking>(api, id));
        }

        // GET: ActivityBookings/Create
        public async Task<IActionResult> Create()
        {
            // Får det inte att fungera. errormsg: Cant convert from system.collections.generic.list to IEnumerable
            List<Activity> a = new List<Activity>();
			using (HttpClient client = new HttpClient()) 
			{

            var aResponse = await client.GetAsync("http://informatik12.ei.hv.se/grupp5v2/api/Activities");
            string aJsonResponse = await aResponse.Content.ReadAsStringAsync();
            a = JsonConvert.DeserializeObject<List<Activity>>(aJsonResponse);
            ViewData["ActivityId"] = new SelectList(a, "Id", "Description");
			}
            return View();

            // funkar inte
            /*	ViewData["ActivityId"] = new SelectList(_context.Activity, "Id", "Id");
                var list = (System.Collections.IEnumerable)RestHelper.ApiGet<Activity>("Activities/");
                ViewData["ActivityId"] = new SelectList(list, "Id", "Id");*/

            /* var list = help_plsAsync();
             ViewData["ActivityId"] = new SelectList(list, "Id", "Id");*/
            /*var client = new HttpClient();

            List<Menu> menus = new List<Menu>();
            var menuResponse = await client.GetAsync(api + "menus/");
            string menuJsonResponse = await menuResponse.Content.ReadAsStringAsync();
            menus = JsonConvert.DeserializeObject<List<Menu>>(menuJsonResponse);
            ViewData["MenuId"] = new SelectList(menus, "Id", "Name");*/
        }

        // används inte
        [Authorize(Roles = "ActAdmin")]
        public async Task<IEnumerable<Activity>> help_plsAsync()
		{
            var foo = await RestHelper.ApiGet<Activity>("Activities/");
            var bar = new List<Activity>();
            foo.ForEach(x => {
                var temp = new Activity();
                temp.Id = x.Id;
                temp.Description = x.Description;
                temp.Location = x.Location;
                temp.Price = x.Price;
                temp.StartTime = x.StartTime;
                temp.EndTime = x.EndTime;
                bar.Add(temp);
            });
            return bar.ToArray();
        }

        // POST: ActivityBookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerName,NumParticipants,ActivityId")] ActivityBooking activityBooking)
        {
            await RestHelper.ApiCreate<ActivityBooking>(api, activityBooking);
            return RedirectToAction("Index");
        }

        // GET: ActivityBookings/Edit/5
        [Authorize(Roles = "ActAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            var activityBooking = new ActivityBooking();
			try
			{
                activityBooking = await RestHelper.ApiGet<ActivityBooking>(api, id);
                var acts = await RestHelper.ApiGet<Activity>("Activities/");
                System.Diagnostics.Debug.WriteLine(acts);
                //_context går inte att använda då den inte har en koppling till databasen
                //ViewData["ActivityId"] = new SelectList(_context.Activity, "Id", "Id", activityBooking.ActivityId);
                ViewData["ActivityId"] = new SelectList(acts, "Id", "Description");
            }
			catch (Exception e)
			{
                _logger.LogError("<-- Error was caught when getting object to be edited --> \n " + e);
			}
            
            return View(activityBooking);
        }

        // POST: ActivityBookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerName,NumParticipants,ActivityId")] ActivityBooking activityBooking)
        {
            _logger.LogInformation("<-- Returned to Edit/httppost-->");
            if (id != activityBooking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await RestHelper.ApiEdit(api + id, activityBooking);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityId"] = new SelectList(_context.Activity, "Id", "Id", activityBooking.ActivityId);
            return View(activityBooking);
        }

        // GET: ActivityBookings/Delete/5
        [Authorize(Roles = "ActAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            return View(await RestHelper.ApiGet<ActivityBooking>(api, id));
        }

        // POST: ActivityBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await RestHelper.ApiDelete<ActivityBooking>(api, id);
            return RedirectToAction(nameof(Index));
        }

        /*private bool ActivityBookingExists(int id)
        {
            return _context.ActivityBooking.Any(e => e.Id == id);
        }*/
    }
}
