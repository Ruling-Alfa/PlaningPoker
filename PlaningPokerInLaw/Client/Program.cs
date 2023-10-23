using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PlaningPokerInLaw.Client;

namespace PlaningPokerInLaw.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            builder.Services.AddSingleton<ParticipantService>();
            builder.Services.AddSingleton<PokerHubConnectionService>();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            var host = builder.Build();
            var u = host.Services.GetRequiredService<ParticipantService>().User;
            await host.RunAsync();
        }
    }
}