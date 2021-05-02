using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketBall.Shared.Models
{
    public class TeamGame
    {
        public int TeamId { get; set; }
        public int GameId { get; set; }
        public Team Team { get; set; }
        public Game Game { get; set; }
    }
}
