using Microsoft.AspNetCore.SignalR;

namespace PlaningPokerInLaw.Server.Hubs
{
    public class ConnectionService
    {
        private Dictionary<string, string> _connections = new Dictionary<string, string>();

        public Dictionary<string, string> GetConnections()
        {
            return _connections;
        }
        public void AddConnection(string userEmail, string connectionId)
        {
            if (userEmail is not null)
            {
                if (_connections.ContainsKey(userEmail!))
                {
                    _connections.Remove(userEmail!);
                }
                _connections.Add(userEmail!, connectionId);

            }
            else
            {
                throw new Exception("User-Email is null or empty");
            }
        }

        public void RemoveConnection(string userEmail)
        {
            _connections.Remove(userEmail!);
        }
        public void TryGetConnectionValue(string userTo, out string connId)
        {
                _connections.TryGetValue(userTo, out connId);
        }
    }
}
