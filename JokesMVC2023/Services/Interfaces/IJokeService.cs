using JokesMVC2023.Models;
using JokesMVC2023.Models.Data;

namespace JokesMVC2023.Services.Interfaces
{
    public interface IJokeService
    {
        public Task<Joke> CreateJoke(JokeCreateDTO jokeCreate);
        public Task<bool> DeleteJoke(int jokeId);
        public Task<bool> UpdateJoke(Joke updatedJoke);
        public Task<Joke> GetJoke(int jokeId);
        public Task<IEnumerable<Joke>> GetAllJokes();
        public Task<IEnumerable<Joke>> GetAllJokesWithFilter(string filter);

    }
}
