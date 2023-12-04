using System;
using System.Collections.Generic;
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
    public class UserService : IUserService
    {
        private IUploadService _uploadService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private ICurrentUserService _currentUserService;
        public UserService(IUserRepository userRepository, IMapper mapper, ICurrentUserService currentUserService,
            IUploadService uploadService)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
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
                _userRepository.Dispose();
            }
        }

        public async Task<User> Authenticate(string email, string password)
        {
            var user = await _userRepository
                .GetAll()
                .Where(o => o.IsDisabled == false)
                .FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            if (user == null || !BC.Verify(password, user.Password))
            {
                return null;
            }

            return user;
        }


        public async Task<bool> CheckEmail(string email)
        {
            var user = await _userRepository
                .GetAll()
                .Where(o => o.IsDisabled == false)
                .FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            if (user == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task<bool> CheckPassword(int id, string Password)
        {
            var userPassword = await _userRepository
                .GetAll()
                .Where(o => o.IsDisabled == false)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (BC.Verify(Password, userPassword.Password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<GetUserDto> CreateUser(CreateUserDto dto)
        {
            var created = _userRepository.Create(_mapper.Map<User>(dto));
            created.PhotoUri = _uploadService.UploadAsync(dto.UploadRequests);
            created.Password = BC.HashPassword(dto.Password);

            _userRepository.Create(created);
            await _userRepository.SaveChangesAsync();
            return _mapper.Map<GetUserDto>(created);


        }

        public async Task<bool> DeleteUser(int id)
        {
            var originalUser = await _userRepository.GetById(id);
            if (originalUser == null) throw new Exception($"User With Id {id} not found");

            if (_currentUserService.Role == "SuperAdmin")
                await _userRepository.Delete(id);
            else if (_currentUserService.Role == "Owner")
            {
                if (originalUser.Role == "Owner")
                    await _userRepository.Delete(id);
            }

            return await _userRepository.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateUser(User user)
        {
            _userRepository.Update(user);
            return await _userRepository.SaveChangesAsync() > 0;
        }
        public async Task<bool> EditUser(CreateUserDto dto)
        {
            var user = await _userRepository.GetById(dto.Id);
            user.MobilePhone = dto.MobilePhone;
            user.NameEn = dto.NameEn;
            user.NameAr = dto.NameAr;
            user.Gender = dto.Gender;
            user.Role = dto.Role;
            user.PhotoUri = _uploadService.UploadAsync(dto.UploadRequests);

            if (!string.IsNullOrEmpty(dto.Password))
                user.Password = BC.HashPassword(dto.Password);
            _userRepository.Update(user);

            await _userRepository.SaveChangesAsync();

            await _userRepository.SaveChangesAsync();

            await _userRepository.SaveChangesAsync();
            return await _userRepository.SaveChangesAsync() > 0;
        }
        public async Task<GetUserDto> UpdatePassword(int id, UpdatePasswordDto dto)
        {
            var originalUser = await _userRepository.GetById(id);
            if (originalUser == null) return null;

            originalUser.Password = BC.HashPassword(dto.Password);
            _userRepository.Update(originalUser);
            await _userRepository.SaveChangesAsync();
            return _mapper.Map<GetUserDto>(originalUser);
        }
        public async Task<PaginatedList<GetUserExtendedDto>> GetAllUser(GetUsersFilter filter)
        {
            filter ??= new GetUsersFilter();
            IQueryable<User> users = null;

            //check the role first 
            if (_currentUserService.Role == "SuperAdmin")
            {
                users = _userRepository
                    .GetAll()
                    .WhereIf(!string.IsNullOrEmpty(filter.Email), x => EF.Functions.Like(x.Email, $"%{filter.Email}%"))
                    .WhereIf(!string.IsNullOrEmpty(filter.Role), x => EF.Functions.Like(x.Role, $"%{filter.Role}%"))
                    .Where(o => o.IsDisabled == false);
            }
            else if (_currentUserService.Role == "Owner")
            {
                users = _userRepository
                    .GetAll()
                    .WhereIf(!string.IsNullOrEmpty(filter.Email), x => EF.Functions.Like(x.Email, $"%{filter.Email}%"))
                    .WhereIf(!string.IsNullOrEmpty(filter.Role), x => EF.Functions.Like(x.Role, $"%{filter.Role}%"))
                    .WhereIf(string.IsNullOrEmpty(filter.Email) && string.IsNullOrEmpty(filter.Role),
                        u => u.Role == "Staff")
                    .Where(o => o.IsDisabled == false);
            }
            else
            {
                users = _userRepository
               .GetAll()
               .WhereIf(!string.IsNullOrEmpty(filter.Email), x => EF.Functions.Like(x.Email, $"%{filter.Email}%"))
               .WhereIf(!string.IsNullOrEmpty(filter.Role), x => EF.Functions.Like(x.Role, $"%{filter.Role}%"))
               .Where(o => o.IsDisabled == false);
            }

            return await _mapper.ProjectTo<GetUserExtendedDto>(users).ToPaginatedListAsync(filter.CurrentPage, filter.PageSize);
        }

        public async Task<GetUserDto> GetUserById(int id)
        {
            return _mapper.Map<GetUserDto>(await _userRepository.GetById(id));
        }
        public async Task<GetUserExtendedDto> GetExtendedUserById(int id)
        {
            var user = await _userRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                return null;
            var userDto = new GetUserExtendedDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                NameEn = user.NameEn,
                NameAr = user.NameAr,
                MobilePhone = user.MobilePhone, 
                RefreshToken = user.RefreshToken,
                RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
                PhotoUri = user.PhotoUri,
                Gender = user.Gender,

            };
            return userDto;
        }

        public async Task<User> GetUser(int id)
        {
            return await _userRepository.GetById(id);
        }
    }
}
