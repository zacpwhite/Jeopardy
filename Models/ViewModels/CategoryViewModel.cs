using System.Collections.Generic;

namespace Jeopardy.Models.ViewModels 
{
    public class CategoryViewModel {

        public int CategoryId { get; set; }

        public string CategoryTitle { get; set; }

        public string CategoryDescription { get; set; }

        public int CategorySortOrder { get; set; }    

        public List<AnswerViewModel> Answers { get; set; }
    }
}