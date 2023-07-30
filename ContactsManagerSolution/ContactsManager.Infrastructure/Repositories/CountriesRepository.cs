using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly AspNetDbContext _context;

        public CountriesRepository(AspNetDbContext context)
        {
            _context = context;
        }

        public async Task<Country> AddCountry(Country country)
        {
            await _context.Countries.AddAsync(country);
            await _context.SaveChangesAsync();

            return country;
        }

        public async Task<List<Country>> GetAllCountries()
        {
            List<Country> countries = new List<Country>();
            countries = await _context.Countries.ToListAsync();
            return countries;
            
        }

        public async Task<Country?> GetCountryById(Guid? countryID)
        {
            return await _context.Countries.FirstOrDefaultAsync(c => c.CountryID == countryID);
        }

        public async Task<Country?> GetCountryByName(string countryName)
        {
            return await _context.Countries.FirstOrDefaultAsync(c => c.CountryName == countryName);
        }
    }
}
