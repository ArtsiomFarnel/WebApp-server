using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Application.Models.DataTransferObjects.Incoming.Users
{
    public class UserChangeProfileDto
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImagePath { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
