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
    public interface IUserEventsService
    {
        public Task<PaginatedList<GetUserEventsDto>> GetAllUserEventsWithPageSize(GetUserEventsFilter filter);
        public Task<AllList<GetUserEventsDto>> GetAllUserEvents(GetUserEventsFilter filter); 
        public Task<GetUserEventsDto> CreateUserEvents(CreateUserEventsDto UserEvents);
        public Task<GetUserEventsDto> GetUserEventsById(int id); 
        public Task<GetUserEventsDto> UpdateUserEvents(int id, UpdateUserEventsDto updatedUserEvents); 
        public Task<bool> DeleteUserEvents(int id); 
    }

    public class GetUserEventsDto
    {
        public int Id {  get; set; }
        public int UserId { get; set; } 
        public int EventsId { get; set; }
        public Events Events { get; set; }
        public DateTime CreatedOn { get; set; }
    }
    public class CreateUserEventsDto
    {
        public int UserId { get; set; }
        public int EventsId { get; set; }
    }

    public class UpdateUserEventsDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventsId { get; set; }
    }

    public class GetUserEventsFilter : PaginationInfoFilter
    {
        public int UserId { get; set; }
        public int EventsId { get; set; }
    }

}
