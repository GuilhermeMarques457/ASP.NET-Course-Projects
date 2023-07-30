using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Core.DTO.PersonDTO
{
    /// <summary>
    /// Represts DTO class that is used as return type of most methos of Persons Service
    /// </summary>
    public class PersonResponse
    {
        public Guid PersonId { get; set; }
        public string? PersonName { get; set; }
        public string? PersonEmail { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PersonGender { get; set; }
        public Guid? CountryID { get; set; }
        public string? CountryName { get; set; }
        public string? PersonAddress { get; set; }
        public bool? ReceiveNewsLetters { get; set; }
        public double? PersonAge { get; set; }

        public override bool Equals(object? obj)
        {
            //var response = (obj == null) ? false : Equals((PersonResponse)obj);

            if (obj == null) return false;
            if (obj.GetType() != typeof(PersonResponse)) return false;

            PersonResponse person = (PersonResponse)obj;
            return PersonId == person.PersonId
                && PersonGender == person.PersonGender
                && PersonAddress == person.PersonAddress
                && PersonEmail == person.PersonEmail
                && CountryID == person.CountryID
                && ReceiveNewsLetters == person.ReceiveNewsLetters
                && DateOfBirth == person.DateOfBirth;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"Person ID: {PersonId}, Person Name: {PersonName}, Email: {PersonEmail}, Age: {PersonAge}, Country Name: {CountryName}, Date of birth: {DateOfBirth?.ToString("dd MMM yyyy")}, Address: {PersonAddress}, Country ID: {CountryID}";
        }

        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                PersonId = PersonId,
                PersonAddress = PersonAddress,
                ReceiveNewsLetters = ReceiveNewsLetters,
                CountryID = CountryID,
                DateOfBirth = DateOfBirth,
                PersonEmail = PersonEmail,
                PersonGender = (GenderOptions)Enum.Parse(typeof(GenderOptions), PersonGender, true),
                PersonName = PersonName,

            };
        }
    }

    /// <summary>
    /// Hey this is a extension method that I would like
    /// to include in the (this.person) class without modifing it,
    /// that's the meaning of this extension method
    /// </summary>

    public static class PersonExtensions
    {
        public static PersonResponse ToPersonResponse(this Person person)
        {
            //person => PersonResponse
            return new PersonResponse()
            {
                PersonId = person.PersonID,
                PersonName = person.PersonName,
                PersonEmail = person.PersonEmail,
                DateOfBirth = person.DateOfBirth,
                CountryID = person.CountryID,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                PersonGender = person.PersonGender,
                PersonAddress = person.PersonAddress,
                PersonAge = person.DateOfBirth != null ? DateTime.Now.Year - person.DateOfBirth.Value.Year : null,
                CountryName = person.Country?.CountryName

            };
        }
    }
}
