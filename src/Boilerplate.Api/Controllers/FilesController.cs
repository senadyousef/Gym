
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
    public class FilesController : ControllerBase
    {
        private IFilesService _Fileservice;

        public FilesController(IFilesService Fileservice)
        {
            _Fileservice = Fileservice;
        } 
         
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedList<GetFilesDto>>> GetFiles()
        {
            return Ok(await _Fileservice.GetAllFiles());
        }
          
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)] 
        public async Task<string> UpLoadFile(CreateFilesDto createFilesDto)
        { 
            return await _Fileservice.UploadJson(createFilesDto);
        } 
        
        [HttpPost]
        [AllowAnonymous]
        [Route("deleteFiles")]
        [ProducesResponseType(StatusCodes.Status201Created)] 
        public async Task<string> DeleteFile(string name)
        { 
            return await _Fileservice.DeleteFile(name);
        } 
        
        [HttpPost]
        [AllowAnonymous]
        [Route("saveFile")]
        [ProducesResponseType(StatusCodes.Status201Created)] 
        public async Task<string> saveFile(CreateFilesDto createFilesDto)
        { 
            return await _Fileservice.saveFile(createFilesDto);
        } 
    }
}
