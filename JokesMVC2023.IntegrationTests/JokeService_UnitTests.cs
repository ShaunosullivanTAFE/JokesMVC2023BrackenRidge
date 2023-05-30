using JokesMVC2023.Controllers;
using JokesMVC2023.Models;
using JokesMVC2023.Models.Data;
using JokesMVC2023.Services.Concrete;
using JokesMVC2023.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace JokesMVC2023.Tests
{
    public class JokeService_UnitTests
    {
        public IJokeService _jokeService;

        public DbContextOptions options;

        [SetUp]
        public void Setup()
        {
            options = new DbContextOptionsBuilder<JokeDBContext>()
            .UseInMemoryDatabase(databaseName: "JokeDatabase")
            .Options;
        }

        [Test]
        public void GetALLJokes_Returns5Jokes()
        {
            // Arrange

            JokeDBContext context = new JokeDBContext(options);
            InitializeDbForTests(context);
            _jokeService = new JokeService(context);

            // Add 5 Jokes
            context.Jokes.AddRange(new List<Joke>
            {
                new Joke { JokeQuestion = "", JokeAnswer = ""},
                new Joke { JokeQuestion = "", JokeAnswer = ""},
                new Joke { JokeQuestion = "", JokeAnswer = ""},
                new Joke { JokeQuestion = "", JokeAnswer = ""},
                new Joke { JokeQuestion = "", JokeAnswer = ""},
            });
            context.SaveChanges();

            // Act
            var jokes = _jokeService.GetAllJokes().Result;

            // Assert

            Assert.That(jokes.Count() == 5);
        }

        [Test]
        public void GetAllJokesWithLowerCaseFilter_Returns3Jokes()
        {
            // Arrange
            JokeDBContext context = new JokeDBContext(options);
            InitializeDbForTests(context);
            _jokeService = new JokeService(context);


            // Add 5 Jokes
            context.Jokes.AddRange(new List<Joke>
            {
                new Joke { JokeQuestion = "abc", JokeAnswer = "GHI"},
                new Joke { JokeQuestion = "ABC", JokeAnswer = "jkl"},
                new Joke { JokeQuestion = "def", JokeAnswer = "JKL"},
                new Joke { JokeQuestion = "DEF", JokeAnswer = "mno"},
                new Joke { JokeQuestion = "Ahi", JokeAnswer = "MNO"},
            });
            context.SaveChanges();

            var jokes = _jokeService.GetAllJokesWithFilter("a").Result;

            // Assert

            Assert.That(jokes.Count() == 3);

        }
        [Test]
        public void GetAllJokesWithUpperCaseFilter_Returns4Jokes()
        {
            // Arrange
            JokeDBContext context = new JokeDBContext(options);
            InitializeDbForTests(context);
            _jokeService = new JokeService(context);


            // Add 5 Jokes
            context.Jokes.AddRange(new List<Joke>
            {
                new Joke { JokeQuestion = "abc", JokeAnswer = "GHI"},
                new Joke { JokeQuestion = "ABC", JokeAnswer = "Dkl"},
                new Joke { JokeQuestion = "def", JokeAnswer = "JKL"},
                new Joke { JokeQuestion = "DEF", JokeAnswer = "Ano"},
                new Joke { JokeQuestion = "ghi", JokeAnswer = "DNO"},
            });
            context.SaveChanges();

            var jokes = _jokeService.GetAllJokesWithFilter("D").Result;

            // Assert

            Assert.That(jokes.Count() == 4);

        }

        [Test]
        public void GetAllJokesWithEmptyFilter_ReturnsAll5Jokes()
        {
            // Arrange
            JokeDBContext context = new JokeDBContext(options);
            InitializeDbForTests(context);
            _jokeService = new JokeService(context);

            // Add 5 Jokes
            context.Jokes.AddRange(new List<Joke>
            {
                new Joke { JokeQuestion = "abc", JokeAnswer = "GHI"},
                new Joke { JokeQuestion = "ABC", JokeAnswer = "jkl"},
                new Joke { JokeQuestion = "def", JokeAnswer = "JKL"},
                new Joke { JokeQuestion = "DEF", JokeAnswer = "mno"},
                new Joke { JokeQuestion = "ghi", JokeAnswer = "MNO"},
            });
            context.SaveChanges();

            var jokes = _jokeService.GetAllJokesWithFilter("").Result;

            // Assert

            Assert.That(jokes.Count() == 5);

        }

        [Test]
        public void CreateNewJokeWithValidDTO_Inserts1MatchingJoke()
        {
            // Arrange
            JokeDBContext context = new JokeDBContext(options);
            InitializeDbForTests(context);
            _jokeService = new JokeService(context);

            string jokeQuestion = "Why did the chicken cross the road?";
            
            var jokeDTO = new JokeCreateDTO
            {
                JokeQuestion = jokeQuestion,
                JokeAnswer = "To get to the other side"
            };

            // Act
            var createdJoke = _jokeService.CreateJoke(jokeDTO).Result;

            // Assert

            Assert.That(context.Jokes.Count() == 1);
            Assert.That(context.Jokes.FirstOrDefault() != null);
            Assert.That(context.Jokes.FirstOrDefault().JokeQuestion == jokeQuestion);

        }

        [Test]
        public void UpdateJokeWithValidDetail_UpdatesMatchingJoke()
        {
            // Arrange
            JokeDBContext context = new JokeDBContext(options);
            InitializeDbForTests(context);
            _jokeService = new JokeService(context);

            //context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Jokes ON");
            var joke = new Joke
            {
                JokeQuestion = "abc1234",
                JokeAnswer = "To get to the other side"
            };

            context.Jokes.Add(joke);
            context.SaveChanges();
            // Required when modifying an entity twice in a single operation (Insert and Update here)
            context.ChangeTracker.Clear();


            var updatedJoke = new Joke
            {
                Id = joke.Id,
                JokeQuestion = "abc12345",
                JokeAnswer = "To get to the other side"
            };

            var success = _jokeService.UpdateJoke(updatedJoke).Result;

            // Assert

            Assert.That(success == true);
            Assert.That(context.Jokes.Count() == 1);
            Assert.That(context.Jokes.FirstOrDefault().JokeQuestion == "abc12345");
        }

        [Test]
        public void UpdateJokeWithInvalidDetail_ReturnsFalse()
        {
            // Arrange
            JokeDBContext context = new JokeDBContext(options);
            InitializeDbForTests(context);
            _jokeService = new JokeService(context);

            //context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Jokes ON");
            var joke = new Joke
            {
                JokeQuestion = "abc1234",
                JokeAnswer = "To get to the other side"
            };

            context.Jokes.Add(joke);
            context.SaveChanges();
            //context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Jokes OFF");
            // Act
            var updatedJoke = new Joke
            {
                Id = joke.Id,
                JokeQuestion = "abc12345"
            };

            var success = _jokeService.UpdateJoke(updatedJoke).Result;

            // Assert

            Assert.That(success == false);
            Assert.That(context.Jokes.FirstOrDefault().JokeQuestion != "abc12345");
        }

        public void InitializeDbForTests(JokeDBContext db)
        {
            db.Database.EnsureCreated();
            ReinitializeDbForTests(db);
        }

        public void ReinitializeDbForTests(JokeDBContext db)
        {
            db.Jokes.RemoveRange(db.Jokes);
            db.SaveChanges();
        }

     
     
    }
}