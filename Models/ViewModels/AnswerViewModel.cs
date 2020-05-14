using System;
using System.Collections.Generic;

namespace Jeopardy.Models.ViewModels {
    public class AnswerViewModel {

        public int AnswerId { get; set; }

        public int AnswerValue { get; set; }

        public string AnswerText { get; set; }

        public bool HasBeenRead { get; set; }

        public Uri ContentUri { get; set; }

        public List<QuestionViewModel> Questions { get; set; }
    }
}