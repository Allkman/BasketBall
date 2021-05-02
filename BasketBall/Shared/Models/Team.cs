using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketBall.Shared.Models
{
    public class Team
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string TeamLogo { get; set; }
        public bool EasternConference { get; set; }
        public bool WesternConference { get; set; }
        public List<TeamGame> TeamGames { get; set; } = new List<TeamGame>();
        public List<TeamPlayer> TeamPlayers { get; set; } = new List<TeamPlayer>();

    }
}
