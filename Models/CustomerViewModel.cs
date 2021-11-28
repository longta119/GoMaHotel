using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelOne20251619.Models
{
    public class CustomerViewModel
    {
        [Required]
        [Display(Name = "Surname")]
        [MinLength(2), MaxLength(20)]
        [RegularExpression(@"^[A-Z][a-z]{2,20}")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [MinLength(2), MaxLength(20)]
        [RegularExpression(@"^[A-Z][a-z]{2,20}")]
        public string GivenName { get; set; }

        [NotMapped] // not mapping this property to database, but exist in memory
        public string FullName => $"{GivenName} {Surname}";

        [Required]
        [RegularExpression(@"^\d{4}")]
        public string PostCode { get; set; }
    }
}
