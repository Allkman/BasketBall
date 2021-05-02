using BasketBall.Server.Data;
using BasketBall.Server.Services.Interfaces;
using BasketBall.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasketBall.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")] //this controller is in ..../api/people
    public class PeopleController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IFileStorageService _fileStorageService;
        private int searchResultLimit = 5;

        public PeopleController(ApplicationDbContext dbContext, IFileStorageService fileStorageService)
        {
            _dbContext = dbContext;
            _fileStorageService = fileStorageService;
        }
        [HttpGet("search/{searchText}")] //api/people/search/[whatever the user types in...]
        public async Task<ActionResult<List<Person>>> FilterByName(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return new List<Person>();
            }
            return await _dbContext.People.Where(x => x.FullName.Contains(searchText))
                .Take(searchResultLimit)
                .ToListAsync();
        }
        [HttpGet]
        public async Task<ActionResult<List<Person>>> Get()
        {
            return await _dbContext.People.ToListAsync();
        }
        [HttpPost]
        //Bellow I am creating an endpoint that is responding to HttpPost
        public async Task<ActionResult<int>> Post(Person person)
        {
            if (!string.IsNullOrWhiteSpace(person.Picture))
            {
                var personPicture = Convert.FromBase64String(person.Picture);
                person.Picture = await _fileStorageService.SaveFile(personPicture, ".png", "people");
            }
            _dbContext.Add(person);
            await _dbContext.SaveChangesAsync();
            return person.Id; //just to signify that it was created
        }
    }
}

