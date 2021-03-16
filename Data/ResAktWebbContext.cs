using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ResAktWebb.Models;

namespace ResAktWebb.Data
{
    public class ResAktWebbContext : DbContext
    {
        public ResAktWebbContext (DbContextOptions<ResAktWebbContext> options)
            : base(options)
        {
        }


        public DbSet<ResAktWebb.Models.Activity> Activity { get; set; }

        public DbSet<ResAktWebb.Models.Reservation> Reservation { get; set; }

        public DbSet<ResAktWebb.Models.Menu> Menu { get; set; }

        public DbSet<ResAktWebb.Models.MenuCategory> MenuCategory { get; set; }

        public DbSet<ResAktWebb.Models.MenuItems> MenuItems { get; set; }

        public DbSet<ResAktWebb.Models.ActivityBooking> ActivityBooking { get; set; }

        public DbSet<ResAktWebb.Models.RestaurantInfo> RestaurantInfo { get; set; }
    }
}
