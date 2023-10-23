using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace PlaningPokerInLaw.Client
{
    public class PokerHubConnectionService
    {
        private readonly NavigationManager Navigation;
        private HubConnection? hubConnection;
        List<Action<string, string, string>> msgReceivedListeners = new List<Action<string, string, string>>();
        List<Action<string>> logInListeners = new List<Action<string>>();
        List<Action<string>> LogOutListeners = new List<Action<string>>();
        List<Action> VotingStartedListeners = new List<Action>();
        List<Action> VotingStoppedListeners = new List<Action>();
        List<Action> OpenCardsListeners = new List<Action>();
        List<Action<string, int, string>> VotingSubmittedListeners = new List<Action<string, int, string>>();
        List<Action> ClearBoardsListeners = new List<Action>();

        private readonly ParticipantService _userService;

        public PokerHubConnectionService(NavigationManager navigationManager,
            ParticipantService userService)
        {
            _userService = userService;
            Navigation = navigationManager;


            var t = InitializeAsync().GetAwaiter();

            t.OnCompleted(() =>
            {
                SendUpdateToListeners();
            });
        }


        private async Task InitializeAsync()
        {
            //ParticipantService.OnLoad += async (usr) =>
            {
                hubConnection = new HubConnectionBuilder()
                    .WithUrl(Navigation.ToAbsoluteUri($"/pokerhub?user={_userService.User.Email}"))
                    .Build();

                SendUpdateToListeners();

                await hubConnection.StartAsync();
            };
        }

        public void AddMessageListener(Action<string, string, string> msgReceivedListener)
        {
            msgReceivedListeners.Add(msgReceivedListener);
        }
        public void AddLogInListener(Action<string> listener)
        {
            logInListeners.Add(listener);
        }
        public void AddLogOutListener(Action<string> listener)
        {
            LogOutListeners.Add(listener);
        }

        public void AddVotingStartedListenerListener(Action listener)
        {
            VotingStartedListeners.Add(listener);
        }
        public void AddVotingStoppedListenerListener(Action listener)
        {
            VotingStoppedListeners.Add(listener);
        }
        public void AddOpenCardsListenersListener(Action listener)
        {
            OpenCardsListeners.Add(listener);
        }
        public void AddVotingSubmittedListener(Action<string, int, string> listener)
        {
            VotingSubmittedListeners.Add(listener);
        }

        public void AddClearBoardsListener(Action listener)
        {
            ClearBoardsListeners.Add(listener);
        }

        public async Task SendAsync(string userFrom, string userTo, string message)
        {
            if (hubConnection is not null)
            {
                await hubConnection.SendAsync("SendMessage", userFrom, userTo, message);
            }
        }

        public async Task StartVoting()
        {
            if (hubConnection is not null)
            {
                await hubConnection.SendAsync("StartVoting");
            }
        }

        public async Task ClearBoards()
        {
            if (hubConnection is not null)
            {
                await hubConnection.SendAsync("ClearBoards");
            }
        }

        public async Task StopVoting()
        {
            if (hubConnection is not null)
            {
                await hubConnection.SendAsync("StopVoting");
            }
        }

        public async Task OpenCards()
        {
            if (hubConnection is not null)
            {
                await hubConnection.SendAsync("OpenCards");
            }
        }

        public async Task VoteSubmit(string userFrom, int points, string storyNumber)
        {
            if (hubConnection is not null)
            {
                await hubConnection.SendAsync("VoteSubmit", userFrom, points, storyNumber);
            }
        }


        public async Task LogOut(string userFrom)
        {
            if (hubConnection is not null)
            {
                await hubConnection.SendAsync("LogOut", userFrom);
            }
        }

        public async Task LogIn(string userFrom)
        {
            if (hubConnection is not null)
            {
                await hubConnection.SendAsync("LogIn", userFrom);
            }
        }

        public bool IsConnected =>
            hubConnection?.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }
        private void SendUpdateToListeners()
        {
            msgReceivedListeners.ForEach(listener =>
            {
                hubConnection?.On<string, string, string>("ReceiveMessage", listener);
            });
            logInListeners.ForEach(listener =>
            {
                hubConnection?.On<string>("LogIn", listener);
            });
            LogOutListeners.ForEach(listener =>
            {
                hubConnection?.On<string>("LogOut", listener);
            });
            VotingStartedListeners.ForEach(listener =>
            {
                hubConnection?.On("VotingStarted", listener);
            });
            VotingStoppedListeners.ForEach(listener =>
            {
                hubConnection?.On("VotingStopped", listener);
            });
            OpenCardsListeners.ForEach(listener =>
            {
                hubConnection?.On("CardsOpenedToAll", listener);
            });

            VotingSubmittedListeners.ForEach(listener =>
            {
                hubConnection?.On<string, int, string>("VotingSubmitted", listener);
            });
            ClearBoardsListeners.ForEach(listener =>
            {
                hubConnection?.On("ClearBoards", listener);
            });
        }
    }
}
