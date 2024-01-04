
using Boilerplate.Application.DTOs;
using Boilerplate.Application.Interfaces;
using Boilerplate.Domain.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Boilerplate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopOfTopController : ControllerBase
    {
        private ITopOfTopService _TopOfTopervice;

        public TopOfTopController(ITopOfTopService TopOfTopervice)
        {
            _TopOfTopervice = TopOfTopervice;
        }

        /// <summary>
        /// Returns all TopOfTop in the database
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        public async Task<ActionResult<PaginatedList<GetTopOfTopDto>>> GetTopOfTop([FromQuery] GetTopOfTopFilter filter)
        {
            return Ok(await _TopOfTopervice.GetAllTopOfTop(filter));
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetTopOfTopDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetTopOfTopDto>> GetTopOfTopById(int id)
        {
            var TopOfTop = await _TopOfTopervice.GetTopOfTopById(id);
            if (TopOfTop == null) return NotFound();
            return Ok(TopOfTop);
        }

        /// <summary>
        /// Insert one TopOfTop in the database
        /// </summary>
        /// <param name="dto">The TopOfTop information</param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTopOfTopDto>> Create([FromBody] CreateTopOfTopDto dto)
        {
            var newTopOfTop = await _TopOfTopervice.CreateTopOfTop(dto);
            return CreatedAtAction(nameof(GetTopOfTopById), new { id = newTopOfTop.Id }, newTopOfTop);

        }
         
        /// <summary>
        /// Update a TopOfTop from the database
        /// </summary>
        /// <param name="id">The TopOfTop's ID</param>
        /// <param name="dto">The update object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        public async Task<ActionResult<GetTopOfTopDto>> UpdateTopOfTop(int id, [FromBody] UpdateTopOfTopDto dto)
        {

            var updatedTopOfTop = await _TopOfTopervice.UpdateTopOfTop(id, dto);

            if (updatedTopOfTop == null) return NotFound();

            return Ok(updatedTopOfTop);
        }

        /// <summary>
        /// Delete a TopOfTop from the database
        /// </summary>
        /// <param name="id">The TopOfTop's ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{id}")]
        public async Task<ActionResult> DeleteTopOfTop(int id)
        {
            var deleted = await _TopOfTopervice.DeleteTopOfTop(id);
            if (deleted) return NoContent();
            return NotFound();
        }
    
    }
}
