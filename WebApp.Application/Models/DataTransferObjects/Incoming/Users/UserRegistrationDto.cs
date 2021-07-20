using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebApp.Application.Models.DataTransferObjects.Incoming.Users
{
    public class UserRegistrationDto
    {
        [MaxLength(20, ErrorMessage = "Maximum length for the Name is 20 characters")]
        public string FirstName { get; set; }

        [MaxLength(20, ErrorMessage = "Maximum length for the Name is 20 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }        

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string PhoneNumber { get; set; }

        public IEnumerable<string> Roles { get; set; }

    }
}
