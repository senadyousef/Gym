using AutoMapper;
using Boilerplate.Application.DTOs;
using Boilerplate.Application.Extensions;
using Boilerplate.Application.Interfaces;
using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Boilerplate.Application.Files
{
    public class FilesService : IFilesService
    {
        private IUploadService _uploadService;
        private IFilesRepository _FilesRepository;
        private IMapper _mapper;
        private ICurrentUserService _currentUserService;

        public FilesService(IFilesRepository FilesRepository, IMapper mapper, ICurrentUserService currentUserService,
               IUploadService uploadService)
        {
            _FilesRepository = FilesRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _uploadService = uploadService;
        } 

        public async Task<string> UploadJson(IFormFile file)
        {
            string result = string.Empty;
            try
            {
                var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/WebImages/");

                var fileName = file.FileName;

                var filePath = Path.Combine("wwwroot/WebImages/", fileName);

                result = "http://gym.useitsmart.com/webimages/" + fileName;
                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var newFiles = new Domain.Entities.Files
                {
                    Name = fileName,
                    Url = result,
                    CreatedOn = DateTime.Now,
                    IsDisabled = false,
                };

                _FilesRepository.Create(newFiles);
                await _FilesRepository.SaveChangesAsync(); 
            }
            catch (Exception ex)
            {
                result = "Error";
            }

            return result;
        }

        public async Task<AllList<GetFilesDto>> GetAllFiles()
        {
            var id = _currentUserService.UserId;  
            IQueryable<Domain.Entities.Files> Files = null; 
            Files = _FilesRepository
               .GetAll()
               .Where(o => o.IsDisabled == false); 
            return await _mapper.ProjectTo<GetFilesDto>(Files).ToAllListAsync(1);
        } 
    }
}