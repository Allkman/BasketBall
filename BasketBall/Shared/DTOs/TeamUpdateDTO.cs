using BasketBall.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketBall.Shared.DTOs
{
    public class TeamUpdateDTO
    {
        public Team Team { get; set; }
        public List<Person> TeamPlayers { get; set; }
        public List<Game> SelectedGames { get; set; }
        public List<Game> NotSelectedGames { get; set; }
    }
}
