using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using HotelOne20251619.Models;

namespace HotelOne20251619.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<HotelOne20251619.Models.Customer> Customer { get; set; }
        public DbSet<HotelOne20251619.Models.Room> Room { get; set; }
        public DbSet<HotelOne20251619.Models.Booking> Booking { get; set; }
    }
}
