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
using System.Text;
using RestHelperLib;

namespace ResAktWebb.Controllers
{
    public class MenuItemsController : Controller
    {
        private readonly ResAktWebbContext _context;
        static readonly HttpClient client = new HttpClient();
        string api = "http://informatik12.ei.hv.se/grupp5/api/";
        string menuItemApi = "menuItems/";
        string menuCatApi = "menuCategories/";
        string menuApi = "menus/";

        public MenuItemsController(ResAktWebbContext context)
        {
            _context = context;
        }

        // GET: MenuItems
        public async Task<IActionResult> Index(int? id)
        {
            //Om man väljer att gå till sidan utan ett menyID så går det inte
            if (id != null)
            {
                var menuItems = await RestHelper.ApiGet<MenuItems>(menuItemApi);
                var menuCategory = await RestHelper.ApiGet<MenuCategory>(menuCatApi, id);
                ViewData["route"] = id;
                List<MenuItems> itemsForCategoryId = new List<MenuItems>();
                ViewData["MenuCategory"] = menuCategory.Name;
                //För att kunna visa maträttens/dryckens tillhörande underkategori
                foreach (var item in menuItems)
                {
                    if (item.MenuCategoryId == id)
                    {
                        itemsForCategoryId.Add(item);
                    }
                }
                return View(itemsForCategoryId);
            }
            else
            {
                return RedirectToAction("Index", "Menus");
            }
        }

        // GET: MenuItems/Details/Id
        public async Task<IActionResult> Details(int? id)
        {
            var menuCats = await RestHelper.ApiGet<MenuCategory>(menuCatApi);
            var menuitem = await RestHelper.ApiGet<MenuItems>(menuItemApi, id);
            foreach (var item in menuCats)
            {
                if (menuitem.MenuCategoryId == item.Id)
                {
                    menuitem.MenuCategory = item;
                }
            }

            return View(menuitem);
        }

        // GET: MenuItems/Create
        [Authorize(Roles = "ResAdmin")]
        public async Task<IActionResult> Create(int? id)
        {
            ViewData["route"] = id;
            var menuCategories = await RestHelper.ApiGet<MenuCategory>(menuCatApi);
            ViewData["MenuCatId"] = new SelectList(menuCategories, "Id", "Name");
            return View();
        }

        // POST: MenuItems/Create
        [HttpPost]
        [Authorize(Roles = "ResAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Allergies,Price,MenuCategoryId")] MenuItems menuItems)
        {
            menuItems.Id = 0;
            await RestHelper.ApiCreate<MenuItems>(menuItemApi, menuItems);
            return RedirectToAction("Index", "Menus"/*, new { id = menuItems.MenuCategoryId }*/);
        }

        // GET: MenuItems/Edit/Id
        [Authorize(Roles = "ResAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            var menuCategories = await RestHelper.ApiGet<MenuCategory>(menuCatApi);
            ViewData["MenuCatId"] = new SelectList(menuCategories, "Id", "Name");

            return View(await RestHelper.ApiGet<MenuItems>(menuItemApi, id));
        }

        // POST: MenuItems/Edit/Id
        [HttpPost]
        [Authorize(Roles = "ResAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Allergies,Price,MenuCategoryId")] MenuItems menuItems)
        {
            await RestHelper.ApiEdit<MenuItems>(menuItemApi + id, menuItems);
            return RedirectToAction("Index", "Menus");
        }

        // GET: MenuItems/Delete/Id
        [Authorize(Roles = "ResAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            var menuCats = await RestHelper.ApiGet<MenuCategory>(menuCatApi);
            var menuitem = await RestHelper.ApiGet<MenuItems>(menuItemApi, id);
            foreach (var item in menuCats)
            {
                if (menuitem.MenuCategoryId == item.Id)
                {
                    menuitem.MenuCategory = item;
                }
            }

            return View(menuitem);
        }

        // POST: MenuItems/Delete/Id
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "ResAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await RestHelper.ApiDelete<MenuItems>(menuItemApi, id);
            return RedirectToAction("Index", "Menus");
        }

        private bool MenuItemsExists(int id)
        {
            return _context.MenuItems.Any(e => e.Id == id);
        }
    }
}
