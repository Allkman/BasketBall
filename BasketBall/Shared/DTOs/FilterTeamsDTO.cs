using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketBall.Shared.DTOs
{
    public class FilterTeamsDTO
    {
        public int Page { get; set; }
        public int RecordsPerPage { get; set; }
        public PaginationDTO Pagination
        {
            get { return new PaginationDTO() { Page = Page, RecordsPerPage = RecordsPerPage }; }
        }
        public string TeamName { get; set; }
        public int GameId { get; set; }
        public bool EasternConference { get; set; }
        public bool WesternConference { get; set; }
    }
}
