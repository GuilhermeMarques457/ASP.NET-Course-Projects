using Entities;
using System;
using System.Collections.Generic;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class that return type for most countries methods
    /// </summary>
    public class CountryResponse
    {
        public Guid CountryId { get; set; }
        public string? CountryName { get; set; }

        //It compares the current object to another object
        //and returns true if both are same; otherwise returns false
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(CountryResponse))
            {
                return false;
            }

            CountryResponse countryResponse = (CountryResponse)obj;

            return this.CountryId == countryResponse.CountryId 
                && this.CountryName == countryResponse.CountryName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class CountryExtensions
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse()
            {
                CountryId = country.CountryID,
                CountryName = country.CountryName
            };
        }
    }
}
