using Boilerplate.Api;
using Microsoft.AspNetCore.Http;

namespace Boilerplate.Api
{
    public class CurrentCustomer : ICurrentUser
    {
        private readonly IHttpContextAccessor _HttpContextAccessor;
        public CurrentCustomer(IHttpContextAccessor HttpContextAccessor)
        {
            _HttpContextAccessor = HttpContextAccessor;
            if (_HttpContextAccessor.HttpContext != null)
            {
                Token = _HttpContextAccessor.HttpContext.Session.GetString("Token");
                Email = _HttpContextAccessor.HttpContext.Session.GetString("Email");
                Id = _HttpContextAccessor.HttpContext.Session.GetString("Id");
                Role = _HttpContextAccessor.HttpContext.Session.GetString("Role");
                ProjectId = _HttpContextAccessor.HttpContext.Session.GetString("ProjectId");
            }
        }
        public string Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }

        public string ProjectId { get; set; }

    }
}
