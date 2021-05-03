using AutoMapper;
using BasketBall.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketBall.Server.Helpers
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            // when updating profiles, it will only send picture if it is updated.
            CreateMap<Person, Person>()
                .ForMember(x => x.Picture, option => option.Ignore());

            CreateMap<Team, Team>()
                .ForMember(x => x.TeamLogo, option => option.Ignore());
        }
    }
}
