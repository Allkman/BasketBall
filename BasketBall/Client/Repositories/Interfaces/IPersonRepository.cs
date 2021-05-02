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
        Task<List<Person>> GetPeople();
        Task<List<Person>> GetPeopleByName(string name);
    }
}
