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
                    isDeleted = t.isDeleted,
                    responsibleUser = t.responsibleUser.Login,
                    ticketOwner = t.ticketOwner.Login,
                    ticketStatus = t.ticketStatus.Name,
                    ticketCategory = t.ticketCategory.Name
                })
                .Where(t => t.isDeleted != true)
                .ToList();
            Users currentUser = _context.Users.Where(m => m.ID == userID).FirstOrDefault();
            TempData["currentUserID"] = userID;
            TempData.Keep("currentUserID");
            var ticketAssigned = ticketList.Where(m => m.responsibleUser == currentUser.Login);
            return View(ticketAssigned);
        }

        public IActionResult ATicketPanel(int userID)
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
                    isDeleted = t.isDeleted,
                    responsibleUser = t.responsibleUser.Login,
                    ticketOwner = t.ticketOwner.Login,
                    ticketStatus = t.ticketStatus.Name,
                    ticketCategory = t.ticketCategory.Name
                })
                .Where(t => t.isDeleted != true)
                .ToList();
            Users currentUser = _context.Users.Where(m => m.ID == userID).FirstOrDefault();
            TempData["currentUserID"] = userID;
            TempData.Keep("currentUserID");
            var ticketAssigned = ticketList.Where(m => m.responsibleUser == currentUser.Login);
            return View(ticketAssigned);
        }


        public IActionResult BackToTicketPanel()
        {
            int userID = int.Parse(TempData["currentUserID"].ToString());
            Users currentUser = _context.Users.Where(m => m.ID == userID).FirstOrDefault();
            if(currentUser.isAdmin)
            {
                return RedirectToAction("ATicketPanel", new { userID = TempData["currentUserID"] });
            }
            return RedirectToAction("TicketPanel", new { userID = TempData["currentUserID"] });
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
                          .Where(m => m.isDeleted != true)
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
                    isDeleted = t.isDeleted,
                    responsibleUser = t.responsibleUser.Login,
                    ticketOwner = t.ticketOwner.Login,
                    ticketStatus = t.ticketStatus.Name,
                    ticketCategory = t.ticketCategory.Name
                })
                .Where(t => t.isDeleted != true)
                .ToList();

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
                isDeleted = false,
                responsibleUser = _context.Users.Where(m => m.ID == userResponsibleID).FirstOrDefault(),
                ticketOwner = _context.Users.Where(m => m.ID == int.Parse(TempData["currentUserID"].ToString())).FirstOrDefault(),
                ticketStatus = _context.TicketStatuses.Where(m => m.Name == "New").FirstOrDefault(),
                ticketCategory = _context.TicketCategories.Where(m => m.ID == ticketCategoryID).FirstOrDefault()
            };

            _context.Tickets.Add(_Tickets);
            _context.SaveChanges();

            int userID = int.Parse(TempData["currentUserID"].ToString());
            Users currentUser = _context.Users.Where(m => m.ID == userID).FirstOrDefault();
            if (currentUser.isAdmin)
            {
                return RedirectToAction("ATicketPanel", new { userID = TempData["currentUserID"] });
            }
            return RedirectToAction("TicketPanel", new { userID = TempData["currentUserID"] });
        }

        [Route("/Tickets/DeleteTicket/{id}")]
        public IActionResult DeleteTicket(int id)
        {
            var ticket = _context.Tickets.Where(m => m.ID == id).FirstOrDefault();
            ticket.isDeleted = true;
            _context.SaveChanges();

            int userID = int.Parse(TempData["currentUserID"].ToString());
            Users currentUser = _context.Users.Where(m => m.ID == userID).FirstOrDefault();
            if (currentUser.isAdmin)
            {
                return RedirectToAction("ATicketPanel", new { userID = TempData["currentUserID"] });
            }

            return RedirectToAction("TicketPanel", new { userID = TempData["currentUserID"] });
        }

        [Route("/Tickets/EditTicket/{id}")]
        public IActionResult EditTicket(int id)
        {
            TicketView ticket = _context.Tickets
                .Select(t => new TicketView
                {
                    TicketID = t.ID,
                    TickietTitle = t.Title,
                    TicketDescriptions = t.Descriptions,
                    TicketPriority = t.Priority,
                    TicketCreatedAt = t.createdAt,
                    isDeleted = t.isDeleted,
                    responsibleUser = t.responsibleUser.Login,
                    ticketOwner = t.ticketOwner.Login,
                    ticketStatus = t.ticketStatus.Name,
                    ticketCategory = t.ticketCategory.Name
                })
                .Where(t => t.isDeleted != true && t.TicketID == id)
                .FirstOrDefault();

            ViewData["Users"] = _context.Users
                          .Select(a => new SelectListItem()
                          {
                              Value = a.ID.ToString(),
                              Text = a.Login
                          })
                          .ToList();
            ViewData["Statuses"] = _context.TicketStatuses
                          .Select(a => new SelectListItem()
                          {
                              Value = a.ID.ToString(),
                              Text = a.Name
                          })
                          .ToList();
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


            return View("EditTicket", ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTicketAction(TicketView _newTicketView)
        {
            var ticket = _context.Tickets.Where(m => m.ID == _newTicketView.TicketID).FirstOrDefault();
            ticket.Title = _newTicketView.TickietTitle;
            ticket.Descriptions = _newTicketView.TicketDescriptions;
            ticket.Priority = int.Parse(Request.Form["priority"]);
            ticket.responsibleUser = _context.Users.Where(m => m.ID == int.Parse(_newTicketView.responsibleUser)).FirstOrDefault();
            ticket.ticketStatus = _context.TicketStatuses.Where(m => m.ID == int.Parse(_newTicketView.ticketStatus)).FirstOrDefault();
            ticket.ticketCategory = _context.TicketCategories.Where(m => m.ID == int.Parse(_newTicketView.ticketCategory)).FirstOrDefault();
            _context.SaveChanges();

            int userID = int.Parse(TempData["currentUserID"].ToString());
            Users currentUser = _context.Users.Where(m => m.ID == userID).FirstOrDefault();
            if (currentUser.isAdmin)
            {
                return RedirectToAction("ATicketPanel", new { userID = TempData["currentUserID"] });
            }

            return RedirectToAction("TicketPanel", new { userID = TempData["currentUserID"] });
        }
    }
}
