using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HotelOne20251619.Models
{
    public class BookingViewModel
    {
        public int RoomID { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Checking in Date")]
        public DateTime CheckIn { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Checking out Date")]
        public DateTime CheckOut { get; set; }
    }
}
