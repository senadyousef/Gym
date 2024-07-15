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
    public class PersonalTrainersClassesController : ControllerBase
    {
        private IPersonalTrainersClassesService _PersonalTrainersClasseservice;

        public PersonalTrainersClassesController(IPersonalTrainersClassesService PersonalTrainersClasseservice)
        {
            _PersonalTrainersClasseservice = PersonalTrainersClasseservice;
        }

        /// <summary>
        /// Returns all PersonalTrainersClasses in the database
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Member + "," + Roles.Coach + "," + Roles.Gym + "," + Roles.Store)]
        public async Task<ActionResult<PaginatedList<GetPersonalTrainersClassesDto>>> GetPersonalTrainersClasses([FromQuery] GetPersonalTrainersClassesFilter filter)
        {
            return Ok(await _PersonalTrainersClasseservice.GetAllPersonalTrainersClassesWithPageSize(filter));
        }

        [HttpGet]
        [Route("getallPersonalTrainersClasses")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Member + "," + Roles.Coach + "," + Roles.Gym + "," + Roles.Store)]
        public async Task<ActionResult<AllList<GetPersonalTrainersClassesDto>>> GetAllPersonalTrainersClasses([FromQuery] GetPersonalTrainersClassesFilter filter)
        {
            return Ok(await _PersonalTrainersClasseservice.GetAllPersonalTrainersClasses(filter));
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Member + "," + Roles.Coach + "," + Roles.Gym + "," + Roles.Store)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetPersonalTrainersClassesDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetPersonalTrainersClassesDto>> GetPersonalTrainersClassesById(int id)
        {
            var subCategories = await _PersonalTrainersClasseservice.GetPersonalTrainersClassesById(id);
            if (subCategories == null) return NotFound();
            return Ok(subCategories);
        }

        /// <summary>
        /// Insert one PersonalTrainersClasses in the database
        /// </summary>
        /// <param name="dto">The PersonalTrainersClasses information</param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Member + "," + Roles.Coach + "," + Roles.Gym + "," + Roles.Store)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetPersonalTrainersClassesDto>> Create([FromBody] CreatePersonalTrainersClassesDto dto)
        {
            var newPersonalTrainersClasses = await _PersonalTrainersClasseservice.CreatePersonalTrainersClasses(dto);
            return CreatedAtAction(nameof(GetPersonalTrainersClassesById), new { id = newPersonalTrainersClasses.Id }, newPersonalTrainersClasses);

        }

        /// <summary>
        /// Update a PersonalTrainersClasses from the database
        /// </summary>
        /// <param name="id">The PersonalTrainersClasses's ID</param>
        /// <param name="dto">The update object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Member + "," + Roles.Coach + "," + Roles.Gym + "," + Roles.Store)]
        public async Task<ActionResult<GetPersonalTrainersClassesDto>> UpdatePersonalTrainersClasses(int id, [FromBody] UpdatePersonalTrainersClassesDto dto)
        {

            var updatedPersonalTrainersClasses = await _PersonalTrainersClasseservice.UpdatePersonalTrainersClasses(id, dto);

            if (updatedPersonalTrainersClasses == null) return NotFound();

            return Ok(updatedPersonalTrainersClasses);
        }

        /// <summary>
        /// Delete a PersonalTrainersClasses from the database
        /// </summary>
        /// <param name="id">The PersonalTrainersClasses's ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Member + "," + Roles.Coach + "," + Roles.Gym + "," + Roles.Store)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{id}")]
        public async Task<ActionResult> DeletePersonalTrainersClasses(int id)
        {
            var deleted = await _PersonalTrainersClasseservice.DeletePersonalTrainersClasses(id);
            if (deleted) return NoContent();
            return NotFound();
        }

    }
}
