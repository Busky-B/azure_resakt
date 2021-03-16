using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResAktWebb.Models
{
    public class ActivityBooking
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Kundnamn")]
        public string CustomerName { get; set; }
        [DisplayName("Antal deltagare")]
        public int NumParticipants { get; set; }

        //bör lägga till email och telefonnummer då vi inte har någon koppling till gästsidan.
 
        // FK
        
        [ForeignKey("Activity")]
        [DisplayName("Aktivitet")]
		public int ActivityId { get; set; }
        //[JsonIgnore]
        [DisplayName("Aktivitet")]
        public virtual Activity Activity { get; set; }

        
	}
}
