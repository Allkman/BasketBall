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
        Task<IndexPageDTO> GetIndexPageDTO();
        Task<TeamProfileDTO> GetTeamProfileDTO(int teamId);
    }
}
