using BasketBall.Client.Helpers;
using BasketBall.Client.Repositories.Interfaces;
using BasketBall.Client.Services.Interfaces;
using BasketBall.Shared.DTOs;
using BasketBall.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketBall.Client.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly IHttpService _httpService;
        private string url = "api/teams"; //endpoint in which my TeamsController is located

        public TeamRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }
        //--------------reusing method from Helper!!
        
        public async Task<IndexPageDTO> GetIndexPageDTO()
        {

            return await _httpService.GetHelper<IndexPageDTO>(url);
        }
        public async Task<TeamProfileDTO> GetTeamProfileDTO(int teamId)
        {
            return await _httpService.GetHelper<TeamProfileDTO>($"{url}/{teamId}");
        }
        //-----------
        public async Task<int> CreateTeam(Team team)
        {
            var response = await _httpService.Post<Team, int>(url, team);
            //if the response is not successfull throw exeption and display error message with body
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }
    }
}
