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

namespace Boilerplate.Application.Gallery
{
    public class GalleryService : IGalleryService
    {
        private IUploadService _uploadService;
        private IGalleryRepository _GalleryRepository;
        private IMapper _mapper;
        private ICurrentUserService _currentUserService;

        public GalleryService(IGalleryRepository GalleryRepository, IMapper mapper, ICurrentUserService currentUserService,
               IUploadService uploadService)
        {
            _GalleryRepository = GalleryRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _uploadService = uploadService;
        }
        public async Task<GetGalleryDto> CreateGallery(CreateGalleryDto Gallery)
        {
            var newGallery = new Domain.Entities.Gallery
            {
                UserId = Gallery.UserId,
                PhotoUrl = Gallery.PhotoUrl,
                CreatedOn = DateTime.Now,
                IsDisabled = false,
            };

            _GalleryRepository.Create(newGallery);
            await _GalleryRepository.SaveChangesAsync();

            var GalleryDto = new GetGalleryDto
            {
                Id = newGallery.Id,
                UserId = newGallery.UserId,
                PhotoUrl = newGallery.PhotoUrl,
            };

            return GalleryDto;
        }
        public async Task<bool> DeleteGallery(int id)
        {
            var originalGallery = await _GalleryRepository.GetById(id);
            if (originalGallery == null) return false;

            originalGallery.IsDisabled = true;
            originalGallery.DisabledOn = DateTime.Now;
            originalGallery.DisabledBy = _currentUserService.UserId;
            _GalleryRepository.Update(originalGallery);
            await _GalleryRepository.SaveChangesAsync();

            return true;
        }


        public async Task<GetGalleryDto> GetGalleryById(int id)
        {
            var ids = _currentUserService.UserId;
            var Gallery = await _GalleryRepository.GetById(id);
            if (Gallery == null)
                return null;
            var GalleryDto = new GetGalleryDto
            {
                Id = Gallery.Id,
                UserId = Gallery.UserId,
                PhotoUrl = Gallery.PhotoUrl,
            };
            return GalleryDto;
        }

        public async Task<GetGalleryDto> UpdateGallery(int id, UpdateGalleryDto updatedGallery)
        {
            var originalGallery = await _GalleryRepository.GetById(id);
            if (originalGallery == null) return null;

            originalGallery.UserId = updatedGallery.UserId;
            originalGallery.PhotoUrl = updatedGallery.PhotoUrl;

            var GalleryDto = new GetGalleryDto
            {

                Id = originalGallery.Id,
                UserId = originalGallery.UserId,
                PhotoUrl = originalGallery.PhotoUrl,
            };
            _GalleryRepository.Update(originalGallery);
            await _GalleryRepository.SaveChangesAsync();

            return GalleryDto;
        }

        public async Task<PaginatedList<GetGalleryDto>> GetAllGalleryWithPageSize(GetGalleryFilter filter)
        {
            var id = _currentUserService.UserId;
            filter ??= new GetGalleryFilter();
            IQueryable<Domain.Entities.Gallery> Gallery = null;
            Gallery = _GalleryRepository
               .GetAll()
               .Where(o => o.UserId == filter.UserId)
               .Where(o => o.IsDisabled == false);
            return await _mapper.ProjectTo<GetGalleryDto>(Gallery).ToPaginatedListAsync(filter.CurrentPage, filter.PageSize);
        }

        public async Task<AllList<GetGalleryDto>> GetAllGallery(GetGalleryFilter filter)
        {
            var id = _currentUserService.UserId;
            filter ??= new GetGalleryFilter();
            IQueryable<Domain.Entities.Gallery> Gallery = null;
            Gallery = _GalleryRepository
               .GetAll()
               .Where(o => o.UserId == filter.UserId)
               .Where(o => o.IsDisabled == false);
            return await _mapper.ProjectTo<GetGalleryDto>(Gallery).ToAllListAsync(filter.CurrentPage);
        }
    }
}