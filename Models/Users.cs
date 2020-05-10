namespace Jeopardy.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public bool UserIsHost { get; set; }

        public int UserScore { get; set; }

        public int UserCorrectAnswers { get; set; }

        public int GameId { get; set; }

        public Game Game { get; set; }
    }
}