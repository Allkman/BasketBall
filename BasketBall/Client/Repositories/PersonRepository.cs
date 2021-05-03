using BasketBall.Client.Helpers;
using BasketBall.Client.Repositories.Interfaces;
using BasketBall.Client.Services.Interfaces;
using BasketBall.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketBall.Client.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IHttpService _httpService;
        private readonly string url = "api/people"; //endpoint in which my PeopleController is located

        public PersonRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }
        public async Task<Person> GetPersonById(int personId)
        {
            return await _httpService.GetHelper<Person>($"{url}/{personId}");
        }
        public async Task<List<Person>> GetPeople()
        {
            var response = await _httpService.Get<List<Person>>(url);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }
        public async Task<List<Person>> GetPeopleByName(string name)
        {
            var response = await _httpService.Get<List<Person>>($"{url}/search/{name}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }
        public async Task CreatePerson(Person person)
        {
            var response = await _httpService.Post(url, person);
            //if the response is not successfull throw exeption and display error message with body
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
        public async Task UpdatePerson(Person person)
        {
            var response = await _httpService.Put(url, person);
            //if the response is not successfull throw exeption and display error message with body
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    } 
}
