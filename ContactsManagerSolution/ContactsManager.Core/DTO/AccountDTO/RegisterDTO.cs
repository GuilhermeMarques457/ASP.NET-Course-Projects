using ContactsManager.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Core.DTO.AccountDTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email must be in a proper email address format")]
        [Remote("EmailValidator", controller: "Account", ErrorMessage = "Email is already in use")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Person Name can't be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Password can't be blank")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm Password can't be blank")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Phone Number can't be blank")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number must contain only numbers")]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }
        public bool RememberMe { get; set; }
        public UserTypeOptions UserType { get; set; } = UserTypeOptions.User;

    }
}
