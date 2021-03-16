using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResAktWebb.Models;

namespace ResAktWebb.Models
{
    public class Menu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<MenuCategory> MenuCategory { get; set; } = new List<MenuCategory>();
    }
}
