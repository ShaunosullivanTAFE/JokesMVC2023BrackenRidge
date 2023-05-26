using JokesMVC2023.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JokesMVC2023.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace JokesMVC2023.Controllers
{
    public class JokeController : Controller
    {
        private readonly JokeDBContext _jokeContext;
        public JokeController(JokeDBContext jokeContext)
        {
            _jokeContext = jokeContext;
        }

        // GET: JokeController
        public ActionResult Index()
        {
            return View(_jokeContext.Jokes.AsEnumerable());
        }

        [HttpGet]
        public async Task<ActionResult> JokeTablePartial(string query = "")
        {

            if (!string.IsNullOrEmpty(query))
            {
                var filteredJokeData = _jokeContext.Jokes.Where(c => c.JokeQuestion.Contains(query) || c.JokeAnswer.Contains(query)).AsEnumerable();
                return PartialView("_JokeTable", filteredJokeData);

            }

            Thread.Sleep(2000);
            var jokeData = _jokeContext.Jokes.AsEnumerable();

            return PartialView("_JokeTable", jokeData);
        }



        // GET: JokeController/Details/5
        public ActionResult Details(int id)
        {
            Thread.Sleep(2000);
            if (id == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var joke = _jokeContext.Jokes.FirstOrDefault(c => c.Id == id);

            return joke != null ? View(joke) : RedirectToAction(nameof(Index));
        }

        // GET: JokeController/Create
        public async Task<ActionResult> Create()
        {
            Thread.Sleep(2000);
            return PartialView("_CreateJoke");
        }

        // POST: JokeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([FromBody]JokeCreateDTO jokeCreate)
        {
            try
            {
                var test = HttpContext.Request;
                Thread.Sleep(2000);
                // simple error handling
                if (ModelState.IsValid)
                {

                    Joke joke = new Joke{
                        JokeQuestion = jokeCreate.JokeQuestion,
                        JokeAnswer = jokeCreate.JokeAnswer 
                    };

                    _jokeContext.Jokes.Add(joke);
                    await _jokeContext.SaveChangesAsync();

                    //return BadRequest("There was an issue creating the joke");
                    return Created("/Joke/Create", joke);
                }
                else
                {
                    return BadRequest("There was an issue processing the provided model");
                }
            }
            catch
            {
                return Problem("There was an issue processing your request");
            }
        }

        // GET: JokeController/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            Thread.Sleep(2000);
            if (id == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var joke = await _jokeContext.Jokes.FirstOrDefaultAsync(c => c.Id == id);

            return joke != null ? PartialView("_Edit", joke) : RedirectToAction(nameof(Index));
        }

        // POST: JokeController/Edit/5
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([FromQuery]int id, [FromBody]Joke joke)
        {
            //if (id != joke.Id)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                _jokeContext.Jokes.Update(joke);
                await _jokeContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(joke);
        }

        // POST: JokeController/Delete/5
        [HttpDelete]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete([FromQuery]int id)
        {
            try
            {
                var joke = _jokeContext.Jokes.FirstOrDefault(c => c.Id == id);
                if (joke != null)
                {
                    _jokeContext.Jokes.Remove(joke);
                    await _jokeContext.SaveChangesAsync();
                    return NoContent();
                }
                return BadRequest("Joke was not found");
            }
            catch
            {
                return Problem("There was an issue completing your request");
            }
        }
    }
}
