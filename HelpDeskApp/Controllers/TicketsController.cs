using HelpDeskApp.Data;
using HelpDeskApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDeskApp.Controllers
{
    public class TicketsController : Controller
    {
        private readonly AppDbContext _context;
        private int _currentUserID;

        public TicketsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult TicketPanel(int userID)
        {
            List<TicketView> ticketList = new List<TicketView>();
            ticketList = _context.Tickets
                .Select(t => new TicketView
                {
                    TicketID = t.ID,
                    TickietTitle = t.Title,
                    TicketDescriptions = t.Descriptions,
                    TicketPriority = t.Priority,
                    TicketCreatedAt = t.createdAt,
                    responsibleUser = t.responsibleUser.Login,
                    ticketOwner = t.ticketOwner.Login,
                    ticketStatus = t.ticketStatus.Name,
                    ticketCategory = t.ticketCategory.Name
                }).ToList();
            Users currentUser = _context.Users.Where(m => m.ID == userID).FirstOrDefault();
            TempData["currentUserID"] = userID;
            TempData.Keep("currentUserID");
            var ticketAssigned = ticketList.Where(m => m.responsibleUser == currentUser.Login);
            return View(ticketAssigned);
        }

        public IActionResult AddTicket()
        {
            ViewData["Numbers"] = Enumerable.Range(1, 5)
            .Select(n => new SelectListItem
            {
                Value = n.ToString(),
                Text = n.ToString()
            }).ToList();

            ViewData["Categories"] = new List<SelectListItem>
            {
                new SelectListItem {Text = "Network problem", Value = "1"},
                new SelectListItem {Text = "Device problem", Value = "2"},
                new SelectListItem {Text = "Software problem", Value = "3"},
                new SelectListItem {Text = "Other", Value = "4"}
            };

            ViewData["Users"] = _context.Users
                          .Select(a => new SelectListItem()
                          {
                              Value = a.ID.ToString(),
                              Text = a.Login
                          })
                          .ToList();

            return View();
        }

        public IActionResult ShowCreatedTickets()
        {
            List<TicketView> ticketList = new List<TicketView>();
            ticketList = _context.Tickets
                .Select(t => new TicketView
                {
                    TicketID = t.ID,
                    TickietTitle = t.Title,
                    TicketDescriptions = t.Descriptions,
                    TicketPriority = t.Priority,
                    TicketCreatedAt = t.createdAt,
                    responsibleUser = t.responsibleUser.Login,
                    ticketOwner = t.ticketOwner.Login,
                    ticketStatus = t.ticketStatus.Name,
                    ticketCategory = t.ticketCategory.Name
                }).ToList();

            Users currentUser = _context.Users.Where(m => m.ID == int.Parse(TempData["currentUserID"].ToString())).FirstOrDefault();
            TempData.Keep("currentUserID");
            var ticketsCreated = ticketList.Where(m => m.ticketOwner == currentUser.Login);

            return View(ticketsCreated);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTicket(Tickets _newTicket)
        {
            TempData.Keep("currentUserID");
            int ticketCategoryID = int.Parse(Request.Form["ticketCategory"]);
            int userResponsibleID = int.Parse(Request.Form["responsibleUser"]);
            var _Tickets = new Tickets
            {
                Title = _newTicket.Title,
                Descriptions = _newTicket.Descriptions,
                Priority = _newTicket.Priority,
                createdAt = DateTime.Now,
                responsibleUser = _context.Users.Where(m => m.ID == userResponsibleID).FirstOrDefault(),
                ticketOwner = _context.Users.Where(m => m.ID == int.Parse(TempData["currentUserID"].ToString())).FirstOrDefault(),
                ticketStatus = _context.TicketStatuses.Where(m => m.Name == "New").FirstOrDefault(),
                ticketCategory = _context.TicketCategories.Where(m => m.ID == ticketCategoryID).FirstOrDefault()
            };

            _context.Tickets.Add(_Tickets);
            _context.SaveChanges();
            return RedirectToAction("TicketPanel", new { userID = TempData["currentUserID"] });
        }
    }
}
