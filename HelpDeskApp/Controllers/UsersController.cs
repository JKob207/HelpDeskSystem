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
    }
}
