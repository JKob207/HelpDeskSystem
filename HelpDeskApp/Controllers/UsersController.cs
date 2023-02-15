using HelpDeskApp.Data;
using HelpDeskApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDeskApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Users _user)
        {
            var status = _context.Users.Where(m => m.Login == _user.Login && m.Password == _user.Password).FirstOrDefault();
            if(status!=null)
            {
                if(!status.isDeleted)
                {
                    if (status.isAdmin)
                    {
                        return RedirectToAction("ATicketPanel", "Tickets", new { userID = status.ID } );
                    }
                    return RedirectToAction("TicketPanel", "Tickets", new { userID = status.ID });
                }else
                {
                    return View();
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Users _newUser)
        {
            if(ModelState.IsValid)
            {
                var _Users = new Users
                {
                    Login = _newUser.Login,
                    Email = _newUser.Email,
                    Password = _newUser.Password,
                    isAdmin = false,
                    isDeleted = false
                };

                if ((_newUser.Login != null) &&
                    (_newUser.Email != null) &&
                    (_newUser.Password != null))
                {
                    _context.Users.Add(_Users);
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }else
            {
                return View();
            }
        }

        public IActionResult AManageUsers()
        {
            List<Users> usersList = new List<Users>();
            usersList = _context.Users.Where(u => u.isDeleted != true).ToList();
            return View(usersList);
        }

        [Route("/Users/MakeAdmin/{id}")]
        public IActionResult MakeAdmin(int id)
        {
            var user = _context.Users.Where(m => m.ID == id).FirstOrDefault();
            user.isAdmin = true;
            _context.SaveChanges();
            int userID = int.Parse(TempData["currentUserID"].ToString());
            return RedirectToAction("ATicketPanel", "Tickets", new { userID = userID });
        }

        [Route("/Users/DeleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Where(m => m.ID == id).FirstOrDefault();
            var ticketsToDelete = _context.Tickets.Where(m => m.ticketOwner == user).ToList();
            user.isDeleted = true;
            foreach(Tickets t in ticketsToDelete)
            {
                t.isDeleted = true;
            }
            _context.SaveChanges();
            int userID = int.Parse(TempData["currentUserID"].ToString());
            return RedirectToAction("ATicketPanel", "Tickets", new { userID = userID });
        }
    }
}
