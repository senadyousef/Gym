using Boilerplate.Domain.Entities;
using System.Collections.Generic;

namespace Boilerplate.Application.Filters
{
    public class GetUsersFilter : PaginationInfoFilter
    {
        public string Email { get; set; }
        public string Role { get; set; } 

    }
}
