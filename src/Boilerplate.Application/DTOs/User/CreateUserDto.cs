using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Boilerplate.Application.DTOs.User
{
    public class CreateUserDto
    {
        public int Id { get; set; } 
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } 
        public string Password { get; set; } 
        public string Role { get; set; }  
        public string NameEn { get; set; } 
        public string NameAr { get; set; } 
        public string Gender { get; set; }
        public DateTime BOD { get; set; }
        public int GymId { get; set; }
        public string MembershipStatus { get; set; }
        public DateTime? MembershipExpDate { get; set; }
        public string MobilePhone { get; set; }   
        public UploadPhotoRequest UploadRequests { get; set; } 
        public string PhotoUri { get; set; } 
    }
}
