using Microsoft.AspNetCore.Mvc;
using ResAktWebb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestHelperLib;


namespace ResAktWebb.Controllers
{
	public class GuestController : Controller
	{
		public IActionResult Index()
		{
			// Hit skall guest komma efter inloggning. SUB-Meny behöver finnas för Mina sidor, Restauranginfo och visa bokade aktiviteter/bord.

			return View();
		}

		public async Task<IActionResult> GetActivities()
		{
			var a = await RestHelper.ApiGet<Activity>("Activities");

			return View(a);
		}

		public async Task<IActionResult> GetReservations()
		{
			var a = await RestHelper.ApiGet<Reservation>("Reservations");

			return View(a);
		}

		public async Task<IActionResult> RestaurantInfo()
		{
			var info = await RestHelper.ApiGet<RestaurantInfo>("restaurantInfoes/");
			
			return View(info);
		}

		public async Task<ActionResult> GetClickedActivityAsync(int id)
		{
			var act = await RestHelper.ApiGet<Activity>("Activities/", id);
			System.Diagnostics.Debug.WriteLine(act.Id + act.Description);

			return Json(act);
		}
		public async Task<ActionResult> CreateActivityBooking(string name, int num, int aId)
		{
			System.Diagnostics.Debug.WriteLine("<-- CreateActivityBooking -->");
			System.Diagnostics.Debug.WriteLine("name: " + name);
			System.Diagnostics.Debug.WriteLine("num: " + num);
			System.Diagnostics.Debug.WriteLine("aId: " + aId);



			var newAB = new ActivityBooking();
			newAB.CustomerName = name;
			newAB.NumParticipants = num;
			newAB.ActivityId = aId;
			var response = "<-- CreateActivityBooking was called -->";


			/// Fungerar, MEN måste fixa så att när den får null att det inte crashar.
			// var s = RestHelper.PrintObjProps<ActivityBooking>(newAB);
			// System.Diagnostics.Debug.WriteLine($"<-- {s} -->");
				
			await RestHelper.ApiCreate<ActivityBooking>("ActivityBookings/", newAB);
			

			return Json(response);
		}

	}
}
