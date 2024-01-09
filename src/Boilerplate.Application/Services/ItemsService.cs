using AutoMapper;
using Boilerplate.Application.DTOs;
using Boilerplate.Application.Extensions;
using Boilerplate.Application.Interfaces;
using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Boilerplate.Application.Services
{
    public class ItemsService : IItemsService
    {
        private IUploadService _uploadService;
        private IItemsRepository _ItemsRepository;
        private IItemPhotosRepository _itemPhotosRepository;
        private IMapper _mapper;
        private ICurrentUserService _currentUserService;

        public ItemsService(IItemsRepository ItemsRepository, IMapper mapper,
            ICurrentUserService currentUserService, IItemPhotosRepository itemPhotosRepository,
             IUploadService uploadService)
        {
            _ItemsRepository = ItemsRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _uploadService = uploadService;
            _itemPhotosRepository = itemPhotosRepository;
        }
        public async Task<GetItemsDto> CreateItems(CreateItemsDto Items)
        { 
            var newItems = new Items
            {
                NameEn = Items.NameEn,
                NameAr = Items.NameAr,
                Price = Items.Price,
                Description = Items.Description,
                CreatedOn = DateTime.Now,
                IsDisabled = false
            };
            if (Items.UploadRequests != null)
            {
                foreach (var item in Items.UploadRequests)
                {
                    newItems.ItemPhotos.Add(new ItemPhotos()
                    {
                        PhotoUri = await _uploadService.UploadImageAsync(item),
                        ItemsId = newItems.Id, 
                        CreatedOn = DateTime.Now,
                        IsDisabled = false
                    });
                }
            }

            var ItemsDto = new GetItemsDto
            {
                Id = newItems.Id,
                NameEn = Items.NameEn,
                NameAr = Items.NameAr,
                Price = Items.Price,
                Description = Items.Description
            };

            _ItemsRepository.Create(newItems);
            await _ItemsRepository.SaveChangesAsync();
            if (newItems.ItemPhotos.Count > 0)
            {
                for (int i = 0; i < newItems.ItemPhotos.Count; i++)
                {
                    var itemPhotos = new ItemPhotos
                    {
                        ItemsId = newItems.Id,
                        PhotoUri = newItems.ItemPhotos[i].PhotoUri, 
                        CreatedOn = DateTime.Now,
                        IsDisabled = false
                    };
                    _itemPhotosRepository.Create(itemPhotos);
                    await _itemPhotosRepository.SaveChangesAsync(); 
                }
            }

            return ItemsDto;
        }
        public async Task<bool> DeleteItems(int id)
        {
            var originalItems = await _ItemsRepository.GetById(id);
            if (originalItems == null) return false;

            originalItems.IsDisabled = true;
            originalItems.DisabledOn = DateTime.Now;
            originalItems.DisabledBy = _currentUserService.UserId;
            _ItemsRepository.Update(originalItems);
            await _ItemsRepository.SaveChangesAsync();

            return true;
        }


        public async Task<GetItemsDto> GetItemsById(int id)
        {
            var ids = _currentUserService.UserId;
            //var Items = await _ItemsRepository.GetById(id);
            var Items = await _ItemsRepository.GetAll()

                .Include(o => o.ItemPhotos.Where(x => x.IsDisabled == false))
                .FirstOrDefaultAsync(o => o.Id == id);
            if (Items == null)
                return null;
            var ItemsDto = new GetItemsDto
            {

                Id = Items.Id,
                NameEn = Items.NameEn,
                NameAr = Items.NameAr,
                Price = Items.Price,
                Description = Items.Description,
                ItemPhotos = Items.ItemPhotos,
            };
            return ItemsDto;
        }

        public async Task<GetItemsDto> UpdateItems(int id, UpdateItemsDto updatedItems)
        {
            var originalItems = await _ItemsRepository.GetById(id);
            if (originalItems == null) return null;

            originalItems.NameAr = updatedItems.NameAr;
            originalItems.NameEn = updatedItems.NameEn;
            originalItems.Price = updatedItems.Price;
            originalItems.Description = updatedItems.Description;

            if (updatedItems.UploadRequests != null)
            {
                foreach (var item in updatedItems.UploadRequests)
                {
                    originalItems.ItemPhotos.Add(new ItemPhotos()
                    {
                        PhotoUri = await _uploadService.UploadImageAsync(item),
                        ItemsId = originalItems.Id,
                        CreatedOn = DateTime.Now,
                        IsDisabled = false
                    });
                }
            }

            var ItemsDto = new GetItemsDto
            { 
                Id = originalItems.Id,
                NameEn = originalItems.NameEn,
                NameAr = originalItems.NameAr,
                Price = originalItems.Price,
                Description = originalItems.Description,
            };
            _ItemsRepository.Update(originalItems);
            await _ItemsRepository.SaveChangesAsync();

            return ItemsDto;
        }

        public async Task<PaginatedList<GetItemsDto>> GetAllItemsWithPageSize(GetItemsFilter filter)
        {
            var id = _currentUserService.UserId;

            filter ??= new GetItemsFilter();
            IQueryable<Items> Items = null;


            Items = _ItemsRepository
               .GetAll()
               .Where(o => o.IsDisabled == false);

            return await _mapper.ProjectTo<GetItemsDto>(Items).ToPaginatedListAsync(filter.CurrentPage, filter.PageSize);
        }

        public async Task<AllList<GetItemsDto>> GetAllItems(GetItemsFilter filter)
        {
            var id = _currentUserService.UserId;

            filter ??= new GetItemsFilter();
            IQueryable<Items> Items = null;

            Items = _ItemsRepository
               .GetAll()
               .Where(o => o.IsDisabled == false)
               .Where(o => o.NameEn.Contains(filter.NameEn) || filter.NameEn == null)
               .Where(o => o.Price == filter.Price || filter.Price == 0)
               .Where(o => o.NameAr.Contains(filter.NameAr) || filter.NameAr == null);

            return await _mapper.ProjectTo<GetItemsDto>(Items).ToAllListAsync(filter.CurrentPage);
        }

    }
}




