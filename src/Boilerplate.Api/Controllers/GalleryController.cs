
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
    public class GalleryController : ControllerBase
    {
        private IGalleryService _Galleryervice;

        public GalleryController(IGalleryService Galleryervice)
        {
            _Galleryervice = Galleryervice;
        }

        /// <summary>
        /// Returns all Gallery in the database
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        ///  
        [HttpGet]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Member + "," + Roles.Coach + "," + Roles.Gym + "," + Roles.Store)]
        public async Task<ActionResult<PaginatedList<GetGalleryDto>>> GetGallery([FromQuery] GetGalleryFilter filter)
        {
            return Ok(await _Galleryervice.GetAllGallery(filter));
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Member + "," + Roles.Coach + "," + Roles.Gym + "," + Roles.Store)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetGalleryDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetGalleryDto>> GetGalleryById(int id)
        {
            var Gallery = await _Galleryervice.GetGalleryById(id);
            if (Gallery == null) return NotFound();
            return Ok(Gallery);
        }

        /// <summary>
        /// Insert one Gallery in the database
        /// </summary>
        /// <param name="dto">The Gallery information</param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Member + "," + Roles.Coach + "," + Roles.Gym + "," + Roles.Store)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetGalleryDto>> Create([FromBody] CreateGalleryDto dto)
        {
            var newGallery = await _Galleryervice.CreateGallery(dto);
            if (newGallery == null)
            {
                return BadRequest(new { message = "This class is already booked." });
            }
            return CreatedAtAction(nameof(GetGalleryById), new { id = newGallery.Id }, newGallery);
        }

        /// <summary>
        /// Update a Gallery from the database
        /// </summary>
        /// <param name="id">The Gallery's ID</param>
        /// <param name="dto">The update object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Member + "," + Roles.Coach + "," + Roles.Gym + "," + Roles.Store)]
        public async Task<ActionResult<GetGalleryDto>> UpdateGallery(int id, [FromBody] UpdateGalleryDto dto)
        {

            var updatedGallery = await _Galleryervice.UpdateGallery(id, dto);

            if (updatedGallery == null) return NotFound();

            return Ok(updatedGallery);
        }

        /// <summary>
        /// Delete a Gallery from the database
        /// </summary>
        /// <param name="id">The Gallery's ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Member + "," + Roles.Coach + "," + Roles.Gym + "," + Roles.Store)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{id}")]
        public async Task<ActionResult> DeleteGallery(int id)
        {
            var deleted = await _Galleryervice.DeleteGallery(id);
            if (deleted) return NoContent();
            return NotFound();
        }

    }
}
