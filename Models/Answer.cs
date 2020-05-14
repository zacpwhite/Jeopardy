using System;
using System.Collections.Generic;

namespace Jeopardy.Models
{
    public class Answer
    {
        public int AnswerId { get; set; }

        public int AnswerValue { get; set; }

        public string AnswerText { get; set; }

        public bool HasBeenRead { get; set; }

        public int CategoryId { get; set; }

        public Uri ContentUri { get; set; }

        public Category Category { get; set; }

        public ICollection<Question> Questions { get; set; }
    }
}