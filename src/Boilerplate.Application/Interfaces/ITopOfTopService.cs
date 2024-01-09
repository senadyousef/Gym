using Boilerplate.Application.DTOs;
using Boilerplate.Application.Filters;
using Boilerplate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Application.Interfaces
{
    public interface ITopOfTopService
    {
        public Task<PaginatedList<GetTopOfTopDto>> GetAllTopOfTopWithPageSize(GetTopOfTopFilter filter);
        public Task<AllList<GetTopOfTopDto>> GetAllTopOfTop(GetTopOfTopFilter filter);
        public Task<GetTopOfTopDto> CreateTopOfTop(CreateTopOfTopDto TopOfTop);
        public Task<GetTopOfTopDto> GetTopOfTopById(int id); 
        public Task<GetTopOfTopDto> UpdateTopOfTop(int id, UpdateTopOfTopDto updatedTopOfTop); 
        public Task<bool> DeleteTopOfTop(int id);

    } 

    public class GetTopOfTopDto
    {
        public int Id { get; set; }  
        public int ItemId { get; set; } 
        public string ItemType { get; set; } 
        public string NameEn { get; set; } 
        public string DescriptionEn { get; set; } 
        public string NameAr { get; set; } 
        public string DescriptionAr { get; set; } 
        public string Highlight { get; set; } 
        public string PhotoUri { get; set; }
    }
    public class CreateTopOfTopDto
    { 
        public int ItemId { get; set; } 
        public string ItemType { get; set; } 
        public string NameEn { get; set; } 
        public string DescriptionEn { get; set; } 
        public string Highlight { get; set; } 
        public string PhotoUri { get; set; } 
        public string NameAr { get; set; } 
        public string DescriptionAr { get; set; }
        public UploadPhotoRequest UploadRequests { get; set; }
    }

    public class UpdateTopOfTopDto
    {
        public int Id { get; set; }  
        public int ItemId { get; set; } 
        public string ItemType { get; set; } 
        public string NameEn { get; set; } 
        public string DescriptionEn { get; set; } 
        public string Highlight { get; set; } 
        public string PhotoUri { get; set; } 
        public string NameAr { get; set; } 
        public string DescriptionAr { get; set; }
        public UploadPhotoRequest UploadRequests { get; set; }
    }

    public class GetTopOfTopFilter : PaginationInfoFilter
    { 
        public int ItemId { get; set; } 
        public string ItemType { get; set; } 
        public string NameEn { get; set; }  
        public string DescriptionEn { get; set; } 
        public string NameAr { get; set; } 
        public string DescriptionAr { get; set; } 
        public string Highlight { get; set; } 
        public string PhotoUri { get; set; }
    } 
}
