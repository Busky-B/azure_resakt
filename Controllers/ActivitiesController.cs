using System;
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


namespace ResAktWebb.Controllers
{
    //[Authorize]
    public class ActivitiesController : Controller
    {
        private readonly ResAktWebbContext _context;

        public ActivitiesController(ResAktWebbContext context)
        {
            _context = context;
        }

        // Connectionstring till api
        string api = "Activities/";
        public async Task<IActionResult> Index()
        {
          return View(await RestHelper.ApiGet<Activity>(api));
        }


        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var activity = await RestHelper.ApiGet<Activity>(api, id);

            var temp = await RestHelper.ApiGet<ActivityBooking>("ActivityBookings");
            foreach (var item in temp)
            {

                System.Diagnostics.Debug.WriteLine($"<-- MATCH for removal --> \nDebugging templist: \nBookingId\n{item.Id} \nActivityID\n{item.ActivityId} \n{item.CustomerName}\n");
				if (item.ActivityId == id)
				{
                    item.Activity = activity;
                    activity.ActivityBookings.Add(item);
				}
            }
            return View(activity);
        }

        // GET: Activities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Activities/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Location,Price,StartTime,EndTime")] Activity activity)
        {
            await RestHelper.ApiCreate<Activity>(api, activity);
            return RedirectToAction("Index");
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return View(await RestHelper.ApiGet<Activity>(api, id));
        }

        // POST: Activities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Location,Price,StartTime,EndTime")] Activity activity)
        {
            await RestHelper.ApiEdit<Activity>(api + id, activity);

            return RedirectToAction("Index","Activities");
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View(await RestHelper.ApiGet<Activity>(api, id));
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await RestHelper.ApiDelete<Activity>(api, id);
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityExists(int id)
        {
            return _context.Activity.Any(e => e.Id == id);
        }

        public async Task<List<ActivityBooking>> GetChildren (int? id)
        {
            var a = await RestHelper.ApiGet<ActivityBooking>("ActivityBookings/");

            var test = new List<ActivityBooking>();
			foreach (var item in a)
			{
				if (item.ActivityId == id)
				{
                    test.Add(item);
				}
			}

            return test;
        }
    }
}
