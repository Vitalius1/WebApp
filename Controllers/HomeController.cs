using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WebApp.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private WebappContext _context;

        public HomeController(WebappContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult RegisterPage()
        {
            return View("Register");
        }


        [HttpGet]
        [Route("login")]
        public IActionResult LoginPage()
        {
            ViewBag.NiceTry = TempData["NiceTry"];
            return View("Login");
        }
        
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LoginPage");
        }

//================================== Register and Login automatically =========================================
        [HttpPost]
        [Route("register")]
        public IActionResult Create(RegisterViewModel NewUser)
        {
            if (ModelState.IsValid)
            {
                // Check if Email is alredy regitered in DataBase
                List<User> CheckUsername = _context.Users.Where(theuser => theuser.Username == NewUser.Username).ToList();
                if (CheckUsername.Count > 0)
                {
                    ViewBag.ErrorRegister = "Username already in use...";
                    return View("Register");
                }
                // Password hashing
                PasswordHasher<RegisterViewModel> Hasher = new PasswordHasher<RegisterViewModel>();
                NewUser.Password = Hasher.HashPassword(NewUser, NewUser.Password);
                // Adding the created User Object to the DB
                User user = new User
                {
                    FirstName = NewUser.FirstName,
                    LastName = NewUser.LastName,
                    Username = NewUser.Username,
                    Wallet = 1000.00,
                    Password = NewUser.Password,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _context.Users.Add(user);
                _context.SaveChanges();
                // Extracting the JustCreated user in order to obtain his ID and full name for storing in session
                User JustCreated = _context.Users.Single(theUser => theUser.Username == NewUser.Username);
                HttpContext.Session.SetInt32("UserId", (int)JustCreated.UserId);
                HttpContext.Session.SetString("UserName", (string)JustCreated.FirstName);
                return RedirectToAction("Index", "Second");
            }
            return View("Register");
        }

//============================================= Login a User ==================================================
        [HttpPost]
        [Route("loginNow")]
        public IActionResult Login(string LUsername = null, string Password = null)
        {
            // Checking if user inputs anything in the fields
            if(Password != null && LUsername != null)
            {
                // Checking if a User this provided Email exists in DB
                List<User> CheckUser = _context.Users.Where(theuser => theuser.Username == LUsername).ToList();
                if (CheckUser.Count > 0)
                {
                    // Checking if the password matches
                    var Hasher  = new PasswordHasher<User>();
                    if(0 != Hasher.VerifyHashedPassword(CheckUser[0], CheckUser[0].Password, Password))
                    {
                        // If the checks are validated than save his ID and Name in session and redirect to the Dashboard page
                        HttpContext.Session.SetInt32("UserId", (int)CheckUser[0].UserId);
                        HttpContext.Session.SetString("UserName", (string)CheckUser[0].FirstName);
                        return RedirectToAction("Index", "Second");
                    }
                }
            }
            // If check are not validated display an error
            ViewBag.ErrorLogin = "Invalid Login Data...";
            return View("Login");
        }        
        
// ============================================================================================================

    }
}
