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

namespace Boilerplate.Application.Interfaces
{
    public interface IGalleryService
    {
        public Task<PaginatedList<GetGalleryDto>> GetAllGalleryWithPageSize(GetGalleryFilter filter);
        public Task<AllList<GetGalleryDto>> GetAllGallery(GetGalleryFilter filter); 
        public Task<GetGalleryDto> CreateGallery(CreateGalleryDto Gallery);
        public Task<GetGalleryDto> GetGalleryById(int id); 
        public Task<GetGalleryDto> UpdateGallery(int id, UpdateGalleryDto updatedGallery); 
        public Task<bool> DeleteGallery(int id); 
    }

    public class GetGalleryDto
    {
        public int Id {  get; set; }
        public int UserId { get; set; } 
        public string PhotoUrl { get; set; }
    }
    public class CreateGalleryDto
    {
        public int UserId { get; set; }
        public string PhotoUrl { get; set; }
    }

    public class UpdateGalleryDto
    {
        public int UserId { get; set; }
        public string PhotoUrl { get; set; }
    }

    public class GetGalleryFilter : PaginationInfoFilter
    {
        public int UserId { get; set; }
        public string PhotoUrl { get; set; }
    }

}
