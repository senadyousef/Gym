using Boilerplate.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Boilerplate.Application.DTOs.User
{
    public class GetUserDto
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public DateTime BOD { get; set; }
        public string MembershipStatus { get; set; }
        public DateTime MembershipExpDate { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string MobilePhone { get; set; } 
        public string RefreshToken { get; set; }
        public string PhotoUri { get; set; }

    }

    public class GetUserExtendedDto
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public DateTime BOD { get; set; }
        public string MembershipStatus { get; set; }
        public DateTime MembershipExpDate { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string MobilePhone { get; set; } 
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string PhotoUri { get; set; } 

    }

}
