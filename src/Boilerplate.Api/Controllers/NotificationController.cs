using Boilerplate.Application.DTOs;
using Boilerplate.Application.Interfaces;
using Boilerplate.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Boilerplate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class NotificationController : ControllerBase
    {
        private IPushTokenService _pushTokenService;

        public NotificationController(IPushTokenService pushTokenService )
        {
            _pushTokenService = pushTokenService;
        }
        [HttpGet]
        public async Task<ActionResult<PaginatedList<NotificationDto>>> GetHeroes()
        {
            return Ok(await _pushTokenService.GetAllNotifications());
        }


        [HttpPost]
        public async Task<ActionResult<PushTokenDto>> Create([FromBody] PushTokenDto dto)
        {
            var newPushToken = await _pushTokenService.CreatePushToken(dto);
            return newPushToken;


        }
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{id}")]
        public async Task<ActionResult> Delete()
        {
            var deleted = await _pushTokenService.DeletePushToken();
            if (deleted) return NoContent();
            return NotFound();
        }

    }
}
