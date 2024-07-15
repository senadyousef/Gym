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
        public int Id {  get; set; }
        public int UserId { get; set; } 
        public string NameEn { get; set; } 
        public string NameAr { get; set; }
        public float Price { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
    }
    public class CreateItemsDto
    {
        public int UserId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public float Price { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
    }

    public class UpdateItemsDto
    {
        public int UserId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public float Price { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
    }

    public class GetItemsFilter : PaginationInfoFilter
    {
        public int UserId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public float Price { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
    } 
}
