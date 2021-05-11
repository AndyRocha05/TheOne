using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using TheOne.Models;

namespace TheOne.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;
        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            _context = context;
        }//(((((((((((((((((((((((((((((((((((((((((((((Index Home Page )))))))))))))))))))))))))))))))))))))))))))))//
        public User GetCurrentUser()
        {
            int? id = HttpContext.Session.GetInt32("id");
            if (id == null)
            {
                return null;
            }
            return _context.Users.First(u => u.UserId == id);
        }
        //(((((((((((((((((((((((((((((((((((((((((((((Index Home Page )))))))))))))))))))))))))))))))))))))))))))))//
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
        //(((((((((((((((((((((((((((((((((((((((((((((DashBoard )))))))))))))))))))))))))))))))))))))))))))))//
        [HttpGet("Dashboard")]
        public ActionResult Dashboard()
        {
            // %%%%%%%%%Get the user info%%%%%%%%%%%%%%
            int? userid = HttpContext.Session.GetInt32("id");
            if (userid == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.CurrentUser = _context.Users.First(u => u.UserId == userid);
            ViewBag.AllEvents = _context
           .Events
           .Include(u => u.PostedBy)
           .Include(u => u.Guests)
           .OrderByDescending(b => b.CreatedAt);
            return View();
        }
        //(((((((((((((((((((((((((((((((((((((((((((((Create a User )))))))))))))))))))))))))))))))))))))))))))))//
        [HttpPost("New/User")]
        public IActionResult NewUser(User newUser)
        {

            // %%%%%%%%%%%%%%Checks the email for duplicates
            if (_context.Users.Any(u => u.Email == newUser.Email))
            {
                // %%%%%  error message%%%%%%%%%%%%%%%%%%%%%%%%%%
                ModelState.AddModelError("Email", "Email already in use!");
                return View("Index");
                // You may consider returning to the View at this point
            }
            //%%%%%%%%%%%%%%%%%%%%Check For Validation%%%%%%%%%%%%%%%%%%%%%%%%%%
            if (ModelState.IsValid)
            {
                // %%%%%%%%%%%%%%%%%%%%%%%%%Hashes the password%%%%%%%%%%%%%%%%%%%%%%%%% 
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                //Save your user object to the database
                // %%%%%%%%%%%%% Adds the user to Database%%%%%%%%%%%%%%%%%
                _context.Add(newUser);

                _context.SaveChanges();
                // %%%%%%%%%%%%%%%Save user ID To Session%%%%%%%%%%%%%%%%%%%%%%%%%%%
                // %%%%%%%%%%%%%%%% declares a key is in green for the userid%%%%%%%
                HttpContext.Session.SetInt32("id", newUser.UserId);

                // %%%%%%%%%%%Redirect to a Dashboard%%%%%%%%%%%%%%%%%%%%%%%%
                return RedirectToAction("Dashboard");
            }
            // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%Validations must be Triggered will return View%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
            return View("Index");
        }
        //(((((((((((((((((((((((((((((((((((((((((((((Login )))))))))))))))))))))))))))))))))))))))))))))//
        [HttpPost("Login")]
        public IActionResult Login(LoginUser userToLogin)
        {
            // %%%%%%%%% Look into DashBoard and make sure Email is in DB 
            var foundUser = _context.Users.FirstOrDefault(u => u.Email == userToLogin.LoginEmail);
            if (foundUser == null)
            {
                ModelState.AddModelError("LoginEmail", "Please check your email and password");
                return View("Index");
            }
            // %%%%%%%%%%%%%%% Makes sure Password match in database
            var hasher = new PasswordHasher<LoginUser>();

            var result = hasher.VerifyHashedPassword(userToLogin, foundUser.Password, userToLogin.LoginPassword);
            if (result == 0)
            {
                ModelState.AddModelError("LoginEmail", "Please check your email and password");
                return View("Index");
            }
            // Set the key in session 
            HttpContext.Session.SetInt32("id", foundUser.UserId);
            return RedirectToAction("Dashboard");

        }

        //(((((((((((((((((((((((((((((((((((((((((((((Event form page)))))))))))))))))))))))))))))))))))))))))))))//
        [HttpGet("Event")]
        public IActionResult Event()
        {
            // %%%%%%%%%Get the user info%%%%%%%%%%%%%%
            int? userid = HttpContext.Session.GetInt32("id");
            if (userid == null)
            {
                return RedirectToAction("Index");
            }
            // pass the user in ViewBag 
            ViewBag.CurrentUser = _context.Users.First(u => u.UserId == userid);

            return View();
        }

        //(((((((((((((((((((((((((((((((((((((((((((((Create a event)))))))))))))))))))))))))))))))))))))))))))))//
        [HttpPost("Event/New")]
        public IActionResult NewEvent(Event NewEvent)
        {
            int? userid = HttpContext.Session.GetInt32("id");
            if (userid == null)
            {
                return RedirectToAction("Index");
            }
            // pass the user in ViewBag 
            ViewBag.CurrentUser = _context.Users.First(u => u.UserId == userid);
            // %%%%%%% Checks the date is in Future%%%%%%%%
            if (NewEvent.Date <= DateTime.Now)
                ModelState.AddModelError("Date", "Please insure date is in future");
            if (ModelState.IsValid)
            {
                NewEvent.UserId = (int)HttpContext.Session.GetInt32("id");
                _context.Add(NewEvent);
                _context.SaveChanges();
                return Redirect($"/Event/{NewEvent.EventId}");
            }
            return View("Event");
        }

        //(((((((((((((((((((((((((((((((((((((((((((((Display the information )))))))))))))))))))))))))))))))))))))))))))))//
        [HttpGet("Event/{EventId}")]

        public IActionResult EventInfo(int EventId)
        {
            int? userid = HttpContext.Session.GetInt32("id");
            if (userid == null)
            {
                return RedirectToAction("Index");
            }
            // pass the user in ViewBag 
            ViewBag.CurrentUser = _context.Users.First(u => u.UserId == userid);
            // send id to ViewBag
            ViewBag.Event = _context.Events
            .Include(m => m.PostedBy)
            .Include(g => g.Guests)
            .ThenInclude(b => b.UserToEvent)
            .First(w => w.EventId == EventId);

            return View();
        }
        //(((((((((((((((((((((((((((((((((((((((((((((Index Home Page )))))))))))))))))))))))))))))))))))))))))))))//
        [HttpPost("Guest/{EventId}/Delete")]
        public IActionResult Delete(int EventId)
        {
            var EventToDelete = _context
            .Events
            .First(w => w.EventId == EventId);
            _context.Remove(EventToDelete);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");

        }

        //(((((((((((((((((((((((((((((((((((((((((((((Index Home Page )))))))))))))))))))))))))))))))))))))))))))))//
        [HttpPost("Guest/{EventId}/Guest/Delete")]
        public IActionResult DeleteRsvp(int EventId)
        {
            // Get current user function
            var currentUser = GetCurrentUser();
            var RsvpDelete = _context
            .Players
            .First(Rsvp => Rsvp.EventId == EventId && Rsvp.UserId == currentUser.UserId);
            _context.Remove(RsvpDelete);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        //(((((((((((((((((((((((((((((((((((((((((((((Add A participant )))))))))))))))))))))))))))))))))))))))))))))//
        [HttpPost("Guest/{EventId}/Guest")]
        public IActionResult AddGuest(int EventId)
        {
            var RsvpToAdd = new Participant
            {
                UserId = GetCurrentUser().UserId,
                EventId = EventId
            };
            _context.Add(RsvpToAdd);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        //(((((((((((((((((((((((((((((((((((((((((((((LogOut )))))))))))))))))))))))))))))))))))))))))))))//
        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        //(((((((((((((((((((((((((((((((((((((((((((((Index Home Page )))))))))))))))))))))))))))))))))))))))))))))//
        //(((((((((((((((((((((((((((((((((((((((((((((Index Home Page )))))))))))))))))))))))))))))))))))))))))))))//
        //(((((((((((((((((((((((((((((((((((((((((((((Index Home Page )))))))))))))))))))))))))))))))))))))))))))))//
        //(((((((((((((((((((((((((((((((((((((((((((((Index Home Page )))))))))))))))))))))))))))))))))))))))))))))//
        //(((((((((((((((((((((((((((((((((((((((((((((Index Home Page )))))))))))))))))))))))))))))))))))))))))))))//
        //(((((((((((((((((((((((((((((((((((((((((((((Index Home Page )))))))))))))))))))))))))))))))))))))))))))))//
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
