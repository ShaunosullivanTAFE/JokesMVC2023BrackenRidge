using JokesMVC2023.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JokesMVC2023.Models.Data;
using Microsoft.EntityFrameworkCore;
using JokesMVC2023.Services.Interfaces;

namespace JokesMVC2023.Controllers
{
    public class JokeController : Controller
    {
        private readonly IJokeService _jokeService;
        public JokeController(IJokeService jokeService)
        {
            _jokeService = jokeService;
        }

        // GET: JokeController
        public async Task<ActionResult> Index()
        {
            return View(await _jokeService.GetAllJokes());
        }

        [HttpGet]
        public async Task<ActionResult> JokeTablePartial(string query = "")
        {

            var jokes = _jokeService.GetAllJokesWithFilter(query);

            return PartialView("_JokeTable", jokes);
        }



        // GET: JokeController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var joke = await _jokeService.GetJoke(id);

            return joke != null ? View(joke) : RedirectToAction(nameof(Index));
        }

        // GET: JokeController/Create
        public async Task<ActionResult> Create()
        {
            return PartialView("_CreateJoke");
        }

        // POST: JokeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([FromBody]JokeCreateDTO jokeCreate)
        {
            try
            {
                // simple error handling
                if (ModelState.IsValid)
                { 
                    var result = await _jokeService.CreateJoke(jokeCreate);

                    //return BadRequest("There was an issue creating the joke");
                    return Created("/Joke/Create", result);
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
            var joke = await _jokeService.GetJoke(id);

            return joke != null ? PartialView("_Edit", joke) : RedirectToAction(nameof(Index));
        }

        // POST: JokeController/Edit/5
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([FromQuery]int id, [FromBody]Joke joke)
        {
            if (ModelState.IsValid)
            {
                var result = await _jokeService.UpdateJoke(joke);
                return result ? Ok(result) : BadRequest();
            }
            return BadRequest();
        }

        // POST: JokeController/Delete/5
        [HttpDelete]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete([FromQuery]int id)
        {
            try
            {
                var result = await _jokeService.DeleteJoke(id);

                return result ? Ok("Joke Deleted") : BadRequest("There was an issue deleting the joke");            
            }
            catch
            {
                return Problem("There was an issue completing your request");
            }
        }
    }
}
