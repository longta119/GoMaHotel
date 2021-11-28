using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelOne20251619.Models
{
    public class RoomStatistic
    {
        [Display(Name = "Room ID")]
        public int RoomID { get; set; }

        [Display(Name = "Number of Bookings")]
        public int numOfBooking { get; set; }
    }
}
