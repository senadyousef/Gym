
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
    public class BranchesController : ControllerBase
    {
        private IBranchesService _Brancheservice;

        public BranchesController(IBranchesService Brancheservice)
        {
            _Brancheservice = Brancheservice;
        }
        [HttpGet]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        public async Task<ActionResult<PaginatedList<GetBranchesDto>>> GetBranches([FromQuery] GetBranchesFilter filter)
        {
            return Ok(await _Brancheservice.GetAllBranchesWithPageSize(filter));
        }

        [HttpGet]
        [Route("getallBranches")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        public async Task<ActionResult<AllList<GetBranchesDto>>> GetAllBranches([FromQuery] GetBranchesFilter filter)
        {
            return Ok(await _Brancheservice.GetAllBranches(filter));
        }
          
        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetBranchesDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetBranchesDto>> GetBranchesById(int id)
        {
            var Branches = await _Brancheservice.GetBranchesById(id);
            if (Branches == null) return NotFound();
            return Ok(Branches);
        }

        /// <summary>
        /// Insert one Branches in the database
        /// </summary>
        /// <param name="dto">The Branches information</param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Roles = Roles.SuperAdmin)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetBranchesDto>> Create([FromBody] CreateBranchesDto dto)
        {
            var newBranches = await _Brancheservice.CreateBranches(dto);
            return CreatedAtAction(nameof(GetBranchesById), new { id = newBranches.Id }, newBranches); 
        }

        /// <summary>
        /// Update a Branches from the database
        /// </summary>
        /// <param name="id">The Branches's ID</param>
        /// <param name="dto">The update object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.SuperAdmin)]
        public async Task<ActionResult<GetBranchesDto>> UpdateBranches(int id, [FromBody] UpdateBranchesDto dto)
        {

            var updatedBranches = await _Brancheservice.UpdateBranches(id, dto);

            if (updatedBranches == null) return NotFound();

            return Ok(updatedBranches);
        }

        /// <summary>
        /// Delete a Branches from the database
        /// </summary>
        /// <param name="id">The Branches's ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = Roles.SuperAdmin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{id}")]
        public async Task<ActionResult> DeleteBranches(int id)
        {
            var deleted = await _Brancheservice.DeleteBranches(id);
            if (deleted) return NoContent();
            return NotFound();
        }
    }
}
