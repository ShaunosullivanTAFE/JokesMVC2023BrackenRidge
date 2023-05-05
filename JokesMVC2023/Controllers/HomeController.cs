using JokesMVC2023.Models;
using JokesMVC2023.Models.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JokesMVC2023.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly JokeDBContext _context;

        public HomeController(ILogger<HomeController> logger, JokeDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
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




        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginUserDTO loginDetails)
        {
            var user = _context.Users.Where(c => c.Email.Equals(loginDetails.Email)).FirstOrDefault();

            if(user == null)
            {
                return View();
            }

            if (BCrypt.Net.BCrypt.EnhancedVerify(loginDetails.Password, user.PasswordHash))
            {
                HttpContext.Session.SetString("ID", user.Id.ToString());
                HttpContext.Session.SetString("Email", user.Email);

                return RedirectToAction("Index");
            }

            return View();

        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterUserDTO registerDetails)
        {
            try
            {
                AppUser newUser = new AppUser()
                {
                    Email = registerDetails.Email,
                    PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(registerDetails.Password)
                };
                _context.Users.Add(newUser);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }
            catch (Exception e)
            {
                return View();
            }
            
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}