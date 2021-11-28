using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HotelOne20251619.Data;
using HotelOne20251619.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace HotelOne20251619.Pages.ManageBooking
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly HotelOne20251619.Data.ApplicationDbContext _context;

        public CreateModel(HotelOne20251619.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["CustomerEmail"] = new SelectList(_context.Customer, "Email", "FullName");
            ViewData["RoomID"] = new SelectList(_context.Room, "ID", "ID");
            return Page();
        }

        [BindProperty]
        public Booking Booking { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var room = new SqliteParameter("room", Booking.RoomID);
            var checkIn = new SqliteParameter("checkin", Booking.CheckIn);
            var checkOut = new SqliteParameter("checkout", Booking.CheckOut);

            var roomAvailability = _context.Booking.FromSqlRaw("select [Booking].RoomID from [Booking] where "
                                   + "(([Booking].RoomID = @room) and ([Booking].CheckOut > @checkin) "
                                   + "and ([Booking].CheckIn < @checkout))", room, checkIn, checkOut);

            var count = roomAvailability.Count();

            if (count == 0 && Booking.CheckIn < Booking.CheckOut)
            {
                _context.Booking.Add(Booking);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
            else if (Booking.CheckIn >= Booking.CheckOut)
            {
                ViewData["CustomerEmail"] = new SelectList(_context.Customer, "Email", "FullName");
                ViewData["RoomID"] = new SelectList(_context.Room, "ID", "ID");
                ViewData["Failure1DB"] = "failure";
                return Page();
            }
            else
            {
                ViewData["CustomerEmail"] = new SelectList(_context.Customer, "Email", "FullName");
                ViewData["RoomID"] = new SelectList(_context.Room, "ID", "ID");
                ViewData["FailureDB"] = "failure";
                return Page();
            }
        }
    }
}
