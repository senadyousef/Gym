
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
    public class UserItemsController : ControllerBase
    {
        private IUserItemsService _UserItemservice;

        public UserItemsController(IUserItemsService UserItemservice)
        {
            _UserItemservice = UserItemservice;
        }
        [HttpGet]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        public async Task<ActionResult<PaginatedList<GetUserItemsDto>>> GetUserItems([FromQuery] GetUserItemsFilter filter)
        {
            return Ok(await _UserItemservice.GetAllUserItemsWithPageSize(filter));
        }

        [HttpGet]
        [Route("getallUserItems")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        public async Task<ActionResult<AllList<GetUserItemsDto>>> GetAllUserItems([FromQuery] GetUserItemsFilter filter)
        {
            return Ok(await _UserItemservice.GetAllUserItems(filter));
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetUserItemsDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetUserItemsDto>> GetUserItemsById(int id)
        {
            var UserItems = await _UserItemservice.GetUserItemsById(id);
            if (UserItems == null) return NotFound();
            return Ok(UserItems);
        }

        /// <summary>
        /// Insert one UserItems in the database
        /// </summary>
        /// <param name="dto">The UserItems information</param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetUserItemsDto>> Create([FromBody] CreateUserItemsDto dto)
        {
            var newUserItems = await _UserItemservice.CreateUserItems(dto);
            return CreatedAtAction(nameof(GetUserItemsById), new { id = newUserItems.Id }, newUserItems);

        }

        /// <summary>
        /// Update a UserItems from the database
        /// </summary>
        /// <param name="id">The UserItems's ID</param>
        /// <param name="dto">The update object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        public async Task<ActionResult<GetUserItemsDto>> UpdateUserItems(int id, [FromBody] UpdateUserItemsDto dto)
        {

            var updatedUserItems = await _UserItemservice.UpdateUserItems(id, dto);

            if (updatedUserItems == null) return NotFound();

            return Ok(updatedUserItems);
        }

        /// <summary>
        /// Delete a UserItems from the database
        /// </summary>
        /// <param name="id">The UserItems's ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{id}")]
        public async Task<ActionResult> DeleteUserItems(int id)
        {
            var deleted = await _UserItemservice.DeleteUserItems(id);
            if (deleted) return NoContent();
            return NotFound();
        }
         
        //[Route("getAllBoughtItems")]
        //[Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        //public List<NumberOfBoughtItems> GetAllBoughtItems(GetUserItemsFilter filter)
        //{
        //    List<NumberOfBoughtItems> countDto = null; 
        //    countDto = _UserItemservice.GetAllBoughtItems(filter);
        //    return countDto;
        //}
    }
}
