using HelpDeskApp.Controllers;
using HelpDeskApp.Data;
using HelpDeskApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HelpDeskTests
{
    [TestClass]
    public class UsersTests
    {
        [TestMethod]
        public void LoginTest()
        {
            var connection = new SqliteConnection("Data Source=HelpDesk.db");
            connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;

            using (var context = new AppDbContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new AppDbContext(options))
            {
                var user = new Users
                {
                    Login = "user",
                    Email = "test@test.pl",
                    Password = "zaq1@WSX",
                    isAdmin = false,
                    isDeleted = false
                };

                var controller = new UsersController(context);
                var result = controller.Login(user) as ViewResult;
                Assert.AreEqual("Login", result.ViewName);
            }

        }

        [TestMethod]
        public void RegisterTest()
        {
            var connection = new SqliteConnection("Data Source=HelpDesk.db");
            connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;

            using (var context = new AppDbContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new AppDbContext(options))
            {
                var user = new Users
                {
                    Login = "TEST_USER",
                    Email = "test_user@test.pl",
                    Password = "zaq1@WSX",
                    isAdmin = false,
                    isDeleted = false
                };

                var controller = new UsersController(context);
                var result = controller.Register(user) as ViewResult;
                Assert.AreEqual("Register", result.ViewName);
            }
        }

        [TestMethod]
        public void MakeAdminTest()
        {
            var connection = new SqliteConnection("Data Source=HelpDesk.db");
            connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;

            using (var context = new AppDbContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new AppDbContext(options))
            {
                int ID = 1;

                var controller = new UsersController(context);
                var result = (RedirectToActionResult)controller.MakeAdmin(ID);
                Assert.AreEqual("ATicketPanel", result.ActionName);
            }
        }

        [TestMethod]
        public void DeleteUserTest()
        {
            var connection = new SqliteConnection("Data Source=HelpDesk.db");
            connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;

            using (var context = new AppDbContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new AppDbContext(options))
            {
                int ID = 1;

                var controller = new UsersController(context);
                var result = (RedirectToActionResult)controller.DeleteUser(ID);
                Assert.AreEqual("ATicketPanel", result.ActionName);
            }
        }

        [TestMethod]
        public void AManageUsersTest()
        {
            var connection = new SqliteConnection("Data Source=HelpDesk.db");
            connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;

            using (var context = new AppDbContext(options))
            {
                context.Database.EnsureCreated();
            }

            using (var context = new AppDbContext(options))
            {
                var controller = new UsersController(context);
                var result = controller.AManageUsers() as ViewResult;
                Assert.AreEqual("AManageUsers", result.ViewName);
            }
        }
    }
}
