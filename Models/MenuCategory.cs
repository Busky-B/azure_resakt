using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResAktWebb.Models
{
    public class MenuCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public int MenuId { get; set; }
        public Menu Menu { get; set; }
        public List<MenuItems> MenuItems { get; set; } = new List<MenuItems>();
    }
}
