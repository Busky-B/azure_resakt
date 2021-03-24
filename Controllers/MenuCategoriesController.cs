using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ResAktWebb.Data;
using ResAktWebb.Models;
using Microsoft.AspNetCore.Authorization;
using RestHelperLib;

namespace ResAktWebb.Controllers
{
    public class MenuCategoriesController : Controller
    {
        private readonly ResAktWebbContext _context;
        string menuCatApi = "menuCategories/";
        string menuApi = "menus/";
        public MenuCategoriesController(ResAktWebbContext context)
        {
            _context = context;
        }

        // GET: MenuCategories
        public async Task<IActionResult> Index(int? id)
        {
            //Om man väljer att gå till sidan utan ett menyID så går det inte
            if (id!=null)
            {
            var menuCategories = await RestHelper.ApiGet<MenuCategory>(menuCatApi);
            var menu = await RestHelper.ApiGet<Menu>(menuApi, id);
            List<MenuCategory> categoriesForMenuId = new List<MenuCategory>();
            ViewData["Menu"] = menu.Name;
            ViewData["route"] = id;
            ViewData["MenuId"] = menu.Id;
                //För att kunna visa underkategorins tillhörande huvudkategori
            foreach (var item in menuCategories)
            {
                if (item.MenuId == id)
                {
                    categoriesForMenuId.Add(item);
                }
            }
            return View(categoriesForMenuId);
            }
            else
            {
                return RedirectToAction("Index","Menus");
            }
        }
        
        // GET: MenuCategories/Details/Id
        public async Task<IActionResult> Details(int? id)
        {
            var menus = await RestHelper.ApiGet<Menu>(menuApi);
            var menucategory = await RestHelper.ApiGet<MenuCategory>(menuCatApi, id);
            foreach (var item in menus)
            {
                if (menucategory.MenuId==item.Id)
                {
                    menucategory.Menu = item;
                }
            }
            
            return View(menucategory);
        }

        // GET: MenuCategories/Create
        [Authorize(Roles = "ResAdmin")]
        public async Task<IActionResult> Create()
        {
            //ViewData["route"] = id;
            var menus = await RestHelper.ApiGet<Menu>(menuApi);
            ViewData["MenuId"] = new SelectList(menus, "Id", "Name");

            return View();
        }

        // POST: MenuCategories/Create
        [HttpPost]
        [Authorize(Roles = "ResAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,MenuId")] MenuCategory menuCategory)
        {
            menuCategory.Id = 0;//För att fixa autoincrement som blir inte automatiskt blir 0
            await RestHelper.ApiCreate<MenuCategory>(menuCatApi, menuCategory);
            return RedirectToAction("Index", "Menus");

        }

        // GET: MenuCategories/Edit/Id
        [Authorize(Roles = "ResAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            var menus = await RestHelper.ApiGet<Menu>(menuApi);
            ViewData["MenuId"] = new SelectList(menus, "Id", "Name");

            return View(await RestHelper.ApiGet<MenuCategory>(menuCatApi, id));
        }

        // POST: MenuCategories/Edit/Id
        [Authorize(Roles = "ResAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,MenuId")] MenuCategory menuCategory)
        {
            await RestHelper.ApiEdit<MenuCategory>(menuCatApi + id, menuCategory);
            return RedirectToAction("Index", "Menus");
        }

        // GET: MenuCategories/Delete/Id
        [Authorize(Roles = "ResAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            var menus = await RestHelper.ApiGet<Menu>(menuApi);
            var menucategory = await RestHelper.ApiGet<MenuCategory>(menuCatApi, id);
            foreach (var item in menus)
            {
                if (menucategory.MenuId == item.Id)
                {
                    menucategory.Menu = item;
                }
            }

            return View(menucategory);
        }

        // POST: MenuCategories/Delete/Id
        [Authorize(Roles = "ResAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await RestHelper.ApiDelete<MenuCategory>(menuCatApi, id);
            return RedirectToAction("Index", "Menus");
        }

        private bool MenuCategoryExists(int id)
        {
            return _context.MenuCategory.Any(e => e.Id == id);
        }
    }
}
