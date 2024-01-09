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
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Boilerplate.Application.Services
{
    public class ChildrenService : IChildrenService
    {
        private IUploadService _uploadService;
        private IChildrenRepository _ChildrenRepository;
        private IMapper _mapper;
        private ICurrentUserService _currentUserService;

        public ChildrenService(IChildrenRepository ChildrenRepository, IMapper mapper,
            ICurrentUserService currentUserService,
             IUploadService uploadService)
        {
            _ChildrenRepository = ChildrenRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _uploadService = uploadService;
        }
        public async Task<GetChildrenDto> CreateChildren(CreateChildrenDto Children)
        {

            var newChildren = new Children
            {
                NameEn = Children.NameEn,
                NameAr = Children.NameAr,
                UserId = Children.UserId,
                Email = Children.Email,
                Password = Children.Password,
                Role = Children.Role,
                Gender = Children.Gender,
                Age = Children.Age,
                BOD = Children.BOD,
                MembershipStatus = Children.MembershipStatus,
                MembershipExpDate = Children.MembershipExpDate,
                MobilePhone = Children.MobilePhone,
                PhotoUri = Children.PhotoUri,
                CreatedOn = DateTime.Now,
                IsDisabled = false
            };
            if (Children.UploadRequests != null)
            {
                newChildren.PhotoUri = await _uploadService.UploadImageAsync(Children.UploadRequests);
            } 
            var ChildrenDto = new GetChildrenDto
            {
                Id = newChildren.Id,
                NameEn = newChildren.NameEn,
                NameAr = newChildren.NameAr,
                UserId = newChildren.UserId,
                Email = newChildren.Email,
                Password = newChildren.Password,
                Role = newChildren.Role,
                Gender = newChildren.Gender,
                Age = newChildren.Age,
                BOD = newChildren.BOD,
                MembershipStatus = newChildren.MembershipStatus,
                MembershipExpDate = newChildren.MembershipExpDate,
                MobilePhone = newChildren.MobilePhone,
                PhotoUri = newChildren.PhotoUri,
            };

            _ChildrenRepository.Create(newChildren);
            await _ChildrenRepository.SaveChangesAsync();
            return ChildrenDto;
        }
        public async Task<bool> DeleteChildren(int id)
        {
            var originalChildren = await _ChildrenRepository.GetById(id);
            if (originalChildren == null) return false;

            originalChildren.IsDisabled = true;
            originalChildren.DisabledOn = DateTime.Now;
            originalChildren.DisabledBy = _currentUserService.UserId;
            _ChildrenRepository.Update(originalChildren);
            await _ChildrenRepository.SaveChangesAsync();

            return true;
        }


        public async Task<GetChildrenDto> GetChildrenById(int id)
        {
            var ids = _currentUserService.UserId;
            //var Children = await _ChildrenRepository.GetById(id);
            var Children = await _ChildrenRepository.GetAll() 
                .FirstOrDefaultAsync(o => o.Id == id);
            if (Children == null)
                return null;
            var ChildrenDto = new GetChildrenDto
            { 
                Id = Children.Id,
                NameEn = Children.NameEn,
                NameAr = Children.NameAr,
                UserId = Children.UserId,
                Email = Children.Email,
                Password = Children.Password,
                Role = Children.Role,
                Gender = Children.Gender,
                Age = Children.Age,
                BOD = Children.BOD,
                MembershipStatus = Children.MembershipStatus,
                MembershipExpDate = Children.MembershipExpDate,
                MobilePhone = Children.MobilePhone,
                PhotoUri = Children.PhotoUri,
            };
            return ChildrenDto;
        }

        public async Task<GetChildrenDto> UpdateChildren(int id, UpdateChildrenDto updatedChildren)
        {
            var originalChildren = await _ChildrenRepository.GetById(id);
            if (originalChildren == null) return null;

            originalChildren.NameAr = updatedChildren.NameAr;
            originalChildren.NameEn = updatedChildren.NameEn;
            originalChildren.UserId = updatedChildren.UserId;
            originalChildren.Email = updatedChildren.Email;
            originalChildren. Password = updatedChildren.Password;
            originalChildren. Role = updatedChildren.Role;
            originalChildren. Gender = updatedChildren.Gender;
            originalChildren. Age = updatedChildren.Age;
            originalChildren. BOD = updatedChildren.BOD;
            originalChildren. MembershipStatus = updatedChildren.MembershipStatus;
            originalChildren. MembershipExpDate = updatedChildren.MembershipExpDate;
            originalChildren. MobilePhone = updatedChildren.MobilePhone;
            originalChildren.PhotoUri = originalChildren.PhotoUri;
            if (updatedChildren.UploadRequests != null)
            {
                originalChildren.PhotoUri = await _uploadService.UploadImageAsync(updatedChildren.UploadRequests);
            }
             
            var ChildrenDto = new GetChildrenDto
            {

                Id = originalChildren.Id,
                NameEn = originalChildren.NameEn,
                NameAr = originalChildren.NameAr,
                UserId = originalChildren.UserId,
                Email = originalChildren.Email,
                Password = originalChildren.Password,
                Role = originalChildren.Role,
                Gender = originalChildren.Gender,
                Age = originalChildren.Age,
                BOD = originalChildren.BOD,
                MembershipStatus = originalChildren.MembershipStatus,
                MembershipExpDate = originalChildren.MembershipExpDate,
                MobilePhone = originalChildren.MobilePhone,
                PhotoUri = originalChildren.PhotoUri,
            };
            _ChildrenRepository.Update(originalChildren);
            await _ChildrenRepository.SaveChangesAsync();

            return ChildrenDto;
        }

        public async Task<PaginatedList<GetChildrenDto>> GetAllChildrenWithPageSize(GetChildrenFilter filter)
        {
            var id = _currentUserService.UserId;

            filter ??= new GetChildrenFilter();
            IQueryable<Children> Children = null;


            Children = _ChildrenRepository
               .GetAll()
               .Where(o => o.IsDisabled == false);

            return await _mapper.ProjectTo<GetChildrenDto>(Children).ToPaginatedListAsync(filter.CurrentPage, filter.PageSize);
        }

        public async Task<AllList<GetChildrenDto>> GetAllChildren(GetChildrenFilter filter)
        {
            var id = _currentUserService.UserId;

            filter ??= new GetChildrenFilter();
            IQueryable<Children> Children = null;

            Children = _ChildrenRepository
               .GetAll()
               .Where(o => o.IsDisabled == false)
               .Where(o => o.NameEn.Contains(filter.NameEn) || filter.NameEn == null)
               .Where(o => o.NameAr.Contains(filter.NameAr) || filter.NameAr == null);

            return await _mapper.ProjectTo<GetChildrenDto>(Children).ToAllListAsync(filter.CurrentPage);
        }

    }
}




