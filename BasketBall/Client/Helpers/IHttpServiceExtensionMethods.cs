using BasketBall.Client.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketBall.Client.Helpers
{
    public static class IHttpServiceExtensionMethods
    {
        //Reusing this code in Game/Person/Team Repositories (6 references) Task<T> can return a single item or List
        public static async Task<T> GetHelper<T>(this IHttpService httpService, string url)
        {
            var response = await httpService.Get<T>(url);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

    }
}
