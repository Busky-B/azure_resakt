using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ResAktWebb.Models
{
    public class Activity
    {
        public int Id { get; set; }
        [DisplayName("Beskrivning")]
        public string Description { get; set; }
        [DisplayName("Plats")]
        public string Location { get; set; }
        [DisplayName("Pris")]
        public decimal Price { get; set; }
        [DisplayName("Starttid")]
        public DateTime StartTime { get; set; }
        [DisplayName("Sluttid")]
        public DateTime EndTime { get; set; }

        public virtual List<ActivityBooking> ActivityBookings { get; set; } = new List<ActivityBooking>();


      //  public List<ActivityBooking> ActivityBookings{ get; set; } = new List<ActivityBooking>();
    }
}
