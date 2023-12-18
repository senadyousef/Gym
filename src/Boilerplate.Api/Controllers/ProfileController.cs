using System;
using System.Threading.Tasks;
using Boilerplate.Application.DTOs;
using Boilerplate.Application.DTOs.Auth;
using Boilerplate.Application.DTOs.User;
using Boilerplate.Application.Filters;
using Boilerplate.Application.Interfaces;
using Boilerplate.Domain.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ISession = Boilerplate.Domain.Auth.Interfaces.ISession;

namespace Boilerplate.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly IAuthService _authService;
        private readonly ISession _session;

        public ProfileController(IProfileService profileService, IAuthService authService, ISession session)
        {
            _profileService = profileService;
            _authService = authService;
            _session = session;
        }

        [Authorize(Roles = Roles.SuperAdmin)]
        [HttpGet]
        [Route("GetMyProfile")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetUserDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetUserDto>> GetUserById()
        {
            var user = await _profileService.GetMyProfile();
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPatch("updateMyPassword")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateMyPassword([FromBody] UpdatePasswordDto dto)
        {
            await _profileService.UpdateMyPassword(dto);
            return NoContent();
        }

        [HttpPatch("updateMyDisplayName")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateMyDisplayName([FromBody] UpdateDisplayNameDto dto)
        {
            await _profileService.UpdateMyDisplayName(dto);
            return NoContent();
        }

        [HttpPatch("updateMyMobile")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateMyMobile([FromBody] UpdateMobileDto dto)
        {
            await _profileService.UpdateMyMobile(dto);
            return NoContent();
        }
        [HttpPatch("updateMyProfilePicture")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> updateMyProfilePicture([FromBody] UploadRequest dto)
        {
            await _profileService.UpdateMyProfilePicture(dto);
            return NoContent();
        }

    }
}
