using BasketBall.Client.Helpers;
using BasketBall.Client.Helpers.Interfaces;
using BasketBall.Client.Repositories;
using BasketBall.Client.Repositories.Interfaces;
using BasketBall.Client.Services;
using BasketBall.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BasketBall.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            //cofiguring instance with token
            builder.Services.AddHttpClient<HttpClientWithToken>(client =>
            client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddHttpClient<HttpClientWithoutToken>(client =>
            client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
                
            ConfigureServices(builder.Services);
            await builder.Build().RunAsync();
        }
        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IHttpService, HttpService>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IAccountsRepository, AccountsRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDisplayMessage, DisplayMessage>();

            services.AddApiAuthorization();
        }
    }
}
