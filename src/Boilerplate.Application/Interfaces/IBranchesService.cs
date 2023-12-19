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
    public interface IBranchesService
    {
        public Task<PaginatedList<GetBranchesDto>> GetAllBranchesWithPageSize(GetBranchesFilter filter);
        public Task<AllList<GetBranchesDto>> GetAllBranches(GetBranchesFilter filter); 
        public Task<GetBranchesDto> CreateBranches(CreateBranchesDto Branches);
        public Task<GetBranchesDto> GetBranchesById(int id); 
        public Task<GetBranchesDto> UpdateBranches(int id, UpdateBranchesDto updatedBranches); 
        public Task<bool> DeleteBranches(int id); 
    }

    public class GetBranchesDto
    {
        public int Id { get; set; } 
        public string NameEn { get; set; }
        public string NameAr { get; set; } 
    }
    public class CreateBranchesDto
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; } 
    } 
    public class UpdateBranchesDto
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; } 
    } 
    public class GetBranchesFilter : PaginationInfoFilter
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; } 
    } 
}
