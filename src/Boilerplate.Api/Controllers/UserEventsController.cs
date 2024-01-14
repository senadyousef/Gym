
using Boilerplate.Application.DTOs;
using Boilerplate.Application.Interfaces;
using Boilerplate.Domain.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System;
using System.Threading.Tasks;

namespace Boilerplate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserEventsController : ControllerBase
    {
        private IUserEventsService _UserEventservice;

        public UserEventsController(IUserEventsService UserEventservice)
        {
            _UserEventservice = UserEventservice;
        }

        /// <summary>
        /// Returns all UserEvents in the database
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        ///  
        [HttpGet]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        public async Task<ActionResult<PaginatedList<GetUserEventsDto>>> GetUserEvents([FromQuery] GetUserEventsFilter filter)
        {
            return Ok(await _UserEventservice.GetAllUserEvents(filter));
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetUserEventsDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetUserEventsDto>> GetUserEventsById(int id)
        {
            var UserEvents = await _UserEventservice.GetUserEventsById(id);
            if (UserEvents == null) return NotFound();
            return Ok(UserEvents);
        }

        /// <summary>
        /// Insert one UserEvents in the database
        /// </summary>
        /// <param name="dto">The UserEvents information</param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetUserEventsDto>> Create([FromBody] CreateUserEventsDto dto)
        {
            var newUserEvents = await _UserEventservice.CreateUserEvents(dto);
            if (newUserEvents == null)
            {
                return BadRequest(new { message = "This class is already booked." });
            }
            return CreatedAtAction(nameof(GetUserEventsById), new { id = newUserEvents.Id }, newUserEvents);
        }

        /// <summary>
        /// Update a UserEvents from the database
        /// </summary>
        /// <param name="id">The UserEvents's ID</param>
        /// <param name="dto">The update object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        public async Task<ActionResult<GetUserEventsDto>> UpdateUserEvents(int id, [FromBody] UpdateUserEventsDto dto)
        {

            var updatedUserEvents = await _UserEventservice.UpdateUserEvents(id, dto);

            if (updatedUserEvents == null) return NotFound();

            return Ok(updatedUserEvents);
        }

        /// <summary>
        /// Delete a UserEvents from the database
        /// </summary>
        /// <param name="id">The UserEvents's ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{id}")]
        public async Task<ActionResult> DeleteUserEvents(int id)
        {
            var deleted = await _UserEventservice.DeleteUserEvents(id);
            if (deleted) return NoContent();
            return NotFound();
        }

    }
}
