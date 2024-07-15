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
    public interface IUserAllServicesService
    {
        public Task<PaginatedList<GetUserAllServicesDto>> GetAllUserAllServicesWithPageSize(GetUserAllServicesFilter filter);
        public Task<AllList<GetUserAllServicesDto>> GetAllUserAllServices(GetUserAllServicesFilter filter); 
        public Task<GetUserAllServicesDto> CreateUserAllServices(CreateUserAllServicesDto UserAllServices);
        public Task<GetUserAllServicesDto> GetUserAllServicesById(int id); 
        public Task<GetUserAllServicesDto> UpdateUserAllServices(int id, UpdateUserAllServicesDto updatedUserAllServices); 
        public Task<bool> DeleteUserAllServices(int id); 
    }

    public class GetUserAllServicesDto
    {
        public int Id {  get; set; }
        public int UserId { get; set; } 
        public int AllServicesId { get; set; }
    }
    public class CreateUserAllServicesDto
    {
        public int UserId { get; set; }
        public int AllServicesId { get; set; }
    }

    public class UpdateUserAllServicesDto
    {
        public int UserId { get; set; }
        public int AllServicesId { get; set; }
    }

    public class GetUserAllServicesFilter : PaginationInfoFilter
    {
        public int UserId { get; set; }
        public int AllServicesId { get; set; }
    }

}
