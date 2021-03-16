using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResAktWebb.Models
{
    public class RestaurantInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Open { get; set; }
        public DateTime Closed { get; set; }
    }
}
