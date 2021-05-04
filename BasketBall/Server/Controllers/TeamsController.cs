using AutoMapper;
using BasketBall.Server.Data;
using BasketBall.Server.Helpers;
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
        private readonly IMapper _mapper;
        private string containerName = "teams";
        private string fileFormat = ".png";

        public TeamsController(ApplicationDbContext dbContext, 
            IFileStorageService fileStorageService,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
        }
        [HttpGet] //specific method to display two lists in IndexPage ("/")
        public async Task<ActionResult<IndexPageDTO>> Get()
        {
            var limit = 5; //there are total 15 teams per conference = 3 pages
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
        public async Task<ActionResult<TeamProfileDTO>> Get(int id)
        {
            var team = await _dbContext.Teams.Where(x => x.TeamId == id)
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
        [HttpPost("filter")]
        public async Task<ActionResult<List<Team>>> Filter(FilterTeamsDTO filterTeamsDTO)
        {
            var teamsQueryable = _dbContext.Teams.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterTeamsDTO.TeamName))
            {
                teamsQueryable = teamsQueryable
                    .Where(x => x.TeamName.Contains(filterTeamsDTO.TeamName));
            }

            if (filterTeamsDTO.EasternConference)
            {
                teamsQueryable = teamsQueryable.Where(x => x.EasternConference);
            }
            if (filterTeamsDTO.WesternConference)
            {
                teamsQueryable = teamsQueryable.Where(x => x.WesternConference);
            }


            if (filterTeamsDTO.GameId != 0)
            {
                teamsQueryable = teamsQueryable
                    .Where(x => x.TeamGames.Select(y => y.GameId)
                    .Contains(filterTeamsDTO.GameId));
            }

            await HttpContext.InsertPaginationParametersInResponse(teamsQueryable,
                filterTeamsDTO.RecordsPerPage);

            var teams = await teamsQueryable.Paginate(filterTeamsDTO.Pagination).ToListAsync();

            return teams;
        }
        [HttpGet("update/{id}")]
        public async Task<ActionResult<TeamUpdateDTO>> PutGet(int id)
        {
            
            var teamActionResult = await Get(id);
            if (teamActionResult.Result is NotFoundResult)
            {
                return NotFound();
            }
            var teamProfileDTO = teamActionResult.Value;
            var selectedGamesIds = teamProfileDTO.Games.Select(x => x.GameId).ToList();
            var notSelectedGames = await _dbContext.Games
                .Where(x => !selectedGamesIds.Contains(x.GameId))
                .ToListAsync();
            var model = new TeamUpdateDTO();
            model.Team = teamProfileDTO.Team;
            model.SelectedGames = teamProfileDTO.Games;
            model.NotSelectedGames = notSelectedGames;
            model.TeamPlayers = teamProfileDTO.Players;

            return model;
        }
        [HttpPost]
        //Bellow I am creating an endpoint that is responding to HttpPost
        public async Task<ActionResult<int>> Post(Team team)
        {
            if (!string.IsNullOrWhiteSpace(team.TeamLogo))
            {
                var teamLogo = Convert.FromBase64String(team.TeamLogo);
                team.TeamLogo = await _fileStorageService.SaveFile(teamLogo, fileFormat, containerName);
            }
            //to specify the order in which players appear on team
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
        [HttpPut]
        public async Task<ActionResult> Put(Team team)
        {
            //same logic like in PeopleController

            var teamDB = await _dbContext.Teams.FirstOrDefaultAsync(x => x.TeamId == team.TeamId);
            if (teamDB == null)
            {
                return NotFound();
            }
            //change Picture only if new file was selected. Display the current picture while in edit page
            teamDB = _mapper.Map(team, teamDB);
            if (!string.IsNullOrWhiteSpace(team.TeamLogo))
            {
                var teamLogo = Convert.FromBase64String(team.TeamLogo);
                teamDB.TeamLogo = await _fileStorageService.EditFile(teamLogo, fileFormat, containerName, teamDB.TeamLogo);
            }

            await _dbContext.Database.ExecuteSqlInterpolatedAsync($"delete from TeamPlayers where TeamId = {team.TeamId}; delete from TeamGames where TeamId = {team.TeamId}");
            if (team.TeamPlayers != null)
            {
                for (int i = 0; i < team.TeamPlayers.Count; i++)
                {
                    team.TeamPlayers[i].Order = i + 1;
                }
            }

            teamDB.TeamPlayers = team.TeamPlayers;
            teamDB.TeamGames = team.TeamGames;

            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int teamId)
        {
            var team = await _dbContext.Teams.FirstOrDefaultAsync(x => x.TeamId == teamId);
            if (team == null)
            {
                return NotFound();
            }
            _dbContext.Remove(team);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
