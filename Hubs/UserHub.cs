using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Jeopardy.Data;
using Jeopardy.Models;
using Jeopardy.Models.ViewModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Jeopardy.Hubs
{
    public class UserHub : Hub 
    {
        static List<ConnectedUser> _connectedUsers = new List<ConnectedUser>();

        private ILogger<UserHub> _logger;

        private JeopardyContext _context;

        private IMapper _mapper;

        public UserHub(ILogger<UserHub> logger, JeopardyContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public override async Task OnDisconnectedAsync(Exception exception) 
        {   
            var user = _connectedUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (user != null) {
                _connectedUsers.Remove(user);
            }
            await Groups.RemoveFromGroupAsync(user.ConnectionId, user.GameId.ToString());
            await Clients.Group(user.GameId.ToString()).SendAsync("refreshUsersList", _connectedUsers.Where(x => x.GameId.Equals(user.GameId)).ToArray());
            await base.OnDisconnectedAsync(exception);
        }

        public async Task UserJoined(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId.Equals(userId));
            if (user != null) {
                var connectedUser = new ConnectedUser {
                    UserId = user.UserId,
                    UserName = user.Username,
                    ConnectionId = Context.ConnectionId,
                    IsHost = user.UserIsHost,
                    GameId = user.GameId
                };

                _connectedUsers.Add(connectedUser);
                await Groups.AddToGroupAsync(connectedUser.ConnectionId, user.GameId.ToString());
                await Clients.Group(user.GameId.ToString()).SendAsync("refreshUsersList", _connectedUsers.Where(x => x.GameId.Equals(user.GameId)).ToArray());
            }
        }
    }
}