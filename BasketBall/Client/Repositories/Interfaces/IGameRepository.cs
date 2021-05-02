using BasketBall.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketBall.Client.Repositories.Interfaces
{
    public interface IGameRepository
    {
        Task CreateGame(Game game);
        Task<List<Game>> GetGames();
    }
}
