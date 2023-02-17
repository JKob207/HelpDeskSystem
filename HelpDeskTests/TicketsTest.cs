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
    public class TicketTest
    {
        [TestMethod]
        public void TicketPanelTest()
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
                var controller = new TicketsController(context);
                var result = controller.TicketPanel(1, true, false) as ViewResult;
                Assert.AreEqual("TicketPanel", result.ViewName);
            }
        }

        [TestMethod]
        public void ATicketPanelTest()
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
                var controller = new TicketsController(context);
                var result = controller.ATicketPanel(1, true, true) as ViewResult;
                Assert.AreEqual("ATicketPanel", result.ViewName);
            }
        }

        [TestMethod]
        public void BackToTicketPanelTest()
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
                var controller = new TicketsController(context);
                var result = (RedirectToActionResult)controller.BackToTicketPanel();
                Assert.AreEqual("TicketPanel", result.ActionName);
            }
        }

        [TestMethod]
        public void EditTicketTest()
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
                int id = 1;
                var controller = new TicketsController(context);
                var result = controller.EditTicket(id) as ViewResult;
                Assert.AreEqual("EditTicket", result.ViewName);
            }
        }
    }
}
