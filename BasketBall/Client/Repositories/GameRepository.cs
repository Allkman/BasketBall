using BasketBall.Client.Helpers;
using BasketBall.Client.Repositories.Interfaces;
using BasketBall.Client.Services.Interfaces;
using BasketBall.Shared.DTOs;
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
        public async Task<Game> GetGame(int gameId)
        {
            return await _httpService.GetHelper<Game>($"{url}/{gameId}");
        }
        public async Task<List<Game>> GetGames()
        {
            return await _httpService.GetHelper<List<Game>>($"{url}");
        }
        public async Task<PaginatedResponse<List<Game>>> GetIndexedGames(PaginationDTO paginationDTO)
        {
            return await _httpService.GetHelper<List<Game>>(url, paginationDTO);
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
        public async Task UpdateGame(Game game)
        {
            var response = await _httpService.Put(url, game);
            
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
        public async Task DeleteGame(int gameId)
        {
            var response = await _httpService.Delete($"{url}/{gameId}");

            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
