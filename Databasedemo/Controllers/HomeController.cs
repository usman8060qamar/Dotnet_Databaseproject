using Databasedemo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Databasedemo.Data;
using Microsoft.CodeAnalysis.Scripting;

namespace Databasedemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; // Inject ApplicationDbContext

        // Constructor injection for both ILogger and ApplicationDbContext
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context; // Initialize _context with injected ApplicationDbContext
        }
        

        public IActionResult Index()
        {
            // Fetch the list of users from the database
            var users = _context.Users.ToList(); // This gets all Users from the Users table

            // Pass the users data to the Index view
            return View(users);

        }
        public IActionResult Create() { 
            return View();
        }
        [HttpPost]
        public IActionResult Create(Users u)
        {
            if (ModelState.IsValid ==true) 
            {
                _context.Users.Add(u);
                var a = _context.SaveChanges();
                
                if (a > 0)
                {
                    TempData["Message"] = "Data submitted successfully!";
                    return RedirectToAction("Index");
                   // ModelState.Clear();
                }
                else
                {
                    TempData["Message"] = "data not submit sucessfully";
                }


            }

                return View();
        }
        public IActionResult Edit(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound(); // Handle the case where the user is not found
            }
            return View(user);
        }


        [HttpPost]
        public IActionResult Edit(Users u)
        {
            if (ModelState.IsValid)
            {
                // Update the user in the database
                _context.Users.Update(u);
                var a = _context.SaveChanges();

                if (a > 0)
                {
                    TempData["Message"] = "User updated successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = "User update failed!";
                }
            }

            return View(u); // return the user object back to the view if update fails
        }
        public IActionResult Delete(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }





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
