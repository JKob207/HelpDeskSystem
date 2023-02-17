using HelpDeskApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDeskApp.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
                {
                    if(context.Users.Any())
                    {
                        return;
                    };

                var user = new Users { Login = "user", Email = "test@test.pl", Password = "zaq1@WSX", isAdmin = false, isDeleted = false };
                var admin = new Users { Login = "admin", Email = "admin@test.pl", Password = "qwerty1234", isAdmin = true, isDeleted = false };
                var statusNew = new TicketStatuses { Name = "New" };
                var statusAccepted = new TicketStatuses { Name = "Accepted" };
                var statusRejected = new TicketStatuses { Name = "Rejected" };
                var statusRealization = new TicketStatuses { Name = "Realization" };
                var statusFinished = new TicketStatuses { Name = "Finished" };
                var categoryNetworkProblem = new TicketCategories { Name = "Network problem" };
                var categoryDeviceProblem = new TicketCategories { Name = "Device problem" };
                var categorySoftwareProblem = new TicketCategories { Name = "Software problem" };
                var categoryOtherProblem = new TicketCategories { Name = "Other" };


                context.Users.AddRange(user, admin);
                context.TicketStatuses.AddRange(statusNew, statusAccepted, statusRejected, statusRealization, statusFinished);
                context.TicketCategories.AddRange(categoryNetworkProblem, categoryDeviceProblem, categorySoftwareProblem, categoryOtherProblem);
                context.SaveChanges();
            }
        }
    }
}
