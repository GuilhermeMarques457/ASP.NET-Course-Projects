using Entities;
using System;
using System.Collections.Generic;

namespace ContactsManager.Core.DTO.CountryDTO
{
    /// <summary>
    /// DTO class for adding new country
    /// </summary>
    public class CountryAddRequest
    {
        public string? CountryName { get; set; }

        public Country ToCountry()
        {
            return new Country()
            {
                CountryName = CountryName,
            };
        }
    }
}
