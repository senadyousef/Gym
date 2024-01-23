using AutoMapper;
using Boilerplate.Application.DTOs;
using Boilerplate.Application.Interfaces;
using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Boilerplate.Application.Services
{
    public class UserItemsService : IUserItemsService
    {
        private IUploadService _uploadService;
        private IUserItemsRepository _UserItemsRepository;
        private IItemsRepository _itemsRepository;
        private IMapper _mapper;
        private ICurrentUserService _currentUserService;

        public UserItemsService(IUserItemsRepository UserItemsRepository, IMapper mapper,
            ICurrentUserService currentUserService, IItemsRepository itemsRepository,
             IUploadService uploadService)
        {
            _UserItemsRepository = UserItemsRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _uploadService = uploadService;
            _itemsRepository = itemsRepository;
        }
        public async Task<GetUserItemsDto> CreateUserItems(CreateUserItemsDto UserItems)
        {
            var newUserItems = new UserItems
            {
                UserId = UserItems.UserId,
                ItemsId = UserItems.ItemsId,
                CreatedOn = DateTime.Now,
                IsDisabled = false
            };

            _UserItemsRepository.Create(newUserItems);
            await _UserItemsRepository.SaveChangesAsync();

            var UserItemsDto = new GetUserItemsDto
            {
                Id = newUserItems.Id,
                UserId = UserItems.UserId,
                ItemsId = UserItems.ItemsId,
            };
            return UserItemsDto;
        }
        public async Task<bool> DeleteUserItems(int id)
        {
            var originalUserItems = await _UserItemsRepository.GetById(id);
            if (originalUserItems == null) return false;

            originalUserItems.IsDisabled = true;
            originalUserItems.DisabledOn = DateTime.Now;
            originalUserItems.DisabledBy = _currentUserService.UserId;
            _UserItemsRepository.Update(originalUserItems);
            await _UserItemsRepository.SaveChangesAsync();

            return true;
        }
        public async Task<GetUserItemsDto> GetUserItemsById(int id)
        {
            var ids = _currentUserService.UserId;
            var UserItems = await _UserItemsRepository.GetById(id);

            if (UserItems == null)
                return null;

            var UserItemsDto = new GetUserItemsDto
            {
                Id = UserItems.Id,
                UserId = UserItems.UserId,
                ItemsId = UserItems.ItemsId,
            };
            return UserItemsDto;
        }

        public async Task<GetUserItemsDto> UpdateUserItems(int id, UpdateUserItemsDto updatedUserItems)
        {
            var originalUserItems = await _UserItemsRepository.GetById(id);
            if (originalUserItems == null) return null;

            originalUserItems.UserId = updatedUserItems.UserId;
            originalUserItems.ItemsId = updatedUserItems.ItemsId;

            var UserItemsDto = new GetUserItemsDto
            {
                Id = originalUserItems.Id,
                UserId = originalUserItems.UserId,
                ItemsId = originalUserItems.ItemsId,
            };
            _UserItemsRepository.Update(originalUserItems);
            await _UserItemsRepository.SaveChangesAsync();

            return UserItemsDto;
        }

        public async Task<PaginatedList<GetUserItemsDto>> GetAllUserItemsWithPageSize(GetUserItemsFilter filter)
        {
            var id = _currentUserService.UserId;

            filter ??= new GetUserItemsFilter();
            IQueryable<UserItems> UserItems = null;

            UserItems = _UserItemsRepository
               .GetAll()
               .Where(o => o.IsDisabled == false);

            return await _mapper.ProjectTo<GetUserItemsDto>(UserItems).ToPaginatedListAsync(filter.CurrentPage, filter.PageSize);
        }
        public async Task<AllList<GetUserItemsDto>> GetAllUserItems(GetUserItemsFilter filter)
        {
            var id = _currentUserService.UserId;

            filter ??= new GetUserItemsFilter();
            IQueryable<UserItems> UserItems = null;

            UserItems = _UserItemsRepository
               .GetAll()
               .Where(o => o.IsDisabled == false)
               .Where(o => o.UserId == filter.UserId || filter.UserId == 0)
               .Where(o => o.ItemsId == filter.ItemsId || filter.ItemsId == 0)
               .Where(o => o.CreatedOn.Date == filter.Date.Date || filter.Date == new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc).Date);

            return await _mapper.ProjectTo<GetUserItemsDto>(UserItems).ToAllListAsync(filter.CurrentPage);
        }
        public List<NumberOfBoughtItems> GetAllBoughtItems(GetUserItemsFilter filter)
        {
            var id = _currentUserService.UserId;

            filter ??= new GetUserItemsFilter();
            IQueryable<UserItems> UserItems = null;
            IQueryable<Items> Items = null;

            List<NumberOfBoughtItems> numberOfBoughtItems = new List<NumberOfBoughtItems>();

            if (filter.ItemsId == 0)
            {
                Items = _itemsRepository
                               .GetAll()
                               .Where(o => o.IsDisabled == false);

                foreach (var item in Items)
                {
                    UserItems = null;
                    UserItems = _UserItemsRepository
                       .GetAll()
                       .Where(o => o.IsDisabled == false)
                       .Where(o => o.ItemsId == item.Id)
                       .Where(o => o.CreatedOn.Date == filter.Date.Date || filter.Date == new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc).Date);

                    NumberOfBoughtItems numberOf = new NumberOfBoughtItems();
                    numberOf.Number = UserItems.Count();
                    numberOf.ItemsId = filter.ItemsId;
                    numberOfBoughtItems.Add(new NumberOfBoughtItems());
                }
            }
            else
            {
                UserItems = null;
                UserItems = _UserItemsRepository
                   .GetAll()
                   .Where(o => o.IsDisabled == false)
                   .Where(o => o.ItemsId == filter.ItemsId || filter.ItemsId == 0)
                   .Where(o => o.CreatedOn.Date == filter.Date.Date || filter.Date == new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc).Date);

                NumberOfBoughtItems numberOf = new NumberOfBoughtItems();
                numberOf.Number = UserItems.Count();
                numberOf.ItemsId = filter.ItemsId;
                numberOfBoughtItems.Add(new NumberOfBoughtItems());
            }

            return numberOfBoughtItems;
        }
    }
}