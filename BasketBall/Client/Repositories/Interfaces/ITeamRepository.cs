using BasketBall.Shared.DTOs;
using BasketBall.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketBall.Client.Repositories.Interfaces
{
    public interface ITeamRepository
    {
        Task<int> CreateTeam(Team team);
        Task DeleteTeam(int teamId);
        Task<IndexPageDTO> GetIndexPageDTO();
        Task<TeamUpdateDTO> GetTeamForUpdate(int teamId);
        Task<TeamProfileDTO> GetTeamProfileDTO(int teamId);
        Task<PaginatedResponse<List<Team>>> GetTeamsFiltered(FilterTeamsDTO filterTeamsDTO);
        Task UpdateTeam(Team team);
    }
}
