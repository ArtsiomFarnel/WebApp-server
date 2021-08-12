using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Models.DataTransferObjects.Outgoing.Users
{
    public class UserFullInfoDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImagePath { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
    }
}
