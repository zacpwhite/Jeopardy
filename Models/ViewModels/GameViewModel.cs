using System.Collections.Generic;

namespace Jeopardy.Models.ViewModels {
    public class GameViewModel {
        public int GameId { get; set; }

        public string GameName { get; set; }

        public List<CategoryViewModel> Categories { get; set; }
    }
}