using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Jeopardy.Models.ViewModels
{
    public class NewGameViewModel
    {
        [Required]
        [DisplayName("Title")]
        public string GameTitle { get; set; }

        [DisplayName("Description")]
        public string GameDescription { get; set; }

        [DisplayName("Your Username")]
        [Required]
        public string Username { get; set; }
    }
}