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
    public interface ICartsService
    {
        public Task<PaginatedList<GetCartsDto>> GetAllCartsWithPageSize(GetCartsFilter filter);
        public Task<AllList<GetCartsDto>> GetAllCarts(GetCartsFilter filter); 
        public Task<GetCartsDto> CreateCarts(CreateCartsDto Carts);
        public Task<GetCartsDto> GetCartsById(int id); 
        public Task<GetCartsDto> UpdateCarts(int id, UpdateCartsDto updatedCarts); 
        public Task<bool> DeleteCarts(int id); 
    }

    public class GetCartsDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ItemsId { get; set; }
    }
    public class CreateCartsDto
    { 
        public int UserId { get; set; }
        public int ItemsId { get; set; }
    }

    public class UpdateCartsDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ItemsId { get; set; }
    }

    public class GetCartsFilter : PaginationInfoFilter
    {
        public int UserId { get; set; }
        public int ItemsId { get; set; }
    } 
}
