using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelOne20251619.Models
{
    public class PostcodeStatistic
    {
        [Display(Name = "Postcode")]
        public string PostCode { get; set; }

        [Display(Name = "Number of Customers")]
        public int numOfCustomer { get; set; }
    }
}
