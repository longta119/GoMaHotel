using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelOne20251619.Data;
using HotelOne20251619.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.Sqlite;

namespace HotelOne20251619.Pages.ManageBooking
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly HotelOne20251619.Data.ApplicationDbContext _context;

        public EditModel(HotelOne20251619.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Booking Booking { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Booking = await _context.Booking
                .Include(b => b.TheCustomer)
                .Include(b => b.TheRoom).FirstOrDefaultAsync(m => m.ID == id);

            if (Booking == null)
            {
                return NotFound();
            }
            ViewData["CustomerEmail"] = new SelectList(_context.Customer, "Email", "FullName");
            ViewData["RoomID"] = new SelectList(_context.Room, "ID", "ID");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Booking).State = EntityState.Modified;

            var room = new SqliteParameter("room", Booking.RoomID);
            var checkIn = new SqliteParameter("checkin", Booking.CheckIn);
            var checkOut = new SqliteParameter("checkout", Booking.CheckOut);

            var roomAvailability = _context.Booking.FromSqlRaw("select [Booking].RoomID " 
                                   + "from [Booking] where (([Booking].RoomID = @room) " 
                                   + " and ([Booking].CheckOut != @checkout) and ([Booking].CheckIn != @checkin)"
                                   + "and ([Booking].CheckOut > @checkin) "
                                   + "and ([Booking].CheckIn < @checkout))", room, checkIn, checkOut);

            var count = roomAvailability.Count();

            if (count != 0)
            {
                ViewData["CustomerEmail"] = new SelectList(_context.Customer, "Email", "FullName");
                ViewData["RoomID"] = new SelectList(_context.Room, "ID", "ID");
                ViewData["FailureDB"] = "failure";
                return Page();
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
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(Booking.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToPage("./Index");
            }
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.ID == id);
        }
    }
}
