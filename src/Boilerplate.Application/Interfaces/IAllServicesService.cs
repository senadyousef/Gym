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
    public interface IAllServicesService
    {
        public Task<PaginatedList<GetAllServicesDto>> GetAllAllServicesWithPageSize(GetAllServicesFilter filter);
        public Task<AllList<GetAllServicesDto>> GetAllAllServices(GetAllServicesFilter filter); 
        public Task<GetAllServicesDto> CreateAllServices(CreateAllServicesDto AllServices);
        public Task<GetAllServicesDto> GetAllServicesById(int id); 
        public Task<GetAllServicesDto> UpdateAllServices(int id, UpdateAllServicesDto updatedAllServices); 
        public Task<bool> DeleteAllServices(int id); 
    }

    public class GetAllServicesDto
    {
        public int Id {  get; set; }
        public string NameAr { get; set; } 
        public string NameEn { get; set; }
    }
    public class CreateAllServicesDto
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }

    public class UpdateAllServicesDto
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }

    public class GetAllServicesFilter : PaginationInfoFilter
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }

}
