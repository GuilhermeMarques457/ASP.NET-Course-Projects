using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Core.DTO.PersonDTO
{
    /// <summary>
    /// Represetns DTO class that contains the person 
    /// details to update
    /// </summary>
    public class PersonUpdateRequest
    {
        [Required]
        public Guid PersonId { get; set; }

        [Required(ErrorMessage = "Person Name can't be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Person Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email value should be a valid email")]
        public string? PersonEmail { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? PersonGender { get; set; }
        public Guid? CountryID { get; set; }
        public string? PersonAddress { get; set; }
        public bool? ReceiveNewsLetters { get; set; }

        /// <summary>
        /// Converts the current object to Person object
        /// </summary>
        /// <returns>Person object</returns>
        public Person ToPerson()
        {
            return new Person()
            {
                PersonID = PersonId,
                PersonName = PersonName,
                PersonEmail = PersonEmail,
                DateOfBirth = DateOfBirth,
                PersonGender = PersonGender.ToString(),
                CountryID = CountryID,
                PersonAddress = PersonAddress,
                ReceiveNewsLetters = ReceiveNewsLetters,
            };
        }
    }
}
