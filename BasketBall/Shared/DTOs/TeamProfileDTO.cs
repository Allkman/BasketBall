using BasketBall.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketBall.Shared.DTOs
{
    public class TeamProfileDTO
    {
        public Team Team { get; set; }
        public List<Game> Games { get; set; }
        public List<Person> Players { get; set; }
    }
}
