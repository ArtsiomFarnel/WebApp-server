using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebApp.Application.Models.DataTransferObjects.Incoming.Users
{
    public class UserChangePasswordDto
    {
        [Required(ErrorMessage = "Old password is required field.")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "New password is required field.")]
        public string NewPassword { get; set; }
    }
}
