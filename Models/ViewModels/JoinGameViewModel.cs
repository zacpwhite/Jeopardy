using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Jeopardy.Models.ViewModels
{
    public class JoinGameViewModel
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid game selection!")]
        public int GameId { get; set; }

        [Required]
        [DisplayName("Your Username")]
        [Remote("ValidateUsername", "User", ErrorMessage = "Username already taken!", AdditionalFields=nameof(GameId))]
        public string Username { get; set; }
    }
}