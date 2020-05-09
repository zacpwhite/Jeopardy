using System.Collections.Generic;
using System.Linq;
using Jeopardy.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Jeopardy.Data
{
    public static class DbInitializer
    {
        public static void Initialize(JeopardyContext context, ILogger<Program> logger)
        {
            logger.LogInformation("Migrating database");
            context.Database.Migrate();

            logger.LogInformation("Checking for Seed data");

            if (context.Games.Any()) 
            {
                logger.LogInformation("Database already seeded");
                return;
            }

            logger.LogInformation("Seeding database");

            //Create Demo Game
            logger.LogInformation("Creating Game");

            var game = new Game()
            {
                GameTitle = "Demo Game",
                GameDescription = "This is a demo game"
            };

            context.Games.Add(game);
            context.SaveChanges();

            logger.LogInformation("Game Saved");

            //Create Categories

            logger.LogInformation("Creating Categories");

            var categories = new List<Category>();

            for (var i = 1; i <= 13; i++)
            {
                if (i % 13 != 0)
                {
                    categories.Add(new Category
                    {
                        CategoryTitle = $"Category {i}",
                        CategoryDescription = $"This is a description of Category {i}",
                        CategorySortOrder = i,
                        Round = (i <= 6 ? Round.Jeopardy : Round.DoubleJeopardy),
                        GameId = 1
                    });
                }
                else
                {
                    categories.Add(new Category
                    {
                        CategoryTitle = $"Final Jeopardy",
                        CategoryDescription = $"This is a description of Final Jeopardy",
                        CategorySortOrder = i,
                        Round = Round.FinalJeopardy,
                        GameId = 1
                    });
                }
            }

            logger.LogInformation($"{categories.Count()} categories to create");

            context.Categories.AddRange(categories);
            context.SaveChanges();

            logger.LogInformation("Categories Saved");

            //Create Answers

            logger.LogInformation("Creating Answers");

            categories = categories.OrderBy(x => x.CategoryId).ToList();

            var answers = new List<Answer>();

            for (var i = 0; i < categories.Count(); i++)
            {
                if (categories[i].Round != Round.FinalJeopardy)
                {
                    for (var j = 1; j <= 5; j++)
                    {
                        answers.Add(new Answer
                        {
                            AnswerValue = j * 100,
                            AnswerText = $"This value of this answer in {categories[i].CategoryTitle}",
                            CategoryId = i + 1
                        });
                    }
                }
                else
                {
                    answers.Add(new Answer
                    {
                        AnswerValue = int.MaxValue,
                        AnswerText = $"This is the value of the final jeopardy answer",
                        CategoryId = i + 1
                    });
                }
            }

            logger.LogInformation($"{answers.Count()} answers to create");

            context.Answers.AddRange(answers);
            context.SaveChanges();

            logger.LogInformation("Answers Saved");

            //Create Questions

            logger.LogInformation("Creating Questions");

            answers = answers.OrderBy(x => x.AnswerId).ToList();

            var questions = new List<Question>();

            for (var i = 0; i < answers.Count(); i++)
            {
                if (answers[i].AnswerValue != int.MaxValue)
                {
                    for (var j = 1; j <= 4; j++)
                    {
                        questions.Add(new Question
                        {
                            QuestionText = $"What is ${answers[i].AnswerValue * (j % 3 == 0 ? 1 : j + 4)}?",
                            IsCorrect = (j % 3 == 0 ? true : false),
                            AnswerId = i + 1
                        });
                    }
                }
                else
                {
                    questions.Add(new Question
                    {
                        QuestionText = $"What is ${int.MaxValue}?",
                        IsCorrect = true,
                        AnswerId = i + 1
                    });
                }
            }

            logger.LogInformation($"{questions.Count()} questions to create");

            context.Questions.AddRange(questions);
            context.SaveChanges();

            logger.LogInformation("Questions Saved");
        }
    }
}