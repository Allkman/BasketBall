using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketBall.Shared.Models
{
    public class TeamPlayer
    {
        public int PersonId { get; set; }
        public int TeamId { get; set; }
        public Person Person { get; set; } //related with BasketBallPlayer
        public Team Team { get; set; }
        public string Position { get; set; }
        public int Order { get; set; } //to visualize Starting five players

        public int OutsideScoring { get; set; }
        public int InsideScoring { get; set; }
        public int Defending { get; set; }
        public int Athleticism { get; set; }
        public int Rebounding { get; set; }
        public int Playmaking { get; set; }
        public int OverallRating { get; set; }
    }
}
