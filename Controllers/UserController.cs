using System;
using System.Web;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
// Allows us to reference this class to instantiate instances of models:
using MessageBoard.Models;
// Necessary for using session:
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
// Necessary for password hashing:
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
// Necessary for connecting to our dbContext -> then to our database:
using MessageBoard.Data;

namespace MessageBoard.Controllers
{
    public class UserController : Controller
    {
        private MessageBoardContext _context;
        public UserController(MessageBoardContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(LogRegWrapper user)
        {
            // Check to see if form data passes validations:
            if (ModelState.IsValid)
            {
                // Check to make sure email address is unique:
                if (_context.User.Any(u => u.Email == user.Register.Email))
                {
                    ModelState.AddModelError("Email", "Email in use. Already Registered? Please Log In.");
                    return View("Registration");
                }
                // If email is unique, process to hash password and create user in database:
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Register.Password = Hasher.HashPassword(user.Register, user.Register.Password);
                _context.User.Add(user.Register);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("UserId", user.Register.UserId);
                return RedirectToAction("Success");
            }
            else
            {
                return View("Registration");
            }
        }
        [HttpPost]
        public IActionResult Login(LogRegWrapper user)
        {
            if (ModelState.IsValid)
            {
                User userInDb = _context.User.FirstOrDefault(u => u.Email == user.Login.Email);
                if (userInDb == null)
                {
                    ModelState.AddModelError("Login.Email", "Invalid email/password.");
                    return View("Registration");
                }
                PasswordHasher<LogUser> Hasher = new PasswordHasher<LogUser>();
                PasswordVerificationResult Result = Hasher.VerifyHashedPassword(user.Login, userInDb.Password, user.Login.Password);

                if (Result == 0)
                {
                    ModelState.AddModelError("Login.Email", "Invalid email/password.");
                    return View("Registration");
                }
                HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                return RedirectToAction("Success");
            }
            else 
            {
                return View("Registration");
            }
        }

        [HttpGet]
        public RedirectToActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Registration");
        }

        [HttpGet]
        public IActionResult Success()
        {
            // Note: C# provides language support for nullable types using a question mark as a suffix. For example, int?
            // Checking to see if a user is logged in session:
            int? LoggedId = HttpContext.Session.GetInt32("UserId");
            if(LoggedId == null)
            {
                return RedirectToAction("Registration");
            }
            User VBUser = _context.User.FirstOrDefault(u => u.UserId == (int)LoggedId);
            ViewBag.bananas = VBUser;

            MessageBoardWrapper MBWrap = new MessageBoardWrapper()
            {
                AllUsers = _context.User.ToList(),
                LoggedUser = _context.User.FirstOrDefault(u => u.UserId == (int)LoggedId),
            };

            return View("Success", MBWrap);
        }

    }
}
