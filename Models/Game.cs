using System.Collections.Generic;

namespace Jeopardy.Models {
    public enum Round {
        Jeopardy, 
        DoubleJeopardy, 
        FinalJeopardy
    }

    public class Game {
        public int GameId { get; set; }

        public string GameTitle { get; set; }       

        public string GameDescription { get; set; }

        public Round? CurrentRound { get; set; }

        public ICollection<Category> Categories { get; set; }

        public ICollection<User> Users { get; set; }
    }
}