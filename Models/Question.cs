namespace Jeopardy.Models
{
    public class Question
    {
        public int QuestionId { get; set; }

        public string QuestionText { get; set; }

        public bool IsCorrect { get; set; }

        public int AnswerId { get; set; }

        public Answer Answer { get; set; }
    }
}