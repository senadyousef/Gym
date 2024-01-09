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
    public class NewsService : INewsService
    {
        private IUploadService _uploadService;
        private INewsRepository _NewsRepository;
        private IMapper _mapper;
        private ICurrentUserService _currentUserService;

        public NewsService(INewsRepository NewsRepository, IMapper mapper, ICurrentUserService currentUserService,
               IUploadService uploadService)
        {
            _NewsRepository = NewsRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _uploadService = uploadService;
        }
        public async Task<GetNewsDto> CreateNews(CreateNewsDto News)
        {
            var newNews = new News
            {
                DescriptionEn = News.DescriptionEn,
                DescriptionAr = News.DescriptionAr,
                Highlight = News.Highlight,
                CreatedOn = DateTime.Now,
                PhotoUri = News.PhotoUri,
                IsDisabled = false,
            };

            if (News.UploadRequests != null)
            {
                newNews.PhotoUri = await _uploadService.UploadImageAsync(News.UploadRequests);
            }
            var NewsDto = new GetNewsDto
            {
                Id = newNews.Id,
                DescriptionEn = newNews.DescriptionEn,
                DescriptionAr = newNews.DescriptionAr,
                Highlight = newNews.Highlight,
                PhotoUri = newNews.PhotoUri,
            };

            _NewsRepository.Create(newNews);
            await _NewsRepository.SaveChangesAsync();
            return NewsDto;
        }
        public async Task<bool> DeleteNews(int id)
        {
            var originalNews = await _NewsRepository.GetById(id);
            if (originalNews == null) return false;

            originalNews.IsDisabled = true;
            originalNews.DisabledOn = DateTime.Now;
            originalNews.DisabledBy = _currentUserService.UserId;
            _NewsRepository.Update(originalNews);
            await _NewsRepository.SaveChangesAsync();

            return true;
        }


        public async Task<GetNewsDto> GetNewsById(int id)
        {
            var ids = _currentUserService.UserId;
            var News = await _NewsRepository.GetById(id);
            if (News == null)
                return null;
            var NewsDto = new GetNewsDto
            {
                Id = News.Id,
                DescriptionEn = News.DescriptionEn,
                DescriptionAr = News.DescriptionAr,
                Highlight = News.Highlight,
                PhotoUri = News.PhotoUri,
            };
            return NewsDto;
        }

        public async Task<GetNewsDto> UpdateNews(int id, UpdateNewsDto updatedNews)
        {
            var originalNews = await _NewsRepository.GetById(id);
            if (originalNews == null) return null;

            originalNews.DescriptionAr = updatedNews.DescriptionAr;
            originalNews.DescriptionEn = updatedNews.DescriptionEn;
            originalNews.Highlight = updatedNews.Highlight;
            originalNews.PhotoUri = originalNews.PhotoUri;
            if (updatedNews.UploadRequests != null)
            {
                originalNews.PhotoUri = await _uploadService.UploadImageAsync(updatedNews.UploadRequests);
            }

            var NewsDto = new GetNewsDto
            {

                Id = originalNews.Id,
                DescriptionEn = originalNews.DescriptionEn,
                DescriptionAr = originalNews.DescriptionAr,
                Highlight = originalNews.Highlight,
                PhotoUri = originalNews.PhotoUri,
            };
            _NewsRepository.Update(originalNews);
            await _NewsRepository.SaveChangesAsync();

            return NewsDto;
        }

        public async Task<PaginatedList<GetNewsDto>> GetAllNewsWithPageSize(GetNewsFilter filter)
        {
            var id = _currentUserService.UserId; 
            filter ??= new GetNewsFilter();
            IQueryable<News> News = null; 
            News = _NewsRepository
               .GetAll()
               .Where(o => o.IsDisabled == false);  
            return await _mapper.ProjectTo<GetNewsDto>(News).ToPaginatedListAsync(filter.CurrentPage, filter.PageSize);
        } 

        public async Task<AllList<GetNewsDto>> GetAllNews(GetNewsFilter filter)
        {
            var id = _currentUserService.UserId; 
            filter ??= new GetNewsFilter();
            IQueryable<News> News = null; 
            News = _NewsRepository
               .GetAll()
               .Where(o => o.IsDisabled == false); 
            return await _mapper.ProjectTo<GetNewsDto>(News).ToAllListAsync(filter.CurrentPage);
        } 
    }
}