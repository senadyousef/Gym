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
    public interface INewsService
    {
        public Task<PaginatedList<GetNewsDto>> GetAllNewsWithPageSize(GetNewsFilter filter);
        public Task<AllList<GetNewsDto>> GetAllNews(GetNewsFilter filter);
        public Task<GetNewsDto> CreateNews(CreateNewsDto News);
        public Task<GetNewsDto> GetNewsById(int id); 
        public Task<GetNewsDto> UpdateNews(int id, UpdateNewsDto updatedNews); 
        public Task<bool> DeleteNews(int id);

    } 

    public class GetNewsDto
    {
        public int Id { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public DateTime NewsDate { get; set; }
        public string Highlight { get; set; }
        public string PhotoUri { get; set; }
        public int UserId { get; set; }
    }
    public class CreateNewsDto
    {
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public DateTime NewsDate { get; set; }
        public string Highlight { get; set; }
        public string PhotoUri { get; set; }
        public int UserId { get; set; }
        public UploadPhotoRequest UploadRequests { get; set; }
    }

    public class UpdateNewsDto
    {
        public int Id { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public DateTime NewsDate { get; set; }
        public string Highlight { get; set; }
        public string PhotoUri { get; set; }
        public int UserId { get; set; }
        public UploadPhotoRequest UploadRequests { get; set; }
    }

    public class GetNewsFilter : PaginationInfoFilter
    {
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public DateTime NewsDate { get; set; }
        public string Highlight { get; set; }
        public string PhotoUri { get; set; }
        public int UserId { get; set; }
    } 
}
