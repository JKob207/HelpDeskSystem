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

        public TicketsController(AppDbContext context)
        {
            _context = context;
        }

        public ActionResult TicketPanel(int? userID, bool? logged, bool? isAdmin)
        {
            if((userID == null || logged == null || logged == false) && isAdmin == false)
            {
                return RedirectToAction("Index", "Users");
            }

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
            try
            {
                Users currentUser = _context.Users.Where(m => m.ID == userID).FirstOrDefault();
                TempData["currentUserID"] = userID;
                TempData.Keep("currentUserID");
                var ticketAssigned = ticketList.Where(m => m.responsibleUser == currentUser.Login);
                return View("TicketPanel", ticketAssigned);
            }
            catch
            {
                Users currentUser = _context.Users.Where(m => m.ID == 1).FirstOrDefault();
                var ticketAssigned = ticketList.Where(m => m.responsibleUser == currentUser.Login);
                return View("TicketPanel", ticketAssigned);
            }
        }

        public ActionResult ATicketPanel(int? userID, bool? logged, bool? isAdmin)
        {
            if ((userID == null || logged == null || logged == false) && isAdmin == true)
            {
                return RedirectToAction("Index", "Users");
            }

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

            try
            {
                Users currentUser = _context.Users.Where(m => m.ID == userID).FirstOrDefault();
                TempData["currentUserID"] = userID;
                TempData.Keep("currentUserID");
                var ticketAssigned = ticketList.Where(m => m.responsibleUser == currentUser.Login);
                return View("ATicketPanel", ticketAssigned);
            }
            catch
            {
                Users currentUser = _context.Users.Where(m => m.ID == 1).FirstOrDefault();
                var ticketAssigned = ticketList.Where(m => m.responsibleUser == currentUser.Login);
                return View("ATicketPanel", ticketAssigned);
            }
        }


        public IActionResult BackToTicketPanel()
        {
            try
            {
                int userID = int.Parse(TempData["currentUserID"].ToString());
                Users currentUser = _context.Users.Where(m => m.ID == userID).FirstOrDefault();
                if (currentUser.isAdmin)
                {
                    return RedirectToAction("ATicketPanel", new { userID = TempData["currentUserID"], logged = true, isAdmin = true });
                }
                return RedirectToAction("TicketPanel", new { userID = TempData["currentUserID"], logged = true, isAdmin = false });
            }
            catch
            {
                Users currentUser = _context.Users.Where(m => m.ID == 1).FirstOrDefault();
                return RedirectToAction("TicketPanel", new { userID = 1, logged = true, isAdmin = false });
            }
        }

        public ActionResult AddTicket()
        {
            try
            {
                int userID = int.Parse(TempData["currentUserID"].ToString());
                TempData.Keep("currentUserID");

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

                return View("AddTicket");
            }
            catch
            {
                return RedirectToAction("Index", "Users");
            }
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

            try
            {
                Users currentUser = _context.Users.Where(m => m.ID == int.Parse(TempData["currentUserID"].ToString())).FirstOrDefault();
                TempData.Keep("currentUserID");
                var ticketsCreated = ticketList.Where(m => m.ticketOwner == currentUser.Login);

                return View(ticketsCreated);
            } catch
            {
                return RedirectToAction("Index", "Users");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTicket(Tickets _newTicket)
        {
            TempData.Keep("currentUserID");
            int ticketCategoryID = int.Parse(Request.Form["ticketCategory"]);
            int userResponsibleID = int.Parse(Request.Form["responsibleUser"]);
            int currentUserID = int.Parse(TempData["currentUserID"].ToString());
            TempData.Keep("currentUserID");
            if(string.IsNullOrEmpty(_newTicket.Title) || string.IsNullOrEmpty(_newTicket.Priority.ToString()))
            {
                ViewData["AddTicketError"] = "Please fill all!";
                return View();
            }
            var _Tickets = new Tickets
            {
                Title = _newTicket.Title,
                Descriptions = _newTicket.Descriptions,
                Priority = _newTicket.Priority,
                createdAt = DateTime.Now,
                isDeleted = false,
                responsibleUser = _context.Users.Where(m => m.ID == userResponsibleID).FirstOrDefault(),
                ticketOwner = _context.Users.Where(m => m.ID == currentUserID).FirstOrDefault(),
                ticketStatus = _context.TicketStatuses.Where(m => m.Name == "New").FirstOrDefault(),
                ticketCategory = _context.TicketCategories.Where(m => m.ID == ticketCategoryID).FirstOrDefault()
            };

            _context.Tickets.Add(_Tickets);
            _context.SaveChanges();

            try
            {
                int userID = int.Parse(TempData["currentUserID"].ToString());
                Users currentUser = _context.Users.Where(m => m.ID == userID).FirstOrDefault();

                if (currentUser.isAdmin)
                {
                    return RedirectToAction("ATicketPanel", new { userID = TempData["currentUserID"], logged = true, isAdmin = true });
                }
                return RedirectToAction("TicketPanel", new { userID = TempData["currentUserID"], logged = true, isAdmin = false });
            }
            catch
            {
                return RedirectToAction("Index", "Users");
            }
        }

        [Route("/Tickets/DeleteTicket/{id}")]
        public IActionResult DeleteTicket(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Users");
            }

            var ticket = _context.Tickets.Where(m => m.ID == id).FirstOrDefault();
            ticket.isDeleted = true;
            _context.SaveChanges();

            try
            {
                int userID = int.Parse(TempData["currentUserID"].ToString());
                Users currentUser = _context.Users.Where(m => m.ID == userID).FirstOrDefault();
                if (currentUser.isAdmin)
                {
                    return RedirectToAction("ATicketPanel", new { userID = TempData["currentUserID"], logged = true, isAdmin = true });
                }

                return RedirectToAction("TicketPanel", new { userID = TempData["currentUserID"], logged = true, isAdmin = false });
            }
            catch
            {
                return RedirectToAction("Index", "Users");
            }
        }

        [Route("/Tickets/EditTicket/{id}")]
        public ActionResult EditTicket(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Users");
            }

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
                          .Where(m => m.isDeleted != true)
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
            if (_newTicketView == null)
            {
                return RedirectToAction("Index", "Users");
            }

            var ticket = _context.Tickets.Where(m => m.ID == _newTicketView.TicketID).FirstOrDefault();

            if (string.IsNullOrEmpty(_newTicketView.TickietTitle) || string.IsNullOrEmpty(_newTicketView.responsibleUser.ToString()) || string.IsNullOrEmpty(_newTicketView.ticketStatus.ToString()) || string.IsNullOrEmpty(_newTicketView.ticketCategory.ToString()) )
            {
                ViewData["EditTicketError"] = "Please fill all!";
                return View();
            }

            ticket.Title = _newTicketView.TickietTitle;
            ticket.Descriptions = _newTicketView.TicketDescriptions;
            ticket.Priority = int.Parse(Request.Form["priority"]);
            ticket.responsibleUser = _context.Users.Where(m => m.ID == int.Parse(_newTicketView.responsibleUser)).FirstOrDefault();
            ticket.ticketStatus = _context.TicketStatuses.Where(m => m.ID == int.Parse(_newTicketView.ticketStatus)).FirstOrDefault();
            ticket.ticketCategory = _context.TicketCategories.Where(m => m.ID == int.Parse(_newTicketView.ticketCategory)).FirstOrDefault();
            _context.SaveChanges();

            try
            {
                int userID = int.Parse(TempData["currentUserID"].ToString());
                Users currentUser = _context.Users.Where(m => m.ID == userID).FirstOrDefault();
                if (currentUser.isAdmin)
                {
                    return RedirectToAction("ATicketPanel", new { userID = TempData["currentUserID"], logged = true, isAdmin = true });
                }

                return RedirectToAction("TicketPanel", new { userID = TempData["currentUserID"], logged = true, isAdmin = false });
            } catch
            {
                return RedirectToAction("Index", "Users");
            }
        }
    }
}
