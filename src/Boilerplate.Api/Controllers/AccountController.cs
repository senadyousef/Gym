using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Boilerplate.Application.DTOs.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Boilerplate.Api.Controllers
{
    [Route("account")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        public AccountController(IConfiguration configuration, IHttpContextAccessor HttpContextAccessor)
        {
            _configuration = configuration;
            _HttpContextAccessor = HttpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            try
            {
                using var Httpclient = new HttpClient();
                Client client = new Client(_configuration.GetValue<string>("HostUrl"), Httpclient);
                var result = await client.AuthenticateAsync(login);

                if (result == null)
                    return View("Login", "Error");

                Httpclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Token);
                client = new Client(_configuration.GetValue<string>("HostUrl"), Httpclient);
                var UserData = await client.UserGETAsync(login.Email, null, 1, 1);

                _HttpContextAccessor.HttpContext.Session.SetString("Id", UserData.Result.FirstOrDefault().Id.ToString());
                _HttpContextAccessor.HttpContext.Session.SetString("Email", UserData.Result.FirstOrDefault().Email.ToString());
                _HttpContextAccessor.HttpContext.Session.SetString("Token", result.Token);
                _HttpContextAccessor.HttpContext.Session.SetString("ExpDate", result.ExpDate.ToString());
                _HttpContextAccessor.HttpContext.Session.SetString("Role", UserData.Result.FirstOrDefault().Role);
               


                return RedirectToAction("Index", "HomeView");

            }
            catch (Exception ex)
            {
                return View("Login", "Error");
            }

        }
    }
}
