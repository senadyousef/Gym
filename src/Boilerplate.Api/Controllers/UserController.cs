using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Boilerplate.Application.DTOs;
using Boilerplate.Application.DTOs.Auth;
using Boilerplate.Application.DTOs.User;
using Boilerplate.Application.Filters;
using Boilerplate.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ISession = Boilerplate.Domain.Auth.Interfaces.ISession;
using Net.Codecrete.QrCodeGenerator;
using System.IO;
using System.Text;
using System.Drawing;
using QRCoder;
using System.Drawing.Imaging;
using Boilerplate.Application.Services;
using Boilerplate.Domain.Auth;

namespace Boilerplate.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ICurrentUser _currentCustomer;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly ISession _session;
        private ICurrentUserService _currentUserService;

        public UserController(IUserService userService, IAuthService authService, ISession session, ICurrentUserService currentUserService, IConfiguration configuration, ICurrentUser currentUser)
        {
            _userService = userService;
            _authService = authService;
            _session = session;
            _currentUserService = currentUserService;
            _configuration = configuration;
            _currentCustomer = currentUser;
        }

        /// <summary>
        /// Authenticates the user and returns the token information.
        /// </summary>
        /// <param name="loginInfo">Email and password information</param>
        /// <returns>Token information</returns>
        [HttpPost]
        [Route("authenticate")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(JwtDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Authenticate([FromBody] LoginDto loginInfo)
        {
            var user = await _userService.Authenticate(loginInfo.EmailOrMobilePhone, loginInfo.Password);
            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            else
            {
                if (user.Role != "SuperAdmin")
                {
                    if (user.MembershipStatus == "Not Active" || user.MembershipExpDate.Date < DateTime.Now.Date)
                    {
                        return BadRequest(new { message = "Your Membership Expired" });
                    }
                }
            }
            return Ok(await _authService.GenerateTokenAsync(user));
        }


        [HttpPost]
        [Route("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(JwtDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshToken([FromBody] JwtDto jwtDto)
        {
            var token = await _authService.RefreshToken(jwtDto);
            if (token == null)
            {
                return BadRequest(new { message = "Cannot generate refresh token." });
            }
            return Ok(token);
        }

        /// <summary>
        /// Returns all users in the database
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(PaginatedList<GetUserDto>), StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PaginatedList<GetUserExtendedDto>>> GetUsers([FromQuery] GetUsersFilter filter)
        {
            return Ok(await _userService.GetAllUser(filter));
        }

        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Member + "," + Roles.Coach + "," + Roles.Gym + "," + Roles.Store)]
        [HttpGet]
        [Route("NumberOfMembersInTheGym")]
        public async Task<ActionResult<int>> NumberOfMembersInTheGym()
        { 
            return await _userService.NumberOfMembersInTheGym();
        }

        /// <summary>
        /// Get one user by id from the database
        /// </summary>
        /// <param name="id">The user's ID</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetUserExtendedDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetUserExtendedDto>> GetUserById(int id)
        {
            var user = await _userService.GetExtendedUserById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Member + "," + Roles.Coach + "," + Roles.Gym + "," + Roles.Store)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Boilerplate.Application.DTOs.User.GetUserDto>> CreateUser(Boilerplate.Application.DTOs.User.CreateUserDto dto)
        {
            var currentRole = _currentUserService.Role;
            var newAccount = await _userService.CreateUser(dto);

            if (currentRole == "SuperAdmin")
            {
                if (dto.Role == "Gym" || dto.Role == "Store")
                {

                }
                else
                {
                    return BadRequest(new { message = "SuperAdmin Can Add Gym Or Store Users Only" });
                }
            }

            if (currentRole == "Gym")
            {
                if (dto.Role == "Member" || dto.Role == "Coach")
                {

                }
                else
                {
                    return BadRequest(new { message = "Gym Can Add Member Or Coach Users Only" });
                }
            }
            return CreatedAtAction(nameof(GetUserById), new { id = newAccount.Id }, newAccount);
        }

        [HttpPatch("updatePassword")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdatePassword([FromBody] UpdatePasswordDto dto)
        {
            await _userService.UpdatePassword(int.Parse(_currentUserService.UserId), dto);
            return NoContent();
        }

        [HttpPost]
        [Route("checkEmailAndMobieNumber")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<CheckEmailAndMobieNumber> CheckEmailAndMobieNumberAsync(string loginInfo)
        {
            CheckEmailAndMobieNumber checkEmailAndMobieNumber = new CheckEmailAndMobieNumber();
            checkEmailAndMobieNumber = await _userService.CheckEmailAndMobieNumber(loginInfo);
            return checkEmailAndMobieNumber;
        }

        [HttpPost]
        [Route("checkpassword")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<bool> CheckPassword(int id, string Password)
        {
            var user = await _userService.CheckPassword(id, Password);
            if (user == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        [Route("QR")]
        [AllowAnonymous]
        public string QR(string Password)
        {
            string x = "";
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator QRCodeGenerator = new QRCodeGenerator();
                QRCodeData QRCodeData = QRCodeGenerator.CreateQrCode(Password, QRCodeGenerator.ECCLevel.Q);
                QRCode qRCode = new QRCode(QRCodeData);
                using (Bitmap oBitmap = qRCode.GetGraphic(20))
                {
                    oBitmap.Save(ms, ImageFormat.Png);
                    x = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    Console.WriteLine("data:image/png;base64," + Convert.ToBase64String(ms.ToArray()));
                }
            }
            return x;
        }

        [HttpPut("updateuser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateUser([FromBody] CreateUserDto dto)
        {
            await _userService.EditUser(dto);
            return NoContent();
        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _userService.DeleteUser(id);
            if (deleted) return NoContent();
            return NotFound();
        }
    }
}