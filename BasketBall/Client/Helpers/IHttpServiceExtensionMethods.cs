using BasketBall.Client.Services.Interfaces;
using BasketBall.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketBall.Client.Helpers
{
    public static class IHttpServiceExtensionMethods
    {
        //Reusing this code in Game/Person/Team Repositories (6 references) Task<T> can return a single item or List
        public static async Task<T> GetHelper<T>(this IHttpService httpService, string url, bool includeToken = true)
        {
            var response = await httpService.Get<T>(url, includeToken);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }
        public static async Task<PaginatedResponse<T>> GetHelper<T>(this IHttpService httpService, string url,
           PaginationDTO paginationDTO, bool includeToken = true)
        {
            string newURL = "";
            if (url.Contains("?")) //if url has "?" it means it already has queryStrings
            {
                newURL = $"{url}&page={paginationDTO.Page}&recordsPerPage={paginationDTO.RecordsPerPage}";
            }
            else
            {
                newURL = $"{url}?page={paginationDTO.Page}&recordsPerPage={paginationDTO.RecordsPerPage}";
            }

            var httpResponse = await httpService.Get<T>(newURL, includeToken);
            var totalAmountOfPages = int.Parse(httpResponse.HttpResponseMessage.Headers.GetValues("totalAmountOfPages").FirstOrDefault());
            var paginatedResponse = new PaginatedResponse<T>
            {
                Response = httpResponse.Response,
                TotalAmountOfPages = totalAmountOfPages
            };
            return paginatedResponse;
        }
    }
}
