using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HotelOne20251619.Data;
using HotelOne20251619.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace HotelOne20251619.Pages.Bookings
{
    [Authorize(Roles = "Customers")]
    public class CreateModel : PageModel
    {
        private readonly HotelOne20251619.Data.ApplicationDbContext _context;

        public CreateModel(HotelOne20251619.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BookingViewModel bookingModel { get; set; }

        public IActionResult OnGet()
        {
        ViewData["RoomID"] = new SelectList(_context.Room, "ID", "ID");
            return Page();
        }
        public IList<Room> CheckRoom { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ViewData["RoomID"] = new SelectList(_context.Room, "ID", "ID");
            // retrieve the logged-in user's email
            // need to add "using System.Security.Claims;"
            string _email = User.FindFirst(ClaimTypes.Name).Value;

            Booking booking = new Booking();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            booking.RoomID = bookingModel.RoomID;
            booking.CustomerEmail = _email;
            booking.CheckIn = bookingModel.CheckIn;
            booking.CheckOut = bookingModel.CheckOut;

            var room = await _context.Room.FirstOrDefaultAsync(m => m.ID == booking.RoomID);
            var customer = await _context.Customer.FirstOrDefaultAsync(m => m.Email == _email);
            var diff = (booking.CheckOut - booking.CheckIn).TotalDays;
            booking.Cost = room.Price*int.Parse(diff.ToString());

            ViewData["cusFullName"] = customer.FullName;
            ViewData["Level"] = room.Level;
            if (int.Parse(diff.ToString()) == 1)
            {
                ViewData["NoOfNight"] = diff + " night";
            }
            else
            {
                ViewData["NoOfNight"] = diff + " nights";
            }
            ViewData["TotalCost"] = booking.Cost;

            if (booking.Cost < 0)
            {
                ViewData["SuccessDB"] = "Wrong Date";
            }
            else
            {
                var CheckInSQL = new SqliteParameter("checkIn", booking.CheckIn);
                var CheckOutSQL = new SqliteParameter("checkOut", booking.CheckOut);
                var CheckRoomID = new SqliteParameter("checkRoomID", booking.RoomID);

                var checkRoom = _context.Room.FromSqlRaw("select [Room].* from [Room] inner join [Booking] on "
                                 + "[Room].ID = [Booking].RoomID where [Booking].CheckIn < @checkOut AND "
                                 + "@checkIn < [Booking].CheckOut AND [Room].ID = @checkRoomID", CheckOutSQL, CheckInSQL, CheckRoomID);

                CheckRoom = await checkRoom.ToListAsync();

                if (CheckRoom.Count == 0)
                {
                    _context.Booking.Add(booking);
                    await _context.SaveChangesAsync();
                    ViewData["SuccessDB"] = "success";
                }
                else
                {
                    ViewData["SuccessDB"] = "fail";
                }
            }
            return Page();
        }
    }
}
