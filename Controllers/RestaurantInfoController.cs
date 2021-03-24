using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ResAktWebb.Data;
using ResAktWebb.Models;
using RestHelperLib;

namespace ResAktWebb.Controllers
{
    //[Authorize]
    public class RestaurantInfoController : Controller
    {
        private readonly ResAktWebbContext _context;

        public RestaurantInfoController(ResAktWebbContext context)
        {
            _context = context;
        }

        string api = "RestaurantInfoes/";
        // GET: RestaurantInfo
        public async Task<IActionResult> Index()
        {
            var a = await RestHelper.ApiGet<RestaurantInfo>(api);
            
            return View(a);
        }

        // GET: RestaurantInfo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return View(await RestHelper.ApiGet<RestaurantInfo>(api, id));
        }

        // GET: RestaurantInfo/Create
        [Authorize(Roles = "ResAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: RestaurantInfo/Create
        [HttpPost]
        [Authorize(Roles = "ResAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DayName,Open,Closed")] RestaurantInfo restaurantInfo)
        {
            await RestHelper.ApiCreate<RestaurantInfo>(api, restaurantInfo);
            return RedirectToAction("Index");
        }

        // GET: RestaurantInfo/Edit/5
        [Authorize(Roles = "ResAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            return View(await RestHelper.ApiGet<RestaurantInfo>(api, id));
        }

        // POST: RestaurantInfo/Edit/5
        [Authorize(Roles = "ResAdmin")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DayName,Open,Closed")] RestaurantInfo restaurantInfo)
        {
            await RestHelper.ApiEdit<RestaurantInfo>(api + id, restaurantInfo);

            return RedirectToAction("Index");
        }


        // GET: RestaurantInfo/Delete/5
        [Authorize(Roles = "ResAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            return View(await RestHelper.ApiGet<RestaurantInfo>(api, id));
        }

        // POST: RestaurantInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "ResAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await RestHelper.ApiDelete<RestaurantInfo>(api, id);
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantInfoExists(int id)
        {
            return _context.RestaurantInfo.Any(e => e.Id == id);
        }

        public async Task<IActionResult> FullMenu()
		{
            System.Diagnostics.Debug.WriteLine("<-- Full Menu --> \n");

            // Hämta allt
            var menu = await RestHelper.ApiGet<Menu>("Menus");
            var menuCats = await RestHelper.ApiGet<MenuCategory>("MenuCategories/");
            var menuItems = await RestHelper.ApiGet<MenuItems>("MenuItems/");


			foreach (var mC in menuCats )
			{
                foreach (var mI in menuItems)
                {

                    if (mI.MenuCategoryId == mC.Id)
                    {
                        mC.MenuItems.Add(mI);
                        System.Diagnostics.Debug.WriteLine($"<-- ADDED: {mI.Name}\n MenuCategoryId: {mI.MenuCategoryId}");

                    }
                }
			}


            foreach (var m in menu)
			{
				foreach (var mC in menuCats)
				{
					if (mC.MenuId == m.Id)
					{
                        m.MenuCategory.Add(mC);
					}
					
				}
			}


            





            return View(menu);
		}
    }

}
