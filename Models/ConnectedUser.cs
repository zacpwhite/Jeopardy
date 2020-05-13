namespace Jeopardy.Models
{
    public class ConnectedUser
    {
        public string ConnectionId { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public bool IsHost { get; set; }

        public int GameId { get; set; }
    }
}