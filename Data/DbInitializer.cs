using System.Linq;
using Jeopardy.Models;

namespace Jeopardy.Data
{
    public static class DbInitializer
    {
        public static void Initialize(JeopardyContext context)
        {
            context.Database.EnsureCreated();

            //Check for games
            if (context.Games.Any())
            {
                //DB has been seeded
                return;
            }

            //Create Demo Game
            var game = new Game()
            {
                GameTitle = "Demo Game",
                GameDescription = "This is a demo game"
            };

            context.Games.Add(game);
            context.SaveChanges();

            //Create Categories

            var categories = new Category[] { };

            for (var i = 1; i <= 13; i++)
            {
                categories.Append(new Category
                {
                    CategoryTitle = $"Category {i}",
                    CategoryDescription = $"This is a description of Category {i}",
                    CategorySortOrder = i,
                    Round = (i <= 6 ? Round.Jeopardy : (i <= 12 ? Round.DoubleJeopardy : Round.FinalJeopardy)),
                    GameId = 1
                });
            }

            context.Categories.AddRange(categories);
            context.SaveChanges();

            //Create Answers

            var answers = new Answer[] { };

            for (var i = 0; i < categories.Length; i++)
            {
                if (categories[i].Round != Round.FinalJeopardy)
                {
                    for (var j = 1; j <= 5; j++)
                    {
                        answers.Append(new Answer
                        {
                            AnswerValue = j * 100,
                            AnswerText = $"This value of this answer in {categories[i].CategoryTitle}",
                            CategoryId = i + 1
                        });
                    }
                }
                else
                {
                    answers.Append(new Answer
                    {
                        AnswerValue = int.MaxValue,
                        AnswerText = $"This is the value of the final jeopardy answer",
                        CategoryId = i + 1
                    });
                }
            }

            context.Answers.AddRange(answers);
            context.SaveChanges();

            //Create Questions

            var questions = new Question[] { };

            for (var i = 0; i < answers.Length; i++)
            {
                if (answers[i].AnswerValue != int.MaxValue)
                {
                    for (var j = 1; j <= 4; j++)
                    {
                        questions.Append(new Question
                        {
                            QuestionText = $"What is ${answers[i].AnswerValue}?",
                            IsCorrect = (j % 3 == 0 ? true : false),
                            AnswerId = i + 1
                        });
                    }
                }
                else
                {
                    questions.Append(new Question{
                        QuestionText = $"What is ${int.MaxValue}?",
                        IsCorrect = true,
                        AnswerId = i + 1
                    });
                }
            }

            context.Questions.AddRange(questions);
            context.SaveChanges();
        }
    }
}