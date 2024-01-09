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
    public interface IItemsService
    {
        public Task<PaginatedList<GetItemsDto>> GetAllItemsWithPageSize(GetItemsFilter filter);
        public Task<AllList<GetItemsDto>> GetAllItems(GetItemsFilter filter);

        public Task<GetItemsDto> CreateItems(CreateItemsDto Items);
        public Task<GetItemsDto> GetItemsById(int id);

        public Task<GetItemsDto> UpdateItems(int id, UpdateItemsDto updatedItems);

        public Task<bool> DeleteItems(int id);

    }

    public class GetItemsDto
    {
        public int Id { get; set; } 
        public string NameEn { get; set; } 
        public string NameAr { get; set; } 
        public decimal Price { get; set; }
        public string Description { get; set; }
        public List<ItemPhotos> ItemPhotos { get; set; }

    }
    public class CreateItemsDto
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public List<ItemPhotos> ItemPhotos { get; set; }
        public List<UploadPhotoRequest> UploadRequests { get; set; }
    }

    public class UpdateItemsDto
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public List<ItemPhotos> ItemPhotos { get; set; }
        public List<UploadPhotoRequest> UploadRequests { get; set; }
    }

    public class GetItemsFilter : PaginationInfoFilter
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; } 
    }

}
