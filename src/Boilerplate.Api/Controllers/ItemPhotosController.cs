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
    public class ItemPhotosController : ControllerBase
    {
        private IItemPhotosService _ItemPhotoservice;

        public ItemPhotosController(IItemPhotosService ItemPhotoservice)
        {
            _ItemPhotoservice = ItemPhotoservice;
        }

        /// <summary>
        /// Returns all ItemPhotos in the database
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet] 
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        public async Task<ActionResult<PaginatedList<GetItemPhotosDto>>> GetItemPhotos([FromQuery] GetItemPhotosFilter filter)
        {
            return Ok(await _ItemPhotoservice.GetAllItemPhotos(filter));
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Owner + "," + Roles.Customer)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetItemPhotosDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetItemPhotosDto>> GetItemPhotosById(int id)
        {
            var subCategories = await _ItemPhotoservice.GetItemPhotosById(id);
            if (subCategories == null) return NotFound();
            return Ok(subCategories);
        }

        /// <summary>
        /// Insert one ItemPhotos in the database
        /// </summary>
        /// <param name="dto">The ItemPhotos information</param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Owner)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetItemPhotosDto>> Create([FromBody] CreateItemPhotosDto dto)
        {
            var newItemPhotos = await _ItemPhotoservice.CreateItemPhotos(dto);
            return CreatedAtAction(nameof(GetItemPhotosById), new { id = newItemPhotos.Id }, newItemPhotos);

        }
         
        /// <summary>
        /// Update a ItemPhotos from the database
        /// </summary>
        /// <param name="id">The ItemPhotos's ID</param>
        /// <param name="dto">The update object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Owner)]
        public async Task<ActionResult<GetItemPhotosDto>> UpdateItemPhotos(int id, [FromBody] UpdateItemPhotosDto dto)
        {

            var updatedItemPhotos = await _ItemPhotoservice.UpdateItemPhotos(id, dto);

            if (updatedItemPhotos == null) return NotFound();

            return Ok(updatedItemPhotos);
        }

        /// <summary>
        /// Delete a ItemPhotos from the database
        /// </summary>
        /// <param name="id">The ItemPhotos's ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Owner)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{id}")]
        public async Task<ActionResult> DeleteItemPhotos(int id)
        {
            var deleted = await _ItemPhotoservice.DeleteItemPhotos(id);
            if (deleted) return NoContent();
            return NotFound();
        }
    
    }
}
