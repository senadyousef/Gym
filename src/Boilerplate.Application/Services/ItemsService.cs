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

namespace Boilerplate.Application.Items
{
    public class ItemsService : IItemsService
    {
        private IUploadService _uploadService;
        private IItemsRepository _ItemsRepository;
        private IMapper _mapper;
        private ICurrentUserService _currentUserService;
        private readonly IUserRepository _userRepository;

        public ItemsService(IItemsRepository ItemsRepository, IMapper mapper, ICurrentUserService currentUserService,
               IUploadService uploadService , IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _ItemsRepository = ItemsRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _uploadService = uploadService;
        }
        public async Task<GetItemsDto> CreateItems(CreateItemsDto Items)
        {
            var newItems = new Domain.Entities.Items
            {
                NameEn = Items.NameEn,
                NameAr = Items.NameAr,
                UserId = Items.UserId,
                Price = Items.Price,
                DescriptionEn = Items.DescriptionEn,
                DescriptionAr = Items.DescriptionAr,
                CreatedOn = DateTime.Now,
                IsDisabled = false,
            };

            _ItemsRepository.Create(newItems);
            await _ItemsRepository.SaveChangesAsync();

            var ItemsDto = new GetItemsDto
            {
                Id = newItems.Id,
                NameAr = newItems.NameAr,
                NameEn = newItems.NameEn,
                UserId = newItems.UserId,
                Price = newItems.Price,
                DescriptionEn = newItems.DescriptionEn,
                DescriptionAr = newItems.DescriptionAr,
            };

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
            var Items = await _ItemsRepository.GetById(id);
            if (Items == null)
                return null;
            var ItemsDto = new GetItemsDto
            {
                Id = Items.Id,
                NameEn = Items.NameEn,
                NameAr = Items.NameAr,
                UserId = Items.UserId,
                Price = Items.Price,
                DescriptionEn = Items.DescriptionEn,
                DescriptionAr = Items.DescriptionAr,
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
            originalItems.DescriptionEn = updatedItems.DescriptionEn;
            originalItems.DescriptionAr = updatedItems.DescriptionAr;
            originalItems.UserId = updatedItems.UserId;

            var ItemsDto = new GetItemsDto
            {

                Id = originalItems.Id,
                NameEn = originalItems.NameEn,
                NameAr = originalItems.NameAr,
                UserId = originalItems.UserId,
                Price = originalItems.Price,
                DescriptionEn = originalItems.DescriptionEn,
                DescriptionAr = originalItems.DescriptionAr,
            };
            _ItemsRepository.Update(originalItems);
            await _ItemsRepository.SaveChangesAsync();

            return ItemsDto;
        }

        public async Task<PaginatedList<GetItemsDto>> GetAllItemsWithPageSize(GetItemsFilter filter)
        {

            filter ??= new GetItemsFilter();
            IQueryable<Domain.Entities.Items> Items = null;
            if (filter.UserId != 0)
            {
                var id = _currentUserService.UserId;
                var user = await _userRepository.GetById(int.Parse(id));
                if (user != null)
                {
                    Items = _ItemsRepository
                    .GetAll()
                    .Where(o => o.UserId == user.GymId)
                    .Where(o => o.IsDisabled == false);
                }
            }
            else
            {
                Items = _ItemsRepository
               .GetAll()
               .Where(o => o.UserId == filter.UserId)
               .Where(o => o.IsDisabled == false);
            }
            return await _mapper.ProjectTo<GetItemsDto>(Items).ToPaginatedListAsync(filter.CurrentPage, filter.PageSize);
        }

        public async Task<AllList<GetItemsDto>> GetAllItems(GetItemsFilter filter)
        {
            filter ??= new GetItemsFilter();
            IQueryable<Domain.Entities.Items> Items = null;
            if (filter.UserId == 0)
            {
                var id = _currentUserService.UserId;
                var user = await _userRepository.GetById(int.Parse(id));
                if (user != null)
                {
                    Items = _ItemsRepository
                    .GetAll()
                    .Where(o => o.UserId == user.GymId)
                    .Where(o => o.IsDisabled == false);
                }
            }
            else
            {
                Items = _ItemsRepository
               .GetAll()
               .Where(o => o.UserId == filter.UserId)
               .Where(o => o.IsDisabled == false);
            }
            return await _mapper.ProjectTo<GetItemsDto>(Items).ToAllListAsync(filter.CurrentPage);
        }
    }
}