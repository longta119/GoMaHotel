using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HotelOne20251619.Models
{
    public class Booking
    {
        //primary key
        public int ID { get; set; }

        //foreign key
        public int RoomID { get; set; }

        //foreign key
        [Required]
        [DataType(DataType.EmailAddress)]
        public string CustomerEmail { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Checking in Date")]
        public DateTime CheckIn { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Checking out Date")]
        public DateTime CheckOut { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Cost")]
        public decimal Cost { get; set; }

        public Room TheRoom { get; set; }
        public Customer TheCustomer { get; set; }
    }
}
