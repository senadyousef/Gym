﻿using AutoMapper;
using Boilerplate.Application.DTOs;
using Boilerplate.Application.Extensions;
using Boilerplate.Application.Interfaces;
using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
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

        public async Task<string> UploadJson(CreateFilesDto createFilesDto)
        {
            string result = string.Empty;
            var filePath = string.Empty;
            string jsonString = string.Empty;
            if (createFilesDto.Type == "Floor" || createFilesDto.Type == "File")
            {
                try
                {
                    result = "http://gym.useitsmart.com/webimages/" + createFilesDto.Name; 
                    var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/WebImages/");

                    filePath = Path.Combine("wwwroot/WebImages/", createFilesDto.Name);

                    byte[] data = Convert.FromBase64String(createFilesDto.file);
                    jsonString = Encoding.UTF8.GetString(data);
                    File.WriteAllText(filePath, jsonString); 
                    createFilesDto.file = result;
                }
                catch (Exception ex)
                {
                    return ex.InnerException.Message;
                }
            }
            if (createFilesDto.Type == "Image")
            {
                try
                {
                    var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/WebImages/");
                    result = "http://gym.useitsmart.com/webimages/" + createFilesDto.Name;
                    
                    byte[] imageBytes = Convert.FromBase64String(createFilesDto.file);
                    string imagePath = Path.Combine(wwwRootPath, createFilesDto.Name);
                    if (!Directory.Exists(wwwRootPath))
                    {
                        Directory.CreateDirectory(wwwRootPath);
                    }
                    System.IO.File.WriteAllBytes(imagePath, imageBytes);
                    createFilesDto.file = result;
                }
                catch (Exception ex)
                {
                    return ex.InnerException.Message;
                }
            }

            return await saveFile(createFilesDto);
        }

        public async Task<string> saveFile(CreateFilesDto createFilesDto)
        {
            string url = createFilesDto.file;
            if (createFilesDto.Id == 0)
            {
                IQueryable<Domain.Entities.Files> Files = null;
                Files = _FilesRepository
                   .GetAll()
                   .Where(o => o.Name == createFilesDto.Name)
                   .Where(o => o.IsDisabled == false);
                if (Files.Count() > 0)
                {
                    url = "Name already exist !!";
                }
                else
                {
                    var newFiles = new Domain.Entities.Files
                    {
                        Id = 0,
                        Name = createFilesDto.Name,
                        Url = createFilesDto.file,
                        Type = createFilesDto.Type,
                        CreatedOn = DateTime.Now,
                        IsDisabled = false,
                    };

                    _FilesRepository.Create(newFiles);
                }
            }
            else
            {
                var FilesById = await _FilesRepository.GetById(createFilesDto.Id);
                if (FilesById != null)
                {
                    FilesById.Url = createFilesDto.file;
                    FilesById.Name = createFilesDto.Name;
                    FilesById.LastModifiedOn = DateTime.Now;
                    FilesById.Type = createFilesDto.Type;

                    _FilesRepository.Update(FilesById);
                }
            }

            await _FilesRepository.SaveChangesAsync();
            return url;
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