
using Boilerplate.Application.DTOs;
using Boilerplate.Application.Interfaces;
using Boilerplate.Application.Services;
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
    public class NewsController : ControllerBase
    {
        private INewsService _Newservice;

        public NewsController(INewsService Newservice)
        {
            _Newservice = Newservice;
        }

        /// <summary>
        /// Returns all News in the database
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        public async Task<ActionResult<PaginatedList<GetNewsDto>>> GetNews([FromQuery] GetNewsFilter filter)
        {
            return Ok(await _Newservice.GetAllNewsWithPageSize(filter));
        }

        [HttpGet]
        [Route("getallNews")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        public async Task<ActionResult<AllList<GetNewsDto>>> GetAllNews([FromQuery] GetNewsFilter filter)
        {
            return Ok(await _Newservice.GetAllNews(filter));
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetNewsDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetNewsDto>> GetNewsById(int id)
        {
            var News = await _Newservice.GetNewsById(id);
            if (News == null) return NotFound();
            return Ok(News);
        }

        /// <summary>
        /// Insert one News in the database
        /// </summary>
        /// <param name="dto">The News information</param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetNewsDto>> Create([FromBody] CreateNewsDto dto)
        {
            var newNews = await _Newservice.CreateNews(dto);
            return CreatedAtAction(nameof(GetNewsById), new { id = newNews.Id }, newNews);

        }
         
        /// <summary>
        /// Update a News from the database
        /// </summary>
        /// <param name="id">The News's ID</param>
        /// <param name="dto">The update object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        public async Task<ActionResult<GetNewsDto>> UpdateNews(int id, [FromBody] UpdateNewsDto dto)
        {

            var updatedNews = await _Newservice.UpdateNews(id, dto);

            if (updatedNews == null) return NotFound();

            return Ok(updatedNews);
        }

        /// <summary>
        /// Delete a News from the database
        /// </summary>
        /// <param name="id">The News's ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = Roles.SuperAdmin + "," + Roles.Customer)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{id}")]
        public async Task<ActionResult> DeleteNews(int id)
        {
            var deleted = await _Newservice.DeleteNews(id);
            if (deleted) return NoContent();
            return NotFound();
        }
    
    }
}
