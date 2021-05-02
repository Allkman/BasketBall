using BasketBall.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace BasketBall.Client.Shared
{
    public partial class IndividualTeam
    {
        [Parameter] public Team Team { get; set; }
        [Parameter] public EventCallback<Team> DeleteTeam { get; set; }
        private string teamURL = string.Empty;

        protected override void OnInitialized()
        {
            teamURL = $"team/{Team.TeamId}/{Team.TeamName.Replace(" ", "-")}";
        }
    }
}
