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
    public interface IEventsService
    {
        public Task<PaginatedList<GetEventsDto>> GetAllEventsWithPageSize(GetEventsFilter filter);
        public Task<AllList<GetEventsDto>> GetAllEvents(GetEventsFilter filter); 
        public Task<GetEventsDto> CreateEvents(CreateEventsDto Events);
        public Task<GetEventsDto> GetEventsById(int id); 
        public Task<GetEventsDto> UpdateEvents(int id, UpdateEventsDto updatedEvents); 
        public Task<bool> DeleteEvents(int id);
        public Task<List<GetEventsByDates>> GetDates(int id,int year, int month);
    }

    public class GetEventsByDates
    {
        public DateTime Date { get; set; }
        public bool IsDateHaveEvent { get; set; }
        //public bool IsUserHasBooked { get; set; }
    }
    public class GetEventsDto
    {
        public int Id {  get; set; }
        public int UserId { get; set; } 
        public string NameAr { get; set; } 
        public string NameEn { get; set; } 
        public string DescriptionEn { get; set; }  
        public string DescriptionAr { get; set; } 
        public string PhotoUri { get; set; } 
        public DateTime Date { get; set; } 
        public string From { get; set; } 
        public string To { get; set; } 
        public int Capacity { get; set; } 
        public int Booked { get; set; } 
        public string Type { get; set; }

    }
    public class CreateEventsDto
    {
        public int UserId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public UploadRequest UploadRequests { get; set; }
        public string PhotoUri { get; set; }
        public DateTime Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int Capacity { get; set; }
        public int Booked { get; set; }
        public string Type { get; set; }
    }

    public class UpdateEventsDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public UploadRequest UploadRequests { get; set; }
        public string PhotoUri { get; set; }
        public DateTime Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int Capacity { get; set; }
        public int Booked { get; set; }
        public string Type { get; set; }
    }

    public class GetEventsFilter : PaginationInfoFilter
    {
        public int UserId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public string PhotoUri { get; set; }
        public DateTime Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int Capacity { get; set; }
        public int Booked { get; set; }
        public string Type { get; set; }
    }

}
