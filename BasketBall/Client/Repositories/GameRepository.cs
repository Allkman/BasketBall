using BasketBall.Client.Repositories.Interfaces;
using BasketBall.Client.Services.Interfaces;
using BasketBall.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketBall.Client.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly IHttpService _httpService;
        private readonly string url = "api/games"; //endpoint in which my GameController is located

        public GameRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }
        public async Task<List<Game>> GetGames()
        {
            var response = await _httpService.Get<List<Game>>(url);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }
        public async Task CreateGame(Game game)
        {
            var response = await _httpService.Post(url, game);
            //if the response is not successfull throw exeption and display error message with body
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
