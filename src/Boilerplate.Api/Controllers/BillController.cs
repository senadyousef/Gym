
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
    public class BillController : ControllerBase
    {
        private IBillService _Billervice;

        public BillController(IBillService Billervice)
        {
            _Billervice = Billervice;
        }
        [HttpGet]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        public async Task<ActionResult<PaginatedList<GetBillDto>>> GetBill([FromQuery] GetBillFilter filter)
        {
            return Ok(await _Billervice.GetAllBillWithPageSize(filter));
        }

        [HttpGet]
        [Route("getallBill")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        public async Task<ActionResult<AllList<GetBillDto>>> GetAllBill([FromQuery] GetBillFilter filter)
        {
            return Ok(await _Billervice.GetAllBill(filter));
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetBillDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetBillDto>> GetBillById(int id)
        {
            var Bill = await _Billervice.GetBillById(id);
            if (Bill == null) return NotFound();
            return Ok(Bill);
        }

        /// <summary>
        /// Insert one Bill in the database
        /// </summary>
        /// <param name="dto">The Bill information</param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetBillDto>> Create([FromBody] CreateBillDto dto)
        {
            var newBill = await _Billervice.CreateBill(dto);
            return CreatedAtAction(nameof(GetBillById), new { id = newBill.Id }, newBill);

        }

        /// <summary>
        /// Update a Bill from the database
        /// </summary>
        /// <param name="id">The Bill's ID</param>
        /// <param name="dto">The update object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        public async Task<ActionResult<GetBillDto>> UpdateBill(int id, [FromBody] UpdateBillDto dto)
        {

            var updatedBill = await _Billervice.UpdateBill(id, dto);

            if (updatedBill == null) return NotFound();

            return Ok(updatedBill);
        }

        /// <summary>
        /// Delete a Bill from the database
        /// </summary>
        /// <param name="id">The Bill's ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{id}")]
        public async Task<ActionResult> DeleteBill(int id)
        {
            var deleted = await _Billervice.DeleteBill(id);
            if (deleted) return NoContent();
            return NotFound();
        } 
    }
}
