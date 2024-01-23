using Boilerplate.Application.DTOs;
using Boilerplate.Application.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Boilerplate.Application.Interfaces
{
    public interface IUserItemsService
    {
        public Task<PaginatedList<GetUserItemsDto>> GetAllUserItemsWithPageSize(GetUserItemsFilter filter);
        public Task<AllList<GetUserItemsDto>> GetAllUserItems(GetUserItemsFilter filter); 
        public List<NumberOfBoughtItems> GetAllBoughtItems(GetUserItemsFilter filter); 
        public Task<GetUserItemsDto> CreateUserItems(CreateUserItemsDto UserItems);
        public Task<GetUserItemsDto> GetUserItemsById(int id); 
        public Task<GetUserItemsDto> UpdateUserItems(int id, UpdateUserItemsDto updatedUserItems); 
        public Task<bool> DeleteUserItems(int id); 
    } 
    public class GetUserItemsDto
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public int ItemsId { get; set; } 
    }
    public class CreateUserItemsDto
    {
        public int UserId { get; set; }
        public int ItemsId { get; set; }
    } 
    public class UpdateUserItemsDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ItemsId { get; set; }
    }  
    public class GetUserItemsFilter : PaginationInfoFilter
    {
        public int UserId { get; set; }
        public int ItemsId { get; set; }
        public DateTime Date { get; set; }
    }
    public class NumberOfBoughtItems
    { 
        public int Number { get; set; }
        public int ItemsId { get; set; }
    }
} 