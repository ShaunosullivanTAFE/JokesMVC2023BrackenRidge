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

        // Add New List 
        [HttpPost]
        public async Task<IActionResult> AddNewList([FromBody] string listName)
        {

            // retrieve ID from session
            string id = HttpContext?.Session?.GetString("ID");

            if (!int.TryParse(id, out int userID))
            {
                return Unauthorized();
            }

            if (_context.FavouriteLists.Any(c => c.Name == listName && c.UserId == userID))
            {
                return BadRequest();
            }

            FavouriteList newList = new FavouriteList()
            {
                Name = listName,
                UserId = userID
            };
            _context.FavouriteLists.Add(newList);
            _context.SaveChanges();

            return Ok();
        }


        // Get Jokes For List
        public async Task<IActionResult> GetJokesForList([FromQuery] int listID)
        {


            List<Joke> jokes = _context.FavouriteListItems.Include(c => c.Joke)
                                                                 .Where(c => c.FavouriteListId == listID)
                                                                 .Select(c => c.Joke)
                                                                 .ToList();



            return PartialView("_JokesForListPartial", jokes);
        }

        public async Task<IActionResult> JokesForEdit(string? query = null)
        {
            if (!String.IsNullOrEmpty(query))
            {
                return PartialView("_JokesForEditListPartial", await _context.Jokes.Where(c => c.JokeQuestion.Contains(query) || c.JokeAnswer.Contains(query)).ToListAsync());

            }

            return PartialView("_JokesForEditListPartial", await _context.Jokes.ToListAsync());
        }

        // Remove Joke From List
        [HttpDelete]
        public async Task<IActionResult> RemoveJokeFromList([FromBody] FavouriteListItem item)
        {
            var favListItem = _context.FavouriteListItems.Where(c => c.FavouriteListId == item.FavouriteListId && c.JokeId == item.JokeId)
                                                         .FirstOrDefault();

            if (favListItem != null)
            {
                _context.FavouriteListItems.Remove(favListItem);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }

        // Add Joke To List
        [HttpPost]
        public async Task<IActionResult> AddJokeToList([FromBody] FavouriteListItem item)
        {
            if (_context.FavouriteListItems.Any(c => c.FavouriteListId == item.FavouriteListId && c.JokeId == item.JokeId))
            {
                return BadRequest();
            }

            _context.FavouriteListItems.Add(item);
            _context.SaveChanges();
            return Ok();
        }
    }
}
