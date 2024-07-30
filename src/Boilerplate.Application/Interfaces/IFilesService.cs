using Boilerplate.Application.DTOs;
using Boilerplate.Application.Filters;
using Boilerplate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Boilerplate.Application.Interfaces
{
    public interface IFilesService
    { 
        public Task<AllList<GetFilesDto>> GetAllFiles();
        Task<string> UploadJson(CreateFilesDto createFilesDto);
    } 
    public class GetFilesDto
    {
        public int Id {  get; set; }
        public string Name { get; set; } 
        public string Url { get; set; }
        public string Type { get; set; }
    }
    public class CreateFilesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }  
        public string file { get; set; }
        public string Type { get; set; }
    } 
}
