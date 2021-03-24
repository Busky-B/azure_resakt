using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ResAktWebb.Data;
using ResAktWebb.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using RestHelperLib;
using Microsoft.Extensions.Logging;

namespace ResAktWebb.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ResAktWebbContext _context;
        private readonly ILogger<ReservationsController> logger;


        public ReservationsController(ResAktWebbContext context, ILogger<ReservationsController> logger)
        {
            _context = context;
            this.logger = logger;
        }


        // Följande metoder använder sig utav en RestHelper class för att utföra  API requests. 


        // Connectionstring till api
        string api = "reservations/";

        [Authorize(Roles = "ResAdmin")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var a = await RestHelper.ApiGet<Reservation>(api);
                return View(a);
            }
            catch (Exception e)
            {

                logger.LogError("GetReservation API call failed \n" + e);
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Reservations/Details/5
        [Authorize(Roles = "ResAdmin")]
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                return View(await RestHelper.ApiGet<Reservation>(api, id));
            }
            catch (Exception e)
            {

                logger.LogError("GetReservationDetails API call failed \n" + e);
                return RedirectToAction("Index", "Reservations");
            }
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {

                logger.LogError("CreateReservation API call failed \n" + e);
                return RedirectToAction("Index", "Reservations");
            }
        }

        // POST: Reservations/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartTime,EndTime,CustomerName,NumParticipants,TableNum")] Reservation reservation)
        {
            try
            {
                await RestHelper.ApiCreate<Reservation>(api, reservation);
            }
            catch (Exception e)
            {
                logger.LogError("Error trying to create new reservation \n" + e);
                throw;
            }
            //När användaren har skapat en reservation så kommer de att tas till respektive hemsida baserat på om de är inloggade som admin eller inte
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("~/Home/Index/");
            }
        }

        // GET: Reservations/Edit/5
        [Authorize(Roles = "ResAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                return View(await RestHelper.ApiGet<Reservation>(api, id));
            }
            catch (Exception e)
            {
                logger.LogError("EditReservation API call failed \n" + e);
                return RedirectToAction("Index", "Reservations");
            }
        }

        // POST: Reservations/Edit/5
        [Authorize(Roles = "ResAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartTime,EndTime,CustomerName,NumParticipants,TableNum")] Reservation reservation)
        {
            try
            {
                await RestHelper.ApiEdit<Reservation>(api + id, reservation);
                logger.LogInformation("Reservation information on id: " + id + " edited");

                return RedirectToAction("Index", "Reservations");
            }
            catch (Exception e)
            {

                logger.LogError("Error trying to edit reservation \n" + e);
                return RedirectToAction("Index", "Reservations");
            }
        }

        // GET: Reservations/Delete/5
        [Authorize(Roles = "ResAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                return View(await RestHelper.ApiGet<Reservation>(api, id));
            }
            catch (Exception e)
            {
                logger.LogError("DeleteReservation API call failed \n" + e);
                return RedirectToAction("Index", "Reservations");
            }
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "ResAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await RestHelper.ApiDelete<Reservation>(api, id);
                logger.LogInformation("Reservation on id: " + id + " deleted");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {

                logger.LogError("Error trying to delete reservation \n" + e);
                return RedirectToAction("Index", "Reservations");
            }
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservation.Any(e => e.Id == id);
        }
    }
}
