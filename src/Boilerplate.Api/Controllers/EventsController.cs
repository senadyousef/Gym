
using Boilerplate.Application.DTOs;
using Boilerplate.Application.Interfaces;
using Boilerplate.Domain.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Boilerplate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private IEventsService _Eventservice;

        public EventsController(IEventsService Eventservice)
        {
            _Eventservice = Eventservice;
        }
        [HttpGet]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        public async Task<ActionResult<PaginatedList<GetEventsDto>>> GetEvents([FromQuery] GetEventsFilter filter)
        {
            return Ok(await _Eventservice.GetAllEventsWithPageSize(filter));
        }

        [HttpGet]
        [Route("getallEvents")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        public async Task<ActionResult<AllList<GetEventsDto>>> GetAllEvents([FromQuery] GetEventsFilter filter)
        {
            return Ok(await _Eventservice.GetAllEvents(filter));
        }
         
        [HttpGet]
        [Route("GetDates")]
        public async Task<ActionResult<PaginatedList<GetEventsByDates>>> GetDates(int id, int year, int month)
        {
            return Ok(await _Eventservice.GetDates(id, year, month));
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetEventsDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetEventsDto>> GetEventsById(int id)
        {
            var Events = await _Eventservice.GetEventsById(id);
            if (Events == null) return NotFound();
            return Ok(Events);
        }

        /// <summary>
        /// Insert one Events in the database
        /// </summary>
        /// <param name="dto">The Events information</param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Roles = Roles.SuperAdmin)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetEventsDto>> Create([FromBody] CreateEventsDto dto)
        {
            var newEvents = await _Eventservice.CreateEvents(dto);
            return CreatedAtAction(nameof(GetEventsById), new { id = newEvents.Id }, newEvents);

        }

        /// <summary>
        /// Update a Events from the database
        /// </summary>
        /// <param name="id">The Events's ID</param>
        /// <param name="dto">The update object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.SuperAdmin)]
        public async Task<ActionResult<GetEventsDto>> UpdateEvents(int id, [FromBody] UpdateEventsDto dto)
        {

            var updatedEvents = await _Eventservice.UpdateEvents(id, dto);

            if (updatedEvents == null) return NotFound();

            return Ok(updatedEvents);
        }

        /// <summary>
        /// Delete a Events from the database
        /// </summary>
        /// <param name="id">The Events's ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = Roles.SuperAdmin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{id}")]
        public async Task<ActionResult> DeleteEvents(int id)
        {
            var deleted = await _Eventservice.DeleteEvents(id);
            if (deleted) return NoContent();
            return NotFound();
        }
    }
}
