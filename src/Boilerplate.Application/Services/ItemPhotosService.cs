using AutoMapper;
using Boilerplate.Application.DTOs;
using Boilerplate.Application.Extensions;
using Boilerplate.Application.Interfaces;
using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Boilerplate.Application.Services
{
    public class ItemPhotosService : IItemPhotosService
    {
        private IItemPhotosRepository _ItemPhotosRepository;
        private IMapper _mapper;
        private ICurrentUserService _currentUserService;

        public ItemPhotosService(IItemPhotosRepository ItemPhotosRepository, IMapper mapper, ICurrentUserService currentUserService)
        {
            _ItemPhotosRepository = ItemPhotosRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        public async Task<GetItemPhotosDto> CreateItemPhotos(CreateItemPhotosDto ItemPhotos)
        { 
            var newItemPhotos = new ItemPhotos
            {
                ItemsId = ItemPhotos.ItemsId,
                PhotoUri = ItemPhotos.PhotoUri,
                CreatedOn = DateTime.Now,
                IsDisabled = false
            }; 
            var ItemPhotosDto = new GetItemPhotosDto
            { 
                Id = newItemPhotos.Id,
                ItemsId = ItemPhotos.ItemsId,
                PhotoUri = ItemPhotos.PhotoUri,
            }; 
            _ItemPhotosRepository.Create(newItemPhotos);
            await _ItemPhotosRepository.SaveChangesAsync();
            return ItemPhotosDto;
        }
        public async Task<bool> DeleteItemPhotos(int id)
        {
            var originalItemPhotos = await _ItemPhotosRepository.GetById(id);
            if (originalItemPhotos == null) return false;

            originalItemPhotos.IsDisabled = true;
            originalItemPhotos.DisabledOn = DateTime.Now;
            originalItemPhotos.DisabledBy = _currentUserService.UserId;
            _ItemPhotosRepository.Update(originalItemPhotos);
            await _ItemPhotosRepository.SaveChangesAsync();

            return true;
        }
         
        public async Task<GetItemPhotosDto> GetItemPhotosById(int id)
        {
            var ids = _currentUserService.UserId;
            var subCategories = await _ItemPhotosRepository.GetById(id);

            if (subCategories == null)
                return null;
            var subCategoriesDto = new GetItemPhotosDto
            { 
                Id = subCategories.Id,
                ItemsId = subCategories.ItemsId,
                PhotoUri = subCategories.PhotoUri,
            };
            return subCategoriesDto;
        } 

        public async Task<GetItemPhotosDto> UpdateItemPhotos(int id, UpdateItemPhotosDto updatedItemPhotos)
        {
            var originalItemPhotos = await _ItemPhotosRepository.GetById(id);
            if (originalItemPhotos == null) return null;

            originalItemPhotos.ItemsId = updatedItemPhotos.ItemsId;
            originalItemPhotos.PhotoUri = updatedItemPhotos.PhotoUri;

            var ItemPhotosDto = new GetItemPhotosDto
            { 
                Id = originalItemPhotos.Id,
                ItemsId = originalItemPhotos.ItemsId,
                PhotoUri = originalItemPhotos.PhotoUri,
            };
            _ItemPhotosRepository.Update(originalItemPhotos);
            await _ItemPhotosRepository.SaveChangesAsync(); 
            return ItemPhotosDto;
        }

        public async Task<PaginatedList<GetItemPhotosDto>> GetAllItemPhotos(GetItemPhotosFilter filter)
        {
            var id = _currentUserService.UserId; 
            filter ??= new GetItemPhotosFilter();
            IQueryable<ItemPhotos> ItemPhotos = null; 
            if (_currentUserService.Role == "SuperAdmin")
            {
                ItemPhotos = _ItemPhotosRepository
                   .GetAll()
                   .Where(o => o.IsDisabled == false);
            } 
            return await _mapper.ProjectTo<GetItemPhotosDto>(ItemPhotos).ToPaginatedListAsync(filter.CurrentPage, filter.PageSize);
        } 
    }
}