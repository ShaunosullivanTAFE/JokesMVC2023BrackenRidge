using JokesMVC2023.Models;
using JokesMVC2023.Models.Data;
using JokesMVC2023.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JokesMVC2023.Services.Concrete
{
    public class JokeService : IJokeService
    {
        private readonly JokeDBContext _dbContext;
        public JokeService(JokeDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Joke> CreateJoke(JokeCreateDTO jokeCreate)
        {
            Joke newJoke = new Joke()
            {
                JokeQuestion = jokeCreate.JokeQuestion,
                JokeAnswer = jokeCreate.JokeAnswer
            };
            _dbContext.Jokes.Add(newJoke);
            await _dbContext.SaveChangesAsync();
            return newJoke;
        }

        public async Task<bool> DeleteJoke(int jokeId)
        {
            var joke = _dbContext.Jokes.FirstOrDefault(c => c.Id == jokeId);
            if (joke != null)
            {
                _dbContext.Jokes.Remove(joke);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Joke>> GetAllJokes()
        {
            var jokes = _dbContext.Jokes.AsNoTracking().AsEnumerable();
            return jokes;
        
        }

        public async Task<IEnumerable<Joke>> GetAllJokesWithFilter(string filter)
        {
            if (String.IsNullOrEmpty(filter))
            {
                return await GetAllJokes();
            }

            var jokes = _dbContext.Jokes.Where(c => c.JokeQuestion.ToLower().Contains(filter.ToLower()) 
            || c.JokeAnswer.ToLower().Contains(filter.ToLower())).AsNoTracking().AsEnumerable();
            return jokes;
        }


        /// <summary>
        /// get a single Joke
        /// </summary>
        /// <param name="jokeId"></param>
        /// <returns>Returns the found joke, or null</returns>
        public async Task<Joke> GetJoke(int jokeId)
        {
            if(jokeId <= 0)
            {
                return null;
            }

            var joke = await _dbContext.Jokes.Where(c => c.Id == jokeId).FirstOrDefaultAsync();
           
            return joke;

        }

        /// <summary>
        /// Update a joke, requires a complete joke object to be provided
        /// </summary>
        /// <param name="updatedJoke"></param>
        /// <returns>true if the operation succeeded, false otherwise</returns>
        public async Task<bool> UpdateJoke(Joke updatedJoke)
        {
            try
            {
                if (updatedJoke == null || updatedJoke.Id <= 0)
                {
                    return false;
                }

                _dbContext.Jokes.Update(updatedJoke);
                var result = await _dbContext.SaveChangesAsync();
                return result == 1 ? true : false;
            }
            catch (Exception e)
            {
                return false;
            }
            
        }
    }
}
