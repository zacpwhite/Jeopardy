using System.Threading.Tasks;
using AutoMapper;
using Jeopardy.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Jeopardy.Hubs
{
    public class UserHub : Hub 
    {
        private ILogger<UserHub> _logger;

        private JeopardyContext _context;

        private IMapper _mapper;

        public UserHub(ILogger<UserHub> logger, JeopardyContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task UserJoined(string userId, string gameId)
        {
            int userIdInt;
            int gameIdInt;
            int.TryParse(userId, out userIdInt);
            int.TryParse(gameId, out gameIdInt);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId.Equals(userIdInt) && x.GameId.Equals(gameIdInt));
            if (user != null) {
                await Clients.All.SendAsync("userJoined", user.UserId, user.Username);
            }
        }
    }
}