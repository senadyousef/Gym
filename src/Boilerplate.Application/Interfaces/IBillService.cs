using Boilerplate.Application.DTOs;
using Boilerplate.Application.Filters;
using Boilerplate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Application.Interfaces
{
    public interface IBillService
    {
        public Task<PaginatedList<GetBillDto>> GetAllBillWithPageSize(GetBillFilter filter);
        public Task<AllList<GetBillDto>> GetAllBill(GetBillFilter filter); 
        public Task<GetBillDto> CreateBill(CreateBillDto Bill);
        public Task<GetBillDto> GetBillById(int id); 
        public Task<GetBillDto> UpdateBill(int id, UpdateBillDto updatedBill); 
        public Task<bool> DeleteBill(int id); 
    } 
    public class GetBillDto
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public float Amount { get; set; } 
        public string Status { get; set; }
    }
    public class CreateBillDto
    {
        public int UserId { get; set; }
        public float Amount { get; set; }
        public string Status { get; set; }
    } 
    public class UpdateBillDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public float Amount { get; set; }
        public string Status { get; set; } 
    } 
    public class GetBillFilter : PaginationInfoFilter
    {
        public int UserId { get; set; }
        public float Amount { get; set; }
        public string Status { get; set; }
    } 
}
