using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HotelOne20251619.Data;
using HotelOne20251619.Models;
using Microsoft.AspNetCore.Authorization;

namespace HotelOne20251619.Pages.ManageBooking
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly HotelOne20251619.Data.ApplicationDbContext _context;

        public IndexModel(HotelOne20251619.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Booking> Booking { get;set; }

        public async Task OnGetAsync()
        {
            Booking = await _context.Booking
                .Include(b => b.TheCustomer)
                .Include(b => b.TheRoom).ToListAsync();
        }
    }
}
