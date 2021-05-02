using BasketBall.Server.Data;
using BasketBall.Server.Services.Interfaces;
using BasketBall.Shared.DTOs;
using BasketBall.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketBall.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")] //this controller is in ..../api/people
    public class TeamsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IFileStorageService _fileStorageService;
        private string containerName = "teams";

        public TeamsController(ApplicationDbContext dbContext, IFileStorageService fileStorageService)
        {
            _dbContext = dbContext;
            _fileStorageService = fileStorageService;
        }
        [HttpGet] //specific method to display two lists in IndexPage ("/")
        public async Task<ActionResult<IndexPageDTO>> Get()
        {
            var limit = 5; //there are total 15 teams per conference
            var easternTeam = await _dbContext.Teams
                .Where(x => x.EasternConference).Take(limit)
                .OrderByDescending(x => x.TeamName).ToListAsync();
            var westernTeam = await _dbContext.Teams
                .Where(x => x.WesternConference).Take(limit)
                .OrderByDescending(x => x.TeamName).ToListAsync();

            var response = new IndexPageDTO();
            response.EasternConference = easternTeam;
            response.WesternConference = westernTeam;

            return response; //response is of type T = IndexPageDTO
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamProfileDTO>> Get(int teamId)
        {
            var team = await _dbContext.Teams.Where(x => x.TeamId == teamId)
                .Include(x => x.TeamGames).ThenInclude(x => x.Game) //bring info of related model(in this case TeamGames)
                .Include(x => x.TeamPlayers).ThenInclude(x => x.Person) // ---||--- (BasketBallPlayer)
                .FirstOrDefaultAsync();//getting the team if data matches

            if (team == null) { return NotFound(); }

            team.TeamPlayers = team.TeamPlayers.OrderBy(x => x.Order).ToList(); //ordering players by Starting position 

            var model = new TeamProfileDTO(); //with this model I am making it easier mapping properties in TeamProfile.
            model.Team = team;
            model.Games = team.TeamGames.Select(x => x.Game).ToList();
            model.Players = team.TeamPlayers.Select(x =>
                new Person
                {
                    FullName = x.Person.FullName,
                    Picture = x.Person.Picture,
                    Position = x.Position,
                    Id = x.PersonId,
                }).ToList();

            return model;
        }

        [HttpPost]
        //Bellow I am creating an endpoint that is responding to HttpPost
        public async Task<ActionResult<int>> Post(Team team)
        {
            if (!string.IsNullOrWhiteSpace(team.TeamLogo))
            {
                var teamLogo = Convert.FromBase64String(team.TeamLogo);
                team.TeamLogo = await _fileStorageService.SaveFile(teamLogo, "png", containerName);
            }
            if (team.TeamPlayers !=null)
            {
                for (int i = 0; i < team.TeamPlayers.Count; i++)
                {
                    team.TeamPlayers[i].Order = i + 1;
                }
            }
            _dbContext.Add(team);
            await _dbContext.SaveChangesAsync();
            return team.TeamId; //just to signify that it was created
        }
    }
}
