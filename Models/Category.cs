using System.Collections.Generic;

namespace Jeopardy.Models {

    public class Category {
        public int CategoryId { get; set; }

        public string CategoryTitle { get; set; }

        public string CategoryDescription { get; set; }

        public int CategorySortOrder { get; set; } 

        public Round Round { get; set; }

        public int GameId { get; set; }

        public Game Game { get; set; }

        public ICollection<Answer> Answers { get; set; }
    }
}