using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using System.Linq.Expressions;

namespace Repository
{
    public class PeopleRepository : IPeopleRepository
    {
        private readonly AspNetDbContext _context;
        private readonly ILogger<PeopleRepository> _logger;

        public PeopleRepository(AspNetDbContext context, ILogger<PeopleRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Person> AddPerson(Person person)
        {
            await _context.Persons.AddAsync(person);

            await _context.SaveChangesAsync();
            return person;
        }

        public async Task<bool> DeletePerson(Guid PersonId)
        {
            Person person = await _context.Persons.FirstAsync(temp => temp.PersonID == PersonId);

            _context.Persons.Remove(person);

            int rowsAfected = await _context.SaveChangesAsync();

            return rowsAfected > 0;
        }

        public async Task<List<Person>> GetFilterdPeople(Expression<Func<Person, bool>> predicate)
        {
            _logger.LogInformation("GetFilteredPeople of the respository");
            return await _context.Persons.Include("Country").Where(predicate).ToListAsync();
        }

        public async Task<Person?> GetPersonById(Guid? PersonId)
        {
            return await _context.Persons.FirstOrDefaultAsync(temp => temp.PersonID == PersonId);
        }

        public async Task<List<Person>> GetPersonList()
        {
            return await _context.Persons.Include("Country").ToListAsync();
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            Person? matchingPerson = await _context.Persons.FirstOrDefaultAsync(temp => temp.PersonID == person.PersonID);

            if(matchingPerson != null)
            {
                matchingPerson.PersonGender = person.PersonGender;
                matchingPerson.PersonEmail = person.PersonEmail;
                matchingPerson.PersonAddress = person.PersonAddress;
                matchingPerson.PersonName = person.PersonName;
                matchingPerson.CountryID = person.CountryID;
                matchingPerson.ReceiveNewsLetters = person.ReceiveNewsLetters;
                matchingPerson.DateOfBirth = person.DateOfBirth;

                await _context.SaveChangesAsync();

                return matchingPerson;
            }
            

            return person;

        }
    }
}