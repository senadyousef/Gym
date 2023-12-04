using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Waffer.Server.Controllers.Abstractions
{
    [Route("[controller]/[action]")]
    public abstract class BaseWebController : Controller
    {
    }
}
