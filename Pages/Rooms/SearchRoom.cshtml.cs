using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using HotelOne20251619.Models;

namespace HotelOne20251619.Pages.Rooms
{
    [Authorize(Roles = "Customers")]
    public class SearchRoomModel : PageModel
    {
        private readonly HotelOne20251619.Data.ApplicationDbContext _context;
        public SearchRoomModel(HotelOne20251619.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Booking RoomInput { get; set; }
        public IList<Room> CheckRoom { get; set; }
        public IActionResult OnGet()
        {
            // Get the options for the MovieGoer select list from the database
            // and save them in ViewData for passing to Content file
            ViewData["BedCount"] = Enumerable.Range(1, 3)
                .Select(n => new SelectListItem
                {
                    Value = n.ToString(),
                    Text = n.ToString()
                }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ViewData["BedCount"] = Enumerable.Range(1, 3)
                .Select(n => new SelectListItem
                {
                    Value = n.ToString(),
                    Text = n.ToString()
                }).ToList();

            if (RoomInput.CheckIn != DateTime.MinValue && RoomInput.CheckOut != DateTime.MinValue)
            {
                // prepare the parameters to be inserted into the query
                var bedCount = new SqliteParameter("bCount", RoomInput.TheRoom.BedCount);
                var checkIn = new SqliteParameter("checkIn", RoomInput.CheckIn);
                var checkOut = new SqliteParameter("checkOut", RoomInput.CheckOut);

                var checkRoom = _context.Room.FromSqlRaw("select [Room].* from [Room] Where [Room].BedCount = @bCount AND "
                                 + "[Room].ID not in (select [Room].ID from [Room] inner join [Booking] on "
                                 + "[Room].ID = [Booking].RoomID where [Booking].CheckIn <= @checkOut AND "
                                 + "@checkIn <= [Booking].CheckOut)", bedCount, checkOut, checkIn);

                CheckRoom = await checkRoom.ToListAsync();
            }
            else
            {
                ViewData["DateValidation"] = "fail";
            }

            // invoke the content file
            return Page();
        }
    }
}
