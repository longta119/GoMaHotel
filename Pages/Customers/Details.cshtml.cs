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
using System.Security.Claims;

namespace HotelOne20251619.Pages.Customers
{
    [Authorize(Roles = "Customers")]
    public class DetailsModel : PageModel
    {
        private readonly HotelOne20251619.Data.ApplicationDbContext _context;

        public DetailsModel(HotelOne20251619.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CustomerViewModel Myself { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            // retrieve the logged-in user's email
            // need to add "using System.Security.Claims;"
            string _email = User.FindFirst(ClaimTypes.Name).Value;

            Customer customer = await _context.Customer.FirstOrDefaultAsync(m => m.Email == _email);

            if (customer != null)
            {
                // The user has been created in the database
                ViewData["ExistInDB"] = "true";
                Myself = new CustomerViewModel
                {
                    // Retrieve his/her details for display in the web form
                    Surname = customer.Surname,
                    GivenName = customer.GivenName,
                    PostCode = customer.PostCode,
                };
            }
            else
            {
                ViewData["ExistInDB"] = "false";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // retrieve the logged-in user's email
            // need to add "using System.Security.Claims;"
            string _email = User.FindFirst(ClaimTypes.Name).Value;

            Customer customer = await _context.Customer.FirstOrDefaultAsync(m => m.Email == _email);

            if (customer != null)
            {
                // This ViewData entry is needed in the content file
                // The user has been created in the database
                ViewData["ExistInDB"] = "true";
            }
            else
            {
                ViewData["ExistInDB"] = "false";
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (customer == null)
            {
                // creating a customer object for inserting database
                customer = new Customer();
            }

            // Construct this customer object based on 'Myself'
            customer.Email = _email;
            customer.Surname = Myself.Surname;
            customer.GivenName = Myself.GivenName;
            customer.PostCode = Myself.PostCode;

            if ((string)ViewData["ExistInDB"] == "true")
            {
                _context.Attach(customer).State = EntityState.Modified;
            }
            else
            {
                _context.Customer.Add(customer);
            }

            try  // catching the conflict of editing this record concurrently
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            ViewData["SuccessDB"] = "success";
            return Page();
        }
    }
}
