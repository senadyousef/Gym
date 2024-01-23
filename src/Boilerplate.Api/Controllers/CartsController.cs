
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
using Boilerplate.Application.Services;
using System.Net.Http;
using System.Text;
using Boilerplate.Api.Models;
using Boilerplate.Application.DTOs.User;
using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using DevExtreme.AspNet.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient.Server;

namespace Boilerplate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private ICartsService _Cartservice;

        public CartsController(ICartsService Cartservice)
        {
            _Cartservice = Cartservice;
        }
        [HttpGet]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        public async Task<ActionResult<PaginatedList<GetCartsDto>>> GetCarts([FromQuery] GetCartsFilter filter)
        {
            return Ok(await _Cartservice.GetAllCartsWithPageSize(filter));
        }

        [HttpGet]
        [Route("getallCarts")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        public async Task<ActionResult<AllList<GetCartsDto>>> GetAllCarts([FromQuery] GetCartsFilter filter)
        {
            return Ok(await _Cartservice.GetAllCarts(filter));
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetCartsDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetCartsDto>> GetCartsById(int id)
        {
            var Carts = await _Cartservice.GetCartsById(id);
            if (Carts == null) return NotFound();
            return Ok(Carts);
        }

        /// <summary>
        /// Insert one Carts in the database
        /// </summary>
        /// <param name="dto">The Carts information</param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetCartsDto>> Create([FromBody] CreateCartsDto dto)
        {
            var newCarts = await _Cartservice.CreateCarts(dto);
            return CreatedAtAction(nameof(GetCartsById), new { id = newCarts.Id }, newCarts);

        }

        /// <summary>
        /// Update a Carts from the database
        /// </summary>
        /// <param name="id">The Carts's ID</param>
        /// <param name="dto">The update object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        public async Task<ActionResult<GetCartsDto>> UpdateCarts(int id, [FromBody] UpdateCartsDto dto)
        {

            var updatedCarts = await _Cartservice.UpdateCarts(id, dto);

            if (updatedCarts == null) return NotFound();

            return Ok(updatedCarts);
        }

        /// <summary>
        /// Delete a Carts from the database
        /// </summary>
        /// <param name="id">The Carts's ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{id}")]
        public async Task<ActionResult> DeleteCarts(int id)
        {
            var deleted = await _Cartservice.DeleteCarts(id);
            if (deleted) return NoContent();
            return NotFound();
        } 
    }
}
