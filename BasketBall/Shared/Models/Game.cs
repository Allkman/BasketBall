using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BasketBall.Shared.Models
{
    public class Game
    {
        public int GameId { get; set; }
        [Required]
        public string GameTitle { get; set; }
        [Required]
        public int WinScore { get; set; }
        //navigational property that allows to link two models (one team can have many games...)
        public List<TeamGame> TeamGames { get; set; } = new List<TeamGame>();
    }
}
