using BasketBall.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketBall.Shared.DTOs
{
    public class IndexPageDTO
    {
        public List<Team> EasternConference { get; set; }
        public List<Team> WesternConference { get; set; }
    }
}
