using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Boilerplate.Application.DTOs;
using Boilerplate.Application.DTOs.User;
using Boilerplate.Application.Extensions;
using Boilerplate.Application.Filters;
using Boilerplate.Application.Interfaces;
using Boilerplate.Domain.Auth;
using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;


namespace Boilerplate.Application.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserRepository _profileRepository;
        private readonly IMapper _mapper;
        private ICurrentUserService _currentUserService;
        private IUploadService _uploadService;

        public ProfileService(IUserRepository profileRepository, IMapper mapper , ICurrentUserService currentProfileService, IUploadService uploadService)
        {
            _profileRepository = profileRepository;
            _mapper = mapper;
            _currentUserService = currentProfileService;
            _uploadService = uploadService;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _profileRepository.Dispose();
            }
        }

        public async Task<GetUserDto> GetMyProfile()
        {
            var id = _currentUserService.UserId;

            var user = _profileRepository
                .GetAll()
                .Where(x => x.Id.ToString() == id).FirstOrDefault();

            return _mapper.Map<GetUserDto>(user);
        }

        public async Task<GetUserDto> UpdateMyPassword(UpdatePasswordDto dto)
        {
            var id = _currentUserService.UserId;

            var originalUser = _profileRepository
                .GetAll()
                .Where(x => x.Id.ToString() == id).FirstOrDefault();

            if (originalUser == null) return null;

            originalUser.Password = BC.HashPassword(dto.Password);
            _profileRepository.Update(originalUser);
            await _profileRepository.SaveChangesAsync();
            return _mapper.Map<GetUserDto>(originalUser);
        }

        public async Task<GetUserDto> UpdateMyDisplayName(UpdateDisplayNameDto dto)
        {
            var id = _currentUserService.UserId;

            var originalUser = _profileRepository
                .GetAll()
                .Where(x => x.Id.ToString() == id).FirstOrDefault();

            if (originalUser == null) return null;

            originalUser.NameEn = dto.DisplayName;
            _profileRepository.Update(originalUser);
            await _profileRepository.SaveChangesAsync();
            return _mapper.Map<GetUserDto>(originalUser);
        }

        public async Task<GetUserDto> UpdateMyMobile(UpdateMobileDto dto)
        {
            var id = _currentUserService.UserId;

            var originalUser = _profileRepository
                .GetAll()
                .Where(x => x.Id.ToString() == id).FirstOrDefault();

            if (originalUser == null) return null;

            originalUser.MobilePhone = dto.Mobile;
            _profileRepository.Update(originalUser);
            await _profileRepository.SaveChangesAsync();
            return _mapper.Map<GetUserDto>(originalUser);
        }

        public async Task<GetUserDto> UpdateMyProfilePicture(UploadRequest dto)
        {
            var id = _currentUserService.UserId;

            var originalUser = _profileRepository
                .GetAll()
                .Where(x => x.Id.ToString() == id).FirstOrDefault();

            if (originalUser == null) return null;

            var ImagePath = _uploadService.UploadAsync(dto);

            originalUser.PhotoUri = ImagePath;
            _profileRepository.Update(originalUser);
            await _profileRepository.SaveChangesAsync();
            return _mapper.Map<GetUserDto>(originalUser);
        }
    }
}
