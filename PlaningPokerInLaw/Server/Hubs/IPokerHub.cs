using Microsoft.AspNetCore.SignalR;

namespace PlaningPokerInLaw.Server.Hubs
{
    public interface IPokerHub
    {
        Dictionary<string, string> GetConnections();
        Task LogIn(string userFrom);
        Task LogOut(string userFrom);
        Task SendMessage(string userFrom, string userTo, string message);
    }
}