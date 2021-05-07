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

            return await _httpService.GetHelper<IndexPageDTO>(url, includeToken: false);
        }
       
        public async Task<TeamProfileDTO> GetTeamProfileDTO(int id)
        {
            return await _httpService.GetHelper<TeamProfileDTO>($"{url}/{id}", includeToken: false);
        }
        //-----------
        public async Task<TeamUpdateDTO> GetTeamForUpdate(int id)
        {
            return await _httpService.GetHelper<TeamUpdateDTO>($"{url}/update/{id}");
        }
        public async Task<PaginatedResponse<List<Team>>> GetTeamsFiltered(FilterTeamsDTO filterTeamsDTO)
        {
            //sending FilterTeamsDTO to the server and receiving List<Team> 
            var responseHTTP = await _httpService.Post<FilterTeamsDTO, List<Team>>($"{url}/filter", filterTeamsDTO, includeToken: false);
            var totalAmountOfPages = int.Parse(responseHTTP.HttpResponseMessage.Headers.GetValues("totalAmountOfPages").FirstOrDefault());
            var paginatedResponse = new PaginatedResponse<List<Team>>()
            {
                Response = responseHTTP.Response,
                TotalAmountOfPages = totalAmountOfPages,
            };
            return paginatedResponse;
        }
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
        public async Task UpdateTeam(Team team)
        {
            var response = await _httpService.Put(url, team);
            //if the response is not successfull throw exeption and display error message with body
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
        public async Task DeleteTeam(int id)
        {
            var response = await _httpService.Delete($"{url}/{id}");

            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
