using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using HotelOne20251619.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HotelOne20251619.Pages.Customers
{
    [Authorize(Roles = "Admin")]
    public class CalcStatsModel : PageModel
    {
        private readonly HotelOne20251619.Data.ApplicationDbContext _context;

        public CalcStatsModel(HotelOne20251619.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<PostcodeStatistic> PostcodeStats { get; set; }
        public IList<RoomStatistic> RoomStats { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // divide into groups by postcode
            var postcodeGroups = _context.Customer.GroupBy(m => m.PostCode);

            // for each group, get its postcode value and the number of customer in this group
            PostcodeStats = await postcodeGroups.Select(g => new PostcodeStatistic { PostCode = g.Key, numOfCustomer = g.Count() }).ToListAsync();

            // divide the room into groups by roomID
            var roomGroups = _context.Booking.GroupBy(m => m.RoomID);

            // for each group, get its roomID value and the number of booking in this group
            RoomStats = await roomGroups.Select(g => new RoomStatistic { RoomID = g.Key, numOfBooking = g.Count() }).ToListAsync();

            return Page();
        }

         /*
        public void OnGet()
        {
        }
         */
    }
}
