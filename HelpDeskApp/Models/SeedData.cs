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
                var admin = new Users { Login = "admin", Email = "admin@test.pl", Password = "pass", isAdmin = true, isDeleted = false };

                context.Users.AddRange(user, admin);
                context.SaveChanges();
            }
        }
    }
}
