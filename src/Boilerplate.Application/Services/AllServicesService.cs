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

namespace Boilerplate.Application.AllServices
{
    public class AllServicesService : IAllServicesService
    {
        private IUploadService _uploadService;
        private IAllServicesRepository _AllServicesRepository;
        private IMapper _mapper;
        private ICurrentUserService _currentUserService;

        public AllServicesService(IAllServicesRepository AllServicesRepository, IMapper mapper, ICurrentUserService currentUserService,
               IUploadService uploadService)
        {
            _AllServicesRepository = AllServicesRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _uploadService = uploadService;
        }
        public async Task<GetAllServicesDto> CreateAllServices(CreateAllServicesDto AllServices)
        {
            var newAllServices = new Domain.Entities.AllServices
            {
                NameEn = AllServices.NameEn,
                NameAr = AllServices.NameAr, 
                CreatedOn = DateTime.Now, 
                IsDisabled = false,
            }; 
            
            _AllServicesRepository.Create(newAllServices);
            await _AllServicesRepository.SaveChangesAsync();

            var AllServicesDto = new GetAllServicesDto
            {
                Id = newAllServices.Id,
                NameAr = newAllServices.NameAr,
                NameEn = newAllServices.NameEn, 
            };

            return AllServicesDto;
        }
        public async Task<bool> DeleteAllServices(int id)
        {
            var originalAllServices = await _AllServicesRepository.GetById(id);
            if (originalAllServices == null) return false;

            originalAllServices.IsDisabled = true;
            originalAllServices.DisabledOn = DateTime.Now;
            originalAllServices.DisabledBy = _currentUserService.UserId;
            _AllServicesRepository.Update(originalAllServices);
            await _AllServicesRepository.SaveChangesAsync();

            return true;
        }


        public async Task<GetAllServicesDto> GetAllServicesById(int id)
        {
            var ids = _currentUserService.UserId;
            var AllServices = await _AllServicesRepository.GetById(id);
            if (AllServices == null)
                return null;
            var AllServicesDto = new GetAllServicesDto
            {
                Id = AllServices.Id,
                NameEn = AllServices.NameEn,
                NameAr = AllServices.NameAr, 
            };
            return AllServicesDto;
        }

        public async Task<GetAllServicesDto> UpdateAllServices(int id, UpdateAllServicesDto updatedAllServices)
        {
            var originalAllServices = await _AllServicesRepository.GetById(id);
            if (originalAllServices == null) return null;

            originalAllServices.NameAr = updatedAllServices.NameAr;
            originalAllServices.NameEn = updatedAllServices.NameEn; 

            var AllServicesDto = new GetAllServicesDto
            {

                Id = originalAllServices.Id,
                NameEn = originalAllServices.NameEn,
                NameAr = originalAllServices.NameAr, 
            };
            _AllServicesRepository.Update(originalAllServices);
            await _AllServicesRepository.SaveChangesAsync();

            return AllServicesDto;
        }

        public async Task<PaginatedList<GetAllServicesDto>> GetAllAllServicesWithPageSize(GetAllServicesFilter filter)
        {
            var id = _currentUserService.UserId; 
            filter ??= new GetAllServicesFilter();
            IQueryable<Domain.Entities.AllServices> AllServices = null; 
            AllServices = _AllServicesRepository
               .GetAll()
               .Where(o => o.IsDisabled == false);  
            return await _mapper.ProjectTo<GetAllServicesDto>(AllServices).ToPaginatedListAsync(filter.CurrentPage, filter.PageSize);
        } 

        public async Task<AllList<GetAllServicesDto>> GetAllAllServices(GetAllServicesFilter filter)
        {
            var id = _currentUserService.UserId; 
            filter ??= new GetAllServicesFilter();
            IQueryable<Domain.Entities.AllServices> AllServices = null; 
            AllServices = _AllServicesRepository
               .GetAll()
               .Where(o => o.IsDisabled == false); 
            return await _mapper.ProjectTo<GetAllServicesDto>(AllServices).ToAllListAsync(filter.CurrentPage);
        } 
    }
}