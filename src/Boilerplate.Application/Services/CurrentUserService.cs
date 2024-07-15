using Boilerplate.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            if(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                UserId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
                Role = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role).Value; 
            }
            if(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.System) !=null)
            Project = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.System).Value;


        }

        public string UserId { get; } 
        public string Role { get; }  
        public string Project { get; }
    }
}
