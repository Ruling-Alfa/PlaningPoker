using Blazored.LocalStorage;
using Newtonsoft.Json;
using PlaningPokerInLaw.Shared;
using System;
using System.Net.Http.Json;

namespace PlaningPokerInLaw.Client
{
    public class ParticipantService
    {
        public static event Action<Participant> OnLoad = (usr) => { };
        public event Action OnChange;
        private readonly IServiceProvider _serviceProvider;
        public ParticipantService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            var t = CheckCacheForLocalUser().ConfigureAwait(false).GetAwaiter();
            t.OnCompleted(() => { OnLoad?.Invoke(userValue!); });

        }

        private async Task CheckCacheForLocalUser()
        {
            using (var scope = _serviceProvider.GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                var localstorage = scope.ServiceProvider.GetRequiredService<ILocalStorageService>();

                var userJson = await localstorage.GetItemAsStringAsync("user");

                if (!string.IsNullOrEmpty(userJson))
                {
                    userValue = JsonConvert.DeserializeObject<Participant>(userJson);

                    var httpClient = scope.ServiceProvider.GetRequiredService<HttpClient>();
                    try
                    {
                        var httpUserResponse = await httpClient.GetAsync(
                            $"/Participants/{userValue.Email}");
                        httpUserResponse.EnsureSuccessStatusCode();
                        var userFromServer = await httpUserResponse.Content
                            .ReadFromJsonAsync<Participant>();
                        
                    }
                    catch
                    {
                        userValue = null;
                        await SetAsync(null);
                    }
                }
            }
        }

        private Participant userValue;

        public Participant User
        {
            get
            {
                if (userValue is null)
                {
                    var t = CheckCacheForLocalUser().ConfigureAwait(false).GetAwaiter();
                    t.OnCompleted(() =>
                    {
                        if (userValue is not null)
                        {
                            OnChange?.Invoke();
                        }
                    });
                }
                return userValue;
            }

            private set
            {
                userValue = value;
            }
        }

        public async Task SetAsync(Participant user)
        {
            using (var scope = _serviceProvider.GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                var localstorage = scope.ServiceProvider.GetRequiredService<ILocalStorageService>();

                userValue = user;
                var userJson = user == null ? "" : JsonConvert.SerializeObject(user);
                await localstorage.SetItemAsStringAsync("user", userJson);
            }
            OnChange?.Invoke();
        }

        public async Task SetIfNullAsync(Participant user)
        {
            if (userValue == null)
            {
                await SetAsync(user);
            }
        }
    }
}
