using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Boilerplate.Application.DTOs.User;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Boilerplate.Api.Controllers
{
    [Route("user")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class UserViewController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ICurrentUser _currentCustomer;
        private readonly IHttpContextAccessor _HttpContextAccessor;

        public UserViewController(IHttpContextAccessor HttpContextAccessor, IConfiguration configuration, 
            ICurrentUser currentCustomer)
        {
            _configuration = configuration;
            _currentCustomer = currentCustomer;
            _HttpContextAccessor = HttpContextAccessor;

        }
        
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }
        
        [Route("create")]
        public IActionResult Create()
        {
            ViewBag.ShowProject = false;

            if (_currentCustomer.Role == "SuperAdmin")
            {
                ViewBag.ShowProject = true;
                ViewBag.Roles = new List<string>() { "Owner"};
            }
            else if (_currentCustomer.Role == "Owner")
            {
                ViewBag.Roles = new List<string>() { "OwnerUser", "ContractorAdmin" };
            }
            else if (_currentCustomer.Role == "ContractorAdmin")
            {
                ViewBag.Roles = new List<string>() { "ContractorUser", "SubContractorUser" };

            }
            return View();
        }
        
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromQuery]string key)
        {
            //var key = int.Parse(form.FirstOrDefault().Value.ToString());

            using var Httpclient = new HttpClient();
            Httpclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _currentCustomer.Token);
             Client client = new Client(_configuration.GetValue<string>("HostUrl"), Httpclient);
            //
            await client.UserDELETEAsync(int.Parse(key));

            return Ok();
        }

        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(int Id)
        {
            ViewBag.ShowProject = false;

            if (_currentCustomer.Role == "SuperAdmin")
            {
                ViewBag.ShowProject = true;
                ViewBag.Roles = new List<string>() { "Owner"};
            }
            else if (_currentCustomer.Role == "Owner")
            {
                ViewBag.Roles = new List<string>() { "Coach" };

            }
           
            using var Httpclient = new HttpClient();
            Httpclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _currentCustomer.Token);
            Client client = new Client(_configuration.GetValue<string>("HostUrl"), Httpclient);

            var UserData = await client.UserGET2Async(Id);

            return View((object?)UserData);
        }

        [Route("logout")]
        public async Task<IActionResult> LogOut()
        {
            _HttpContextAccessor.HttpContext.Session.Clear();
            _HttpContextAccessor.HttpContext.Session.Remove("Id");
            _HttpContextAccessor.HttpContext.Session.Remove("Email");
            _HttpContextAccessor.HttpContext.Session.Remove("Token");
            _HttpContextAccessor.HttpContext.Session.Remove("ExpDate");
            _HttpContextAccessor.HttpContext.Session.Remove("Role");
            return RedirectToAction("login", "account");
        }
        
        [Route("Profile")]
        public async Task<IActionResult> Profile()
        {
            using var Httpclient = new HttpClient();
            Httpclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _currentCustomer.Token);
            Client client = new Client(_configuration.GetValue<string>("HostUrl"), Httpclient);

            var UserData = await client.UserGET2Async(int.Parse(_currentCustomer.Id));

            return View((object?)UserData);
        }
      
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody]CreateUserDto create)
        {
            if (_currentCustomer.Role != "SuperAdmin")
            {
                //create.ProjectId = int.Parse((string)_currentCustomer.ProjectId);
            }
            using var Httpclient = new HttpClient();
            Httpclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _currentCustomer.Token);
            Client client = new Client(_configuration.GetValue<string>("HostUrl"), Httpclient);

            var UserData = await client.UserPOSTAsync(create);

            return Ok();
        }
        
        [Route("checkemail/{email}")]
        public async Task<bool> checkemailAsync(string email)
        {
            using var Httpclient = new HttpClient();
            Httpclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _currentCustomer.Token);
            Client client = new Client(_configuration.GetValue<string>("HostUrl"), Httpclient);

            var UserData = await client.UserGETAsync(email: email, currentPage: 1, pageSize: 1);
            if (UserData.TotalItems > 0)
                return false;

            return true; 
        }
       
        [HttpPost]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(CreateUserDto create,int id)
        {
            if (_currentCustomer.Role != "SuperAdmin")
            {
                //create.ProjectId = int.Parse((string)_currentCustomer.ProjectId);
            }

            using var Httpclient = new HttpClient();
            Httpclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _currentCustomer.Token);
            Client client = new Client(_configuration.GetValue<string>("HostUrl"), Httpclient);
            await client.UpdateuserAsync(create);


            return Ok();
        }
        
        [Route("GetUserList")]
        public async Task<IActionResult> GetUserListAsync(DataSourceLoadOptions loadOptions)
        {
            try
            {
                using var Httpclient = new HttpClient();
                Httpclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _currentCustomer.Token);
                Client client = new Client(_configuration.GetValue<string>("HostUrl"), Httpclient);

                int currentPage = 1;
                if (loadOptions.Skip > 0)
                    currentPage = (loadOptions.Skip / loadOptions.Take) == 1 ? 2 : (loadOptions.Skip / loadOptions.Take) + 1;

                string email = null;
                string role=null;

                if (loadOptions.Filter != null)
                {
                    try
                    {
                        var filterName = loadOptions.Filter[0] as string;
                        var filterValue = loadOptions.Filter[2] as string;
                        if (!String.IsNullOrEmpty(filterValue) && !String.IsNullOrEmpty(filterValue))
                        {
                            if (filterName == "email")
                                email = filterValue;
                            else if (filterName == "role")
                                role = filterValue;
                        }
                    }
                    catch
                    {

                    }
                }


                var UserData = await client.UserGETAsync(pageSize: loadOptions.Take, currentPage: currentPage,email:email,role:role);


                return Json(UserData);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        [Route("GetAssignUserList")]
        public async Task<IActionResult> GetAssignUserListAsync(DataSourceLoadOptions loadOptions)
        {
            try
            {
                using var Httpclient = new HttpClient();
                Httpclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _currentCustomer.Token);
                Client client = new Client(_configuration.GetValue<string>("HostUrl"), Httpclient);

                int currentPage = 1;
                if (loadOptions.Skip > 0)
                    currentPage = (loadOptions.Skip / loadOptions.Take) == 1 ? 2 : (loadOptions.Skip / loadOptions.Take) + 1;

                string role = null;

                if (_currentCustomer.Role == "Owner")
                    role = "Coach";
                
                //senad subcontractor.. this feature is not used any more?

                var UserData = await client.UserGETAsync(pageSize: loadOptions.Take, currentPage: currentPage, role: role);


                return Json(UserData);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
