using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Waffer.Server.Controllers.Abstractions
{
    [Authorize(AuthenticationSchemes = "Identity.Application, Bearer")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class BaseApiController : Controller
    {
    }
}
