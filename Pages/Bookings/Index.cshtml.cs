using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HotelOne20251619.Data;
using HotelOne20251619.Models;

namespace HotelOne20251619.Pages.Bookings
{
    [Authorize(Roles = "Customers")]
    public class IndexModel : PageModel
    {
        private readonly HotelOne20251619.Data.ApplicationDbContext _context;

        public IndexModel(HotelOne20251619.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Booking> Booking { get;set; }

        public async Task<IActionResult> OnGetAsync(string sortOrder)
        {
            if (String.IsNullOrEmpty(sortOrder))
            {
                // When the Index page is loaded for the first time, the sortOrder is empty.
                // By default, the movies should be displayed in the order of title_asc.
                sortOrder = "inDate_asc";
            }

            var booking = (IQueryable<Booking>)_context.Booking;

            switch (sortOrder)
            {
                case "inDate_asc":
                    booking = booking.OrderBy(m => m.CheckIn);
                    break;
                case "inDate_desc":
                    booking = booking.OrderByDescending(m => m.CheckIn);
                    break;
                case "cost_asc":
                    booking = booking.OrderBy(m => (double)m.Cost);
                    break;
                case "cost_desc":
                    booking = booking.OrderByDescending(m => (double)m.Cost);
                    break;
            }

            ViewData["NextInDateOrder"] = sortOrder != "inDate_asc" ? "inDate_asc" : "inDate_desc";
            ViewData["NextCostOrder"] = sortOrder != "cost_asc" ? "cost_asc" : "cost_desc";
            Booking = await booking.Include(s => s.TheCustomer)
                .Include(s => s.TheRoom).AsNoTracking().ToListAsync();

            return Page();
        }
    }
}
