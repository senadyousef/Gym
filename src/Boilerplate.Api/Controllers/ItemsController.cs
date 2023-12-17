﻿using Boilerplate.Application.DTOs;
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
    public class ItemsController : ControllerBase
    {
        private IItemsService _Itemservice;

        public ItemsController(IItemsService Itemservice)
        {
            _Itemservice = Itemservice;
        }

        /// <summary>
        /// Returns all Items in the database
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        public async Task<ActionResult<PaginatedList<GetItemsDto>>> GetItems([FromQuery] GetItemsFilter filter)
        {
            return Ok(await _Itemservice.GetAllItemsWithPageSize(filter));
        }

        [HttpGet]
        [Route("getallItems")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        public async Task<ActionResult<AllList<GetItemsDto>>> GetAllItems([FromQuery] GetItemsFilter filter)
        {
            return Ok(await _Itemservice.GetAllItems(filter));
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Owner + "," + Roles.Customer)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetItemsDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetItemsDto>> GetItemsById(int id)
        {
            var subCategories = await _Itemservice.GetItemsById(id);
            if (subCategories == null) return NotFound();
            return Ok(subCategories);
        }

        /// <summary>
        /// Insert one Items in the database
        /// </summary>
        /// <param name="dto">The Items information</param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Owner)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetItemsDto>> Create([FromBody] CreateItemsDto dto)
        {
            var newItems = await _Itemservice.CreateItems(dto);
            return CreatedAtAction(nameof(GetItemsById), new { id = newItems.Id }, newItems);

        }
         
        /// <summary>
        /// Update a Items from the database
        /// </summary>
        /// <param name="id">The Items's ID</param>
        /// <param name="dto">The update object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Owner)]
        public async Task<ActionResult<GetItemsDto>> UpdateItems(int id, [FromBody] UpdateItemsDto dto)
        {

            var updatedItems = await _Itemservice.UpdateItems(id, dto);

            if (updatedItems == null) return NotFound();

            return Ok(updatedItems);
        }

        /// <summary>
        /// Delete a Items from the database
        /// </summary>
        /// <param name="id">The Items's ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Owner)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{id}")]
        public async Task<ActionResult> DeleteItems(int id)
        {
            var deleted = await _Itemservice.DeleteItems(id);
            if (deleted) return NoContent();
            return NotFound();
        }
    
    }
}
