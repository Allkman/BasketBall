using BasketBall.Client.Helpers;

using BasketBall.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketBall.Client.Shared
{
    public partial class TeamsList
    {
        [Inject] IJSRuntime js { get; set; }  
       
        [Parameter] public List<Team> Teams { get; set; }

        private async Task DeleteTeam(Team team)
        {
            await js.MyFunction("custom message");
            var confirmed = await js.Confirm($"Are you sure you want to delete {team.TeamName}?");

            if (confirmed)
            {
                //await teamRepository.DeleteTeam(team.TeamId);
                Teams.Remove(team);
            }
        }
    }
}
