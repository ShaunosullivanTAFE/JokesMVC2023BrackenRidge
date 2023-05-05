using JokesMVC2023.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JokesMVC2023.Controllers
{
    public class FavouriteController : Controller
    {
        private readonly JokeDBContext _context;
        public FavouriteController(JokeDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("ID")))
            {
                return RedirectToAction("Login", "Home");
            }

            ViewBag.UserId = HttpContext.Session.GetString("ID");

            return View();
        }

        public async Task<IActionResult> FavouriteListDDL()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("ID")))
            {
                return Unauthorized();
            }

            string userId = HttpContext.Session.GetString("ID");

            var user = _context.Users.Where(c => c.Id == int.Parse(userId))
                                    .Include(c => c.FavoriteLists).FirstOrDefault();

            if(user == null){
                return BadRequest();
            }

            var selectList = user.FavoriteLists.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

            if(selectList.Count == 0){
                selectList.Add(new SelectListItem{
                    Text="No Entries",
                    Value = "0"
                });
            }

            ViewBag.SelectList = selectList;

            return PartialView("_FavouriteListDDL");
        }
    }
}
