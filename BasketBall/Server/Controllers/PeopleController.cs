using AutoMapper;
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
        private readonly IMapper _mapper;
        private int searchResultLimit = 5;

        public PeopleController(ApplicationDbContext dbContext, 
            IFileStorageService fileStorageService,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
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
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> Get(int personId)
        {
            var person = await _dbContext.People.FirstOrDefaultAsync(x => x.Id == personId);
            if (person == null)
            {
                return NotFound();
            }
            return person;
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
        [HttpPut]
        public async Task<ActionResult> Put(Person person)
        {
            //personDB == person, but its from database
            //the reason for this is to use AutoMapper to map the values of personDB and person (could be done with [HttpPatch]
            var personDB = await _dbContext.People.FirstOrDefaultAsync(x => x.Id == person.Id);
            if (personDB == null)
            {
                return NotFound();
            }
            //change Picture only if new file was selected. Display the current picture while in edit page
            personDB = _mapper.Map(person, personDB);
            if (!string.IsNullOrWhiteSpace(person.Picture))
            {
                var personPicture = Convert.FromBase64String(person.Picture);
                personDB.Picture = await _fileStorageService.EditFile(personPicture, ".png", "people", personDB.Picture);
            }
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int personId)
        {
            var person = await _dbContext.People.FirstOrDefaultAsync(x => x.Id == personId);
            if (person == null)
            {
                return NotFound();
            }
            _dbContext.Remove(person);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}

