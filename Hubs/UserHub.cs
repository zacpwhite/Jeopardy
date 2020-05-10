using AutoMapper;
using Jeopardy.Data;
using Microsoft.AspNetCore.SignalR;
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
    }
}