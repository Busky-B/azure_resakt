using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResAktWebb.Models
{
    public class RestaurantInfo
    {
        public int Id { get; set; }
        public string DayName { get; set; }
        public string Open { get; set; }
        public string Closed { get; set; }
    }
}
