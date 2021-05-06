using BasketBall.Client.Helpers;
using BasketBall.Client.Repositories.Interfaces;
using BasketBall.Client.Services.Interfaces;
using BasketBall.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketBall.Client.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IHttpService _httpService;
        private readonly string url = "api/users";

        public UserRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<PaginatedResponse<List<UserDTO>>> GetUsers(PaginationDTO paginationDTO)
        {
            return await _httpService.GetHelper<List<UserDTO>>(url, paginationDTO);
        }

        public async Task<List<RoleDTO>> GetRoles()
        {
            return await _httpService.GetHelper<List<RoleDTO>>($"{url}/roles");
        }

        public async Task AssignRole(EditRoleDTO editRole)
        {
            var response = await _httpService.Post($"{url}/assignRole", editRole);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task RemoveRole(EditRoleDTO editRole)
        {
            var response = await _httpService.Post($"{url}/removeRole", editRole);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}
