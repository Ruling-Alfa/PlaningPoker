using Microsoft.AspNetCore.SignalR;

namespace PlaningPokerInLaw.Server.Hubs
{
    public class PokerHub : Hub
    {
        private readonly ConnectionService _connectionService;

        public PokerHub(ConnectionService connectionService)
        {
            _connectionService = connectionService;
        }
        public override Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var userEmail = httpContext?.Request.Query["user"];
            _connectionService.AddConnection(userEmail, Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var httpContext = Context.GetHttpContext();
            var userEmail = httpContext?.Request.Query["user"];

            _connectionService.RemoveConnection(userEmail!);

            return base.OnDisconnectedAsync(exception);
        }
        public async Task SendMessage(string userFrom, string userTo, string message)
        {
            if (string.IsNullOrEmpty(userTo))
            {
                await Clients.All.SendAsync("ReceiveMessage", userFrom, userTo, message);
            }
            else
            {
                _connectionService.TryGetConnectionValue(userTo, out var connId);
                await Clients.Client(connId!).SendAsync("ReceiveMessage", userFrom, userTo, message);
            }
        }

        public async Task StartVoting()
        {
            await Clients.All.SendAsync("VotingStarted");
        }

        public async Task StopVoting()
        {
            await Clients.All.SendAsync("VotingStopped");
        }

        public async Task ClearBoards()
        {
            await Clients.All.SendAsync("ClearBoards");
        }

        public async Task OpenCards()
        {
            await Clients.All.SendAsync("CardsOpenedToAll");
        }

        public async Task VoteSubmit(string userFrom, int points, string storyNumber)
        {
            await Clients.All.SendAsync("VotingSubmitted", userFrom, points, storyNumber);
        }

        public async Task LogOut(string userFrom)
        {
            await Clients.All.SendAsync("LogOut", userFrom!);
        }

        public async Task LogIn(string userFrom)
        {
            await Clients.All.SendAsync("Login", userFrom);
        }
    }
}
