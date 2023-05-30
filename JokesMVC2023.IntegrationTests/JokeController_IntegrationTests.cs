using JokesMVC2023.Models.Data;
using JokesMVC2023.Models;
using JokesMVC2023.Services.Concrete;
using JokesMVC2023.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JokesMVC2023.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace JokesMVC2023.Tests
{
    public class JokeController_IntegrationTests
    {
        public IJokeService _jokeService;
        public JokeController controller;
        public DbContextOptions options;
        public JokeDBContext context;

        [SetUp]
        public void Setup()
        {
            options = new DbContextOptionsBuilder<JokeDBContext>()
            .UseInMemoryDatabase(databaseName: "JokeDatabase")
            .Options;

            context = new JokeDBContext(options);
            _jokeService = new JokeService(context);
            controller = new JokeController(_jokeService);

        }

        [Test]
        public void InsertNewJoke_CreatesNewJoke()
        {
            // Arrange

            InitializeDbForTests(context);
            JokeCreateDTO jokeCreate = new JokeCreateDTO()
            {
                JokeAnswer = "joke answer",
                JokeQuestion = "joke question"
            };

            // Act
            var result = controller.Create(jokeCreate).Result;

            Assert.That(result.GetType() ==  typeof(CreatedResult));
            // Assert
        }

        [Test]
        public void InsertNewJoke_IncorrectModelFormat_NoAnswer_ReturnsProblem_500_Result ()
        {
            // Arrange

            InitializeDbForTests(context);
            JokeCreateDTO jokeCreate = new JokeCreateDTO()
            {
                JokeQuestion = "joke question"
            };

            // Act
            var result = controller.Create(jokeCreate).Result;

            Assert.That(result.GetType() == typeof(ObjectResult));
            var stronglyTypedResult = result as ObjectResult;
            Assert.That(stronglyTypedResult.StatusCode == 500);
            // Assert
        }

        [Test]
        public void InsertNewJoke_IncorrectModelFormat_NoQuestion_ReturnsProblem_500_Result()
        {
            // Arrange

            InitializeDbForTests(context);
            JokeCreateDTO jokeCreate = new JokeCreateDTO()
            {
                JokeAnswer = "joke answer"
            };

            // Act
            var result = controller.Create(jokeCreate).Result;

            Assert.That(result.GetType() == typeof(ObjectResult));
            var stronglyTypedResult = result as ObjectResult;
            Assert.That(stronglyTypedResult.StatusCode == 500);
            // Assert
        }

        [Test]
        public void GetJokePartial_ReturnsCorrectNumberOfJokesInModel()
        {
            // Arrange

            InitializeDbForTests(context);

            context.Jokes.AddRange(new List<Joke>
            {
                new Joke { JokeQuestion = "abc", JokeAnswer = "GHI"},
                new Joke { JokeQuestion = "ABC", JokeAnswer = "jkl"},
                new Joke { JokeQuestion = "def", JokeAnswer = "JKL"},
                new Joke { JokeQuestion = "DEF", JokeAnswer = "mno"},
                new Joke { JokeQuestion = "ghi", JokeAnswer = "MNO"},
            });
            context.SaveChanges();
            // Act
            var result = controller.JokeTablePartial().Result;

            Assert.That(result.GetType() == typeof(PartialViewResult));
            var stronglyTypedResult = result as PartialViewResult;
            var modelDetails = (Task<IEnumerable<Joke>>)stronglyTypedResult.ViewData.Model;
            Assert.That(modelDetails.Result.Count() == 5);
            // Assert
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
