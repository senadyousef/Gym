using Boilerplate.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Boilerplate.Application.Filters
{
    public class GetUsersFilter : PaginationInfoFilter
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public int GymId { get; set; }
        public string NameEn { get; set; } 
        public string NameAr { get; set; } 
        public string MembershipStatus { get; set; } 
        public DateTime? MembershipExpDate { get; set; } 

    }
}
