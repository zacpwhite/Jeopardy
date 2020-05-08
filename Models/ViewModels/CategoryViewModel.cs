using System.Collections.Generic;

namespace Jeopardy.Models.ViewModels 
{
    public class CategoryViewModel {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int CategoryOrder { get; set; }    

        public List<AnswerViewModel> Answers { get; set; }
    }
}