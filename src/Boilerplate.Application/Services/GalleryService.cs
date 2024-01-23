using Amazon.S3.Model;
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
    public class GalleryService : IGalleryService
    {
        private IUploadService _uploadService;
        private IGalleryRepository _GalleryRepository; 
        private IMapper _mapper;
        private ICurrentUserService _currentUserService;

        public GalleryService(IGalleryRepository GalleryRepository, IMapper mapper,
            ICurrentUserService currentUserService,
             IUploadService uploadService)
        {
            _GalleryRepository = GalleryRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _uploadService = uploadService; 
        }
        public async Task<GetGalleryDto> CreateGallery(CreateGalleryDto Gallery)
        {
            var newGallery = new Gallery
            { 
                CreatedOn = DateTime.Now,
                IsDisabled = false
            };

            if (Gallery.UploadRequests != null)
            {
                newGallery.PhotoUri = await _uploadService.UploadImageAsync(Gallery.UploadRequests);
            }
             
            _GalleryRepository.Create(newGallery);
            await _GalleryRepository.SaveChangesAsync();

            var GalleryDto = new GetGalleryDto
            { 
                PhotoUri = Gallery.PhotoUri, 
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
            var Gallery = await _GalleryRepository.GetAll()

                .FirstOrDefaultAsync(o => o.Id == id);
            if (Gallery == null)
                return null;
            var GalleryDto = new GetGalleryDto
            {
                Id = Gallery.Id, 
                PhotoUri = Gallery.PhotoUri, 
            };
            return GalleryDto;
        }

        public async Task<GetGalleryDto> UpdateGallery(int id, UpdateGalleryDto updatedGallery)
        {
            var originalGallery = await _GalleryRepository.GetById(id);
            if (originalGallery == null) return null;
             
            originalGallery.PhotoUri = updatedGallery.PhotoUri;
            if (updatedGallery.UploadRequests != null)
            {
                originalGallery.PhotoUri = await _uploadService.UploadImageAsync(updatedGallery.UploadRequests);
            } 

            var GalleryDto = new GetGalleryDto
            {
                Id = originalGallery.Id, 
                PhotoUri = originalGallery.PhotoUri, 
            };
            _GalleryRepository.Update(originalGallery);
            await _GalleryRepository.SaveChangesAsync();

            return GalleryDto;
        }

        public async Task<PaginatedList<GetGalleryDto>> GetAllGalleryWithPageSize(GetGalleryFilter filter)
        {
            var id = _currentUserService.UserId;

            filter ??= new GetGalleryFilter();
            IQueryable<Gallery> Gallery = null;


            Gallery = _GalleryRepository
                .GetAll()
               .Where(o => o.IsDisabled == false);

            return await _mapper.ProjectTo<GetGalleryDto>(Gallery).ToPaginatedListAsync(filter.CurrentPage, filter.PageSize);
        }

        public async Task<AllList<GetGalleryDto>> GetAllGallery(GetGalleryFilter filter)
        {
            var id = _currentUserService.UserId;

            filter ??= new GetGalleryFilter();
            IQueryable<Gallery> Gallery = null;

            Gallery = _GalleryRepository
               .GetAll()
               .Where(o => o.IsDisabled == false);

            return await _mapper.ProjectTo<GetGalleryDto>(Gallery).ToAllListAsync(filter.CurrentPage);
        } 
    }
}




