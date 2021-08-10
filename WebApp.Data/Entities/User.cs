using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebApp.Data.Entities
{
    public class User : IdentityUser
    {
        [MaxLength(20, ErrorMessage = "Maximum length for the Name is 20 characters")]
        public string FirstName { get; set; }

        [MaxLength(20, ErrorMessage = "Maximum length for the Name is 20 characters")]
        public string LastName { get; set; }
        public string ImagePath { get; set; }
    }
}
