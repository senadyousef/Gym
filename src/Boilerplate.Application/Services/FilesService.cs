using AutoMapper;
using Boilerplate.Application.DTOs;
using Boilerplate.Application.Extensions;
using Boilerplate.Application.Interfaces;
using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
            var fileName = createFilesDto.Name;
            var filePath = string.Empty;
            string jsonString = string.Empty;
            try
            {
                result = "http://gym.useitsmart.com/webimages/" + fileName;

                var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/WebImages/");

                filePath = Path.Combine("wwwroot/WebImages/", fileName);

                byte[] data = Convert.FromBase64String(createFilesDto.file);
                jsonString = Encoding.UTF8.GetString(data);

                if (createFilesDto.Id == 0)
                {
                    IQueryable<Domain.Entities.Files> Files = null;
                    Files = _FilesRepository
                       .GetAll()
                       .Where(o => o.Name == createFilesDto.Name)
                       .Where(o => o.IsDisabled == false);
                    if (Files.Count() > 0)
                    {
                        result = "Name already exist !!";
                    }
                    else
                    {
                        var newFiles = new Domain.Entities.Files
                        {
                            Id = 0,
                            Name = fileName,
                            Url = result,
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
                        FilesById.Url = result;
                        FilesById.Name = createFilesDto.Name;
                        FilesById.LastModifiedOn = DateTime.Now;

                        _FilesRepository.Update(FilesById);
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
            await _FilesRepository.SaveChangesAsync();
            return await saveFile(filePath, jsonString, result);
        }

        public async Task<string> saveFile(string filePath, string jsonString,string result)
        {
            try
            {
                File.WriteAllText(filePath, jsonString);

                var streamFile = new FileStream(jsonString, FileMode.Open);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await streamFile.CopyToAsync(stream);
                }
            } 
            catch (Exception ex) 
            {
                return ex.InnerException.Message;
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