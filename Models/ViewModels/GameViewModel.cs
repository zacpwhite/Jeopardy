using System.Collections.Generic;

namespace Jeopardy.Models.ViewModels {
    public class GameViewModel {

        public int GameId { get; set; }

        public string GameTitle { get; set; }

        public string GameDescription { get; set; }

        public List<CategoryViewModel> Categories { get; set; }
    }
}