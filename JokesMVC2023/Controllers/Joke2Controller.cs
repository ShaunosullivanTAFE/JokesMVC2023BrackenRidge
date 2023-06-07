using JokesMVC2023.Models;
using JokesMVC2023.Models.Data;
using Microsoft.AspNetCore.Mvc;

namespace JokesMVC2023.Controllers
{
    public class Joke2Controller : Controller
    {
        JokeDBContext _jokedbContext;
        public Joke2Controller(JokeDBContext jokeDBContext)
        {
            _jokedbContext = jokeDBContext;
        }
        public IActionResult Index()
        {
            return View(_jokedbContext.Jokes.ToList());
        }

        public IActionResult IndexPartial()
        {
            return PartialView("_IndexPartial", _jokedbContext.Jokes.ToList());
        }

        public IActionResult CreateModal() 
        {
            return PartialView();
        }


        public IActionResult CreateJoke([FromBody] JokeCreateDTO newJoke)
        {
            _jokedbContext.Jokes.Add(new Joke
            {
                JokeAnswer = newJoke.JokeAnswer,
                JokeQuestion = newJoke.JokeQuestion
            });
            _jokedbContext.SaveChanges();
            return IndexPartial();
        }
    }
}
