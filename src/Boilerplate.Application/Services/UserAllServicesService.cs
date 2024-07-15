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

namespace Boilerplate.Application.UserAllServices
{
    public class UserAllServicesService : IUserAllServicesService
    {
        private IUploadService _uploadService;
        private IUserAllServicesRepository _UserAllServicesRepository;
        private IMapper _mapper;
        private ICurrentUserService _currentUserService;

        public UserAllServicesService(IUserAllServicesRepository UserAllServicesRepository, IMapper mapper, ICurrentUserService currentUserService,
               IUploadService uploadService)
        {
            _UserAllServicesRepository = UserAllServicesRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _uploadService = uploadService;
        }
        public async Task<GetUserAllServicesDto> CreateUserAllServices(CreateUserAllServicesDto UserAllServices)
        {
            var newUserAllServices = new Domain.Entities.UserAllServices
            {
                UserId = UserAllServices.UserId,
                AllServicesId = UserAllServices.AllServicesId, 
                CreatedOn = DateTime.Now, 
                IsDisabled = false,
            }; 
            
            _UserAllServicesRepository.Create(newUserAllServices);
            await _UserAllServicesRepository.SaveChangesAsync();

            var UserAllServicesDto = new GetUserAllServicesDto
            {
                Id = newUserAllServices.Id,
                UserId = newUserAllServices.UserId,
                AllServicesId = newUserAllServices.AllServicesId, 
            };

            return UserAllServicesDto;
        }
        public async Task<bool> DeleteUserAllServices(int id)
        {
            var originalUserAllServices = await _UserAllServicesRepository.GetById(id);
            if (originalUserAllServices == null) return false;

            originalUserAllServices.IsDisabled = true;
            originalUserAllServices.DisabledOn = DateTime.Now;
            originalUserAllServices.DisabledBy = _currentUserService.UserId;
            _UserAllServicesRepository.Update(originalUserAllServices);
            await _UserAllServicesRepository.SaveChangesAsync();

            return true;
        }


        public async Task<GetUserAllServicesDto> GetUserAllServicesById(int id)
        {
            var ids = _currentUserService.UserId;
            var UserAllServices = await _UserAllServicesRepository.GetById(id);
            if (UserAllServices == null)
                return null;
            var UserAllServicesDto = new GetUserAllServicesDto
            {
                Id = UserAllServices.Id,
                UserId = UserAllServices.UserId,
                AllServicesId = UserAllServices.AllServicesId, 
            };
            return UserAllServicesDto;
        }

        public async Task<GetUserAllServicesDto> UpdateUserAllServices(int id, UpdateUserAllServicesDto updatedUserAllServices)
        {
            var originalUserAllServices = await _UserAllServicesRepository.GetById(id);
            if (originalUserAllServices == null) return null;

            originalUserAllServices.UserId = updatedUserAllServices.UserId;
            originalUserAllServices.AllServicesId = updatedUserAllServices.AllServicesId; 

            var UserAllServicesDto = new GetUserAllServicesDto
            {

                Id = originalUserAllServices.Id,
                UserId = originalUserAllServices.UserId,
                AllServicesId = originalUserAllServices.AllServicesId, 
            };
            _UserAllServicesRepository.Update(originalUserAllServices);
            await _UserAllServicesRepository.SaveChangesAsync();

            return UserAllServicesDto;
        }

        public async Task<PaginatedList<GetUserAllServicesDto>> GetAllUserAllServicesWithPageSize(GetUserAllServicesFilter filter)
        {
            var id = _currentUserService.UserId; 
            filter ??= new GetUserAllServicesFilter();
            IQueryable<Domain.Entities.UserAllServices> UserAllServices = null; 
            UserAllServices = _UserAllServicesRepository
               .GetAll()
               .Where(o => o.IsDisabled == false);  
            return await _mapper.ProjectTo<GetUserAllServicesDto>(UserAllServices).ToPaginatedListAsync(filter.CurrentPage, filter.PageSize);
        } 

        public async Task<AllList<GetUserAllServicesDto>> GetAllUserAllServices(GetUserAllServicesFilter filter)
        {
            var id = _currentUserService.UserId; 
            filter ??= new GetUserAllServicesFilter();
            IQueryable<Domain.Entities.UserAllServices> UserAllServices = null; 
            UserAllServices = _UserAllServicesRepository
               .GetAll()
               .Where(o => o.IsDisabled == false); 
            return await _mapper.ProjectTo<GetUserAllServicesDto>(UserAllServices).ToAllListAsync(filter.CurrentPage);
        } 
    }
}