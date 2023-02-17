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

        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult Login()
        {
            return View("Login");
        }

        public ActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Users _user)
        {
            var status = _context.Users.Where(m => m.Login == _user.Login && m.Password == _user.Password && _user.isDeleted == false).FirstOrDefault();
            if(status!=null)
            {
                if(!status.isDeleted)
                {
                    if (status.isAdmin)
                    {
                        return RedirectToAction("ATicketPanel", "Tickets", new { userID = status.ID, logged = true, isAdmin = true } );
                    }
                    return RedirectToAction("TicketPanel", "Tickets", new { userID = status.ID, logged = true, isAdmin = true });
                }else
                {
                    return View("Login");
                }
            }
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Users _newUser)
        {
            if(ModelState.IsValid)
            {
                if ((_newUser.Login != null) &&
                    (_newUser.Email != null) &&
                    (_newUser.Password != null))
                {
                    var loginExits = _context.Users.Where(m => m.Login == _newUser.Login && m.isDeleted != true).FirstOrDefault();
                    if (loginExits == null)
                    {
                        var emailExits = _context.Users.Where(m => m.Email == _newUser.Email && m.isDeleted != true).FirstOrDefault();
                        if (emailExits == null)
                        {
                            var _Users = new Users
                            {
                                Login = _newUser.Login,
                                Email = _newUser.Email,
                                Password = _newUser.Password,
                                isAdmin = false,
                                isDeleted = false
                            };

                            _context.Users.Add(_Users);
                            _context.SaveChanges();

                            ViewData["RegisterError"] = "";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewData["RegisterError"] = "The selected email is already taken";
                            return View("Register");
                        }
                    }
                    else
                    {
                        ViewData["RegisterError"] = "The selected login is already taken";
                        return View("Register");
                    }
                }
                else
                {
                    ViewData["RegisterError"] = "You must fill all inputs";
                    return View("Register");
                }
            }
            else
            {
                ViewData["RegisterError"] = "Form invalid!";
                return View("Register");
            }
        }

        public ActionResult AManageUsers()
        {
            List<Users> usersList = new List<Users>();
            usersList = _context.Users.Where(u => u.isDeleted != true).ToList();
            return View("AManageUsers", usersList);
        }

        [Route("/Users/MakeAdmin/{id}")]
        public ActionResult MakeAdmin(int id)
        {
            var user = _context.Users.Where(m => m.ID == id).FirstOrDefault();
            user.isAdmin = true;
            _context.SaveChanges();
            try
            {
                int userID = int.Parse(TempData["currentUserID"].ToString());
                return RedirectToAction("ATicketPanel", "Tickets", new { userID = userID });
            }
            catch
            {
                return RedirectToAction("ATicketPanel", "Tickets", new { userID = 1 });
            }
        }

        [Route("/Users/DeleteUser/{id}")]
        public ActionResult DeleteUser(int id)
        {
            var user = _context.Users.Where(m => m.ID == id).FirstOrDefault();
            var ticketsToDelete = _context.Tickets.Where(m => m.ticketOwner == user).ToList();
            user.isDeleted = true;
            foreach(Tickets t in ticketsToDelete)
            {
                t.isDeleted = true;
            }
            _context.SaveChanges();
            try
            {
                int userID = int.Parse(TempData["currentUserID"].ToString());
                return RedirectToAction("ATicketPanel", "Tickets", new { userID = userID });
            }
            catch
            {
                return RedirectToAction("ATicketPanel", "Tickets", new { userID = 1 });
            }
        }
    }
}
