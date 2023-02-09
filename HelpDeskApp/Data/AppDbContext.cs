using HelpDeskApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDeskApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Models.Users> Users { get; set; } = default!;

        public DbSet<Models.Tickets> Tickets { get; set; } = default!;

        public DbSet<Models.TicketCategories> TicketCategories { get; set; } = default!;

        public DbSet<Models.TicketStatuses> TicketStatuses { get; set; } = default!;
    }
}
