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
    public class TopOfTopService : ITopOfTopService
    {
        private IUploadService _uploadService;
        private ITopOfTopRepository _TopOfTopRepository;
        private IMapper _mapper;
        private ICurrentUserService _currentUserService;

        public TopOfTopService(ITopOfTopRepository TopOfTopRepository, IMapper mapper, ICurrentUserService currentUserService,
               IUploadService uploadService)
        {
            _TopOfTopRepository = TopOfTopRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _uploadService = uploadService;  
        }
        public async Task<GetTopOfTopDto> CreateTopOfTop(CreateTopOfTopDto TopOfTop)
        {
            foreach (var item in TopOfTop.UploadRequests)
            {
                TopOfTop.PhotoUri = _uploadService.UploadAsync(item);

            }

            var newTopOfTop = new TopOfTop
            { 
                ItemId = TopOfTop.ItemId, 
                ItemType = TopOfTop.ItemType, 
                DescriptionEn = TopOfTop.DescriptionEn, 
                NameAr = TopOfTop.NameAr, 
                DescriptionAr = TopOfTop.DescriptionAr, 
                NameEn = TopOfTop.NameEn, 
                Highlight = TopOfTop.Highlight, 
                CreatedOn = DateTime.Now,
                PhotoUri = TopOfTop.PhotoUri,
                IsDisabled = false,
            };
          
 
            var TopOfTopDto = new GetTopOfTopDto
            {

                Id = newTopOfTop.Id, 
                ItemId = TopOfTop.ItemId,
                ItemType = TopOfTop.ItemType,
                DescriptionEn = TopOfTop.DescriptionEn,
                NameAr = TopOfTop.NameAr,
                DescriptionAr = TopOfTop.DescriptionAr,
                NameEn = TopOfTop.NameEn,
                Highlight = TopOfTop.Highlight,
                PhotoUri = TopOfTop.PhotoUri,

            };

            _TopOfTopRepository.Create(newTopOfTop);
            await _TopOfTopRepository.SaveChangesAsync();
            return TopOfTopDto;
        }
        public async Task<bool> DeleteTopOfTop(int id)
        {
            var originalTopOfTop = await _TopOfTopRepository.GetById(id);
            if (originalTopOfTop == null) return false;

            originalTopOfTop.IsDisabled = true;
            originalTopOfTop.DisabledOn = DateTime.Now;
            originalTopOfTop.DisabledBy = _currentUserService.UserId;
            _TopOfTopRepository.Update(originalTopOfTop);
            await _TopOfTopRepository.SaveChangesAsync();

            return true; 
        }


        public async Task<GetTopOfTopDto> GetTopOfTopById(int id)
        {
            var ids = _currentUserService.UserId;
            var TopOfTop = await _TopOfTopRepository.GetById(id);
            if (TopOfTop == null)
                return null;
            var TopOfTopDto = new GetTopOfTopDto
            {

                Id = TopOfTop.Id, 
                ItemId = TopOfTop.ItemId,
                ItemType = TopOfTop.ItemType,
                DescriptionEn = TopOfTop.DescriptionEn,
                DescriptionAr = TopOfTop.DescriptionAr,
                NameAr = TopOfTop.NameAr,
                NameEn = TopOfTop.NameEn,
                Highlight = TopOfTop.Highlight,
                PhotoUri = TopOfTop.PhotoUri,
            };
            return TopOfTopDto;
        }

        public async Task<GetTopOfTopDto> UpdateTopOfTop(int id, UpdateTopOfTopDto updatedTopOfTop)
        {
            var originalTopOfTop = await _TopOfTopRepository.GetById(id);
            if (originalTopOfTop == null) return null;
             
            originalTopOfTop.ItemId = updatedTopOfTop.ItemId;  
            originalTopOfTop.ItemType = updatedTopOfTop.ItemType;  
            originalTopOfTop.NameEn = updatedTopOfTop.NameEn;  
            originalTopOfTop.NameAr = updatedTopOfTop.NameAr;  
            originalTopOfTop.DescriptionAr = updatedTopOfTop.DescriptionAr;  
            originalTopOfTop.DescriptionEn= updatedTopOfTop.DescriptionEn;  
            originalTopOfTop.Highlight= updatedTopOfTop.Highlight;  
            originalTopOfTop.PhotoUri= updatedTopOfTop.PhotoUri;  

            var TopOfTopDto = new GetTopOfTopDto
            {

                Id = originalTopOfTop.Id, 
                ItemId = originalTopOfTop.ItemId,
                ItemType = originalTopOfTop.ItemType,
                DescriptionEn = originalTopOfTop.DescriptionEn,
                DescriptionAr = originalTopOfTop.DescriptionAr,
                NameAr = originalTopOfTop.NameAr,
                NameEn = originalTopOfTop.NameEn,
                Highlight = originalTopOfTop.Highlight,
                PhotoUri = originalTopOfTop.PhotoUri,
            };
            _TopOfTopRepository.Update(originalTopOfTop);
            await _TopOfTopRepository.SaveChangesAsync();

            return TopOfTopDto;
        }

        public async Task<PaginatedList<GetTopOfTopDto>> GetAllTopOfTop(GetTopOfTopFilter filter)
        {
            var id = _currentUserService.UserId;

            filter ??= new GetTopOfTopFilter();
            IQueryable<TopOfTop> TopOfTop = null;

            //if (_currentUserService.Role == "SuperAdmin" || _currentUserService.Role == "Customer")
            //{
                TopOfTop = _TopOfTopRepository
                   .GetAll()
                   .Where(o => o.IsDisabled == false) 
              .WhereIf(!string.IsNullOrEmpty(filter.ItemType), x => EF.Functions.Like(x.ItemType, $"%{filter.ItemType}%"));
            //}
              
            return await _mapper.ProjectTo<GetTopOfTopDto>(TopOfTop).ToPaginatedListAsync(filter.CurrentPage, filter.PageSize);
        }

    }
}