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
    public class ChildrenController : ControllerBase
    {
        private IChildrenService _Childrenervice;

        public ChildrenController(IChildrenService Childrenervice)
        {
            _Childrenervice = Childrenervice;
        }

        /// <summary>
        /// Returns all Children in the database
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        public async Task<ActionResult<PaginatedList<GetChildrenDto>>> GetChildren([FromQuery] GetChildrenFilter filter)
        {
            return Ok(await _Childrenervice.GetAllChildrenWithPageSize(filter));
        }

        [HttpGet]
        [Route("getallChildren")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        public async Task<ActionResult<AllList<GetChildrenDto>>> GetAllChildren([FromQuery] GetChildrenFilter filter)
        {
            return Ok(await _Childrenervice.GetAllChildren(filter));
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetChildrenDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetChildrenDto>> GetChildrenById(int id)
        {
            var subCategories = await _Childrenervice.GetChildrenById(id);
            if (subCategories == null) return NotFound();
            return Ok(subCategories);
        }

        /// <summary>
        /// Insert one Children in the database
        /// </summary>
        /// <param name="dto">The Children information</param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Roles = Roles.SuperAdmin)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetChildrenDto>> Create([FromBody] CreateChildrenDto dto)
        {
            var newChildren = await _Childrenervice.CreateChildren(dto);
            return CreatedAtAction(nameof(GetChildrenById), new { id = newChildren.Id }, newChildren);

        }

        /// <summary>
        /// Update a Children from the database
        /// </summary>
        /// <param name="id">The Children's ID</param>
        /// <param name="dto">The update object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.SuperAdmin)]
        public async Task<ActionResult<GetChildrenDto>> UpdateChildren(int id, [FromBody] UpdateChildrenDto dto)
        {

            var updatedChildren = await _Childrenervice.UpdateChildren(id, dto);

            if (updatedChildren == null) return NotFound();

            return Ok(updatedChildren);
        }

        /// <summary>
        /// Delete a Children from the database
        /// </summary>
        /// <param name="id">The Children's ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = Roles.SuperAdmin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{id}")]
        public async Task<ActionResult> DeleteChildren(int id)
        {
            var deleted = await _Childrenervice.DeleteChildren(id);
            if (deleted) return NoContent();
            return NotFound();
        }

    }
}
