using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HotelOne20251619.Models
{
    public class Room
    {
        //private key
        [Display(Name = "Room ID")]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Level")]
        [RegularExpression(@"^(?:G|g|1|2|3)")]
        public string Level { get; set; }

        [Display(Name = "Bed Count")]
        [Range(1, 3)]
        public int BedCount { get; set; }

        [Range(50, 300.0)]
        public decimal Price { get; set; }

        public ICollection<Booking> TheBookings { get; set; }
    }
}
