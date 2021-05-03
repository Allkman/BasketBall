using BasketBall.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketBall.Client.Repositories.Interfaces
{
    public interface IPersonRepository
    {
        Task CreatePerson(Person person);
        Task DeletePerson(int personId);
        Task<List<Person>> GetPeople();
        Task<List<Person>> GetPeopleByName(string name);
        Task<Person> GetPersonById(int personId);
        Task UpdatePerson(Person person);
    }
}
