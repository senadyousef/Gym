
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
    public class AllServicesController : ControllerBase
    {
        private IAllServicesService _AllServiceservice;

        public AllServicesController(IAllServicesService AllServiceservice)
        {
            _AllServiceservice = AllServiceservice;
        }

        /// <summary>
        /// Returns all AllServices in the database
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        ///  
        [HttpGet]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Member + "," + Roles.Coach + "," + Roles.Gym + "," + Roles.Store)]
        public async Task<ActionResult<PaginatedList<GetAllServicesDto>>> GetAllServices([FromQuery] GetAllServicesFilter filter)
        {
            return Ok(await _AllServiceservice.GetAllAllServices(filter));
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Member + "," + Roles.Coach + "," + Roles.Gym + "," + Roles.Store)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetAllServicesDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetAllServicesDto>> GetAllServicesById(int id)
        {
            var AllServices = await _AllServiceservice.GetAllServicesById(id);
            if (AllServices == null) return NotFound();
            return Ok(AllServices);
        }

        /// <summary>
        /// Insert one AllServices in the database
        /// </summary>
        /// <param name="dto">The AllServices information</param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Member + "," + Roles.Coach + "," + Roles.Gym + "," + Roles.Store)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetAllServicesDto>> Create([FromBody] CreateAllServicesDto dto)
        {
            var newAllServices = await _AllServiceservice.CreateAllServices(dto);
            if (newAllServices == null)
            {
                return BadRequest(new { message = "This class is already booked." });
            }
            return CreatedAtAction(nameof(GetAllServicesById), new { id = newAllServices.Id }, newAllServices);
        }

        /// <summary>
        /// Update a AllServices from the database
        /// </summary>
        /// <param name="id">The AllServices's ID</param>
        /// <param name="dto">The update object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Member + "," + Roles.Coach + "," + Roles.Gym + "," + Roles.Store)]
        public async Task<ActionResult<GetAllServicesDto>> UpdateAllServices(int id, [FromBody] UpdateAllServicesDto dto)
        {

            var updatedAllServices = await _AllServiceservice.UpdateAllServices(id, dto);

            if (updatedAllServices == null) return NotFound();

            return Ok(updatedAllServices);
        }

        /// <summary>
        /// Delete a AllServices from the database
        /// </summary>
        /// <param name="id">The AllServices's ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Member + "," + Roles.Coach + "," + Roles.Gym + "," + Roles.Store)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{id}")]
        public async Task<ActionResult> DeleteAllServices(int id)
        {
            var deleted = await _AllServiceservice.DeleteAllServices(id);
            if (deleted) return NoContent();
            return NotFound();
        }

    }
}
