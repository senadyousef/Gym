
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
    public class UserAllServicesController : ControllerBase
    {
        private IUserAllServicesService _UserAllServiceservice;

        public UserAllServicesController(IUserAllServicesService UserAllServiceservice)
        {
            _UserAllServiceservice = UserAllServiceservice;
        }

        /// <summary>
        /// Returns all UserAllServices in the database
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        ///  
        [HttpGet]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Member + "," + Roles.Coach + "," + Roles.Gym + "," + Roles.Store)]
        public async Task<ActionResult<PaginatedList<GetUserAllServicesDto>>> GetUserAllServices([FromQuery] GetUserAllServicesFilter filter)
        {
            return Ok(await _UserAllServiceservice.GetAllUserAllServices(filter));
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Member + "," + Roles.Coach + "," + Roles.Gym + "," + Roles.Store)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetUserAllServicesDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetUserAllServicesDto>> GetUserAllServicesById(int id)
        {
            var UserAllServices = await _UserAllServiceservice.GetUserAllServicesById(id);
            if (UserAllServices == null) return NotFound();
            return Ok(UserAllServices);
        }

        /// <summary>
        /// Insert one UserAllServices in the database
        /// </summary>
        /// <param name="dto">The UserAllServices information</param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Member + "," + Roles.Coach + "," + Roles.Gym + "," + Roles.Store)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetUserAllServicesDto>> Create([FromBody] CreateUserAllServicesDto dto)
        {
            var newUserAllServices = await _UserAllServiceservice.CreateUserAllServices(dto);
            if (newUserAllServices == null)
            {
                return BadRequest(new { message = "This class is already booked." });
            }
            return CreatedAtAction(nameof(GetUserAllServicesById), new { id = newUserAllServices.Id }, newUserAllServices);
        }

        /// <summary>
        /// Update a UserAllServices from the database
        /// </summary>
        /// <param name="id">The UserAllServices's ID</param>
        /// <param name="dto">The update object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Member + "," + Roles.Coach + "," + Roles.Gym + "," + Roles.Store)]
        public async Task<ActionResult<GetUserAllServicesDto>> UpdateUserAllServices(int id, [FromBody] UpdateUserAllServicesDto dto)
        {

            var updatedUserAllServices = await _UserAllServiceservice.UpdateUserAllServices(id, dto);

            if (updatedUserAllServices == null) return NotFound();

            return Ok(updatedUserAllServices);
        }

        /// <summary>
        /// Delete a UserAllServices from the database
        /// </summary>
        /// <param name="id">The UserAllServices's ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Member + "," + Roles.Coach + "," + Roles.Gym + "," + Roles.Store)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{id}")]
        public async Task<ActionResult> DeleteUserAllServices(int id)
        {
            var deleted = await _UserAllServiceservice.DeleteUserAllServices(id);
            if (deleted) return NoContent();
            return NotFound();
        }

    }
}
