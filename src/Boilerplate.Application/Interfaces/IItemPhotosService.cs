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
    public interface IItemPhotosService
    {
        public Task<PaginatedList<GetItemPhotosDto>> GetAllItemPhotos(GetItemPhotosFilter filter);

        public Task<GetItemPhotosDto> CreateItemPhotos(CreateItemPhotosDto ItemPhotos);
        public Task<GetItemPhotosDto> GetItemPhotosById(int id);

        public Task<GetItemPhotosDto> UpdateItemPhotos(int id, UpdateItemPhotosDto updatedItemPhotos);

        public Task<bool> DeleteItemPhotos(int id);

    } 

    public class GetItemPhotosDto
    {
        public int Id { get; set; }

        public int ItemsId { get; set; }
 
        public string PhotoUri { get; set; }
    }
    public class CreateItemPhotosDto
    {
        public int ItemsId { get; set; }

        public string PhotoUri { get; set; }
    }

    public class UpdateItemPhotosDto
    {
        public int Id { get; set; }

        public int ItemsId { get; set; }

        public string PhotoUri { get; set; }
    }

    public class GetItemPhotosFilter : PaginationInfoFilter
    {
        public string ItemsId { get; set; }

        public int PhotoUri { get; set; }
    }

}
