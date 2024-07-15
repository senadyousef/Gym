﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Amazon.S3.Model;
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
using static System.Net.Mime.MediaTypeNames;
using BC = BCrypt.Net.BCrypt;
using Net.Codecrete.QrCodeGenerator;
using System.IO;
using System.Text;
using System.Drawing;

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
            var user = new User();
            user = await _userRepository
                .GetAll()
                .Where(o => o.IsDisabled == false)
                .FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            if (user == null || !BC.Verify(password, user.Password))
            {
                return null;
            }
            if (user != null)
            {
                if (user.Role != "SuperAdmin")
                {
                    if (user.MembershipExpDate.Date < DateTime.Now.Date)
                    {
                        var userUpdate = await _userRepository.GetById(user.Id);
                        userUpdate.MembershipStatus = "Not Active";
                        _userRepository.Update(userUpdate);
                        await _userRepository.SaveChangesAsync();
                    }
                    else
                    {
                        if (user.MembershipStatus == "Not Active")
                        {
                            var userUpdate = await _userRepository.GetById(user.Id);
                            userUpdate.MembershipStatus = "Active";
                            _userRepository.Update(userUpdate);
                            await _userRepository.SaveChangesAsync();
                        }
                    }
                }
            }
            return user;
        }

        public async Task<CheckEmailAndMobieNumber> CheckEmailAndMobieNumber(string Text)
        {
            CheckEmailAndMobieNumber checkEmailAndMobieNumber = new CheckEmailAndMobieNumber();
            var user = new User();
            bool IsMobileNumer = true;
            checkEmailAndMobieNumber.Message = "Enter Email Or Mobile Phone";
            checkEmailAndMobieNumber.IsValid = false;
            if (Text != null)
            {
                foreach (char c in Text)
                {
                    if (c < '0' || c > '9')
                    {
                        IsMobileNumer = false;
                    }
                }
                if (IsMobileNumer)
                {
                    user = await _userRepository
                      .GetAll()
                      .Where(o => o.IsDisabled == false)
                      .FirstOrDefaultAsync(x => x.MobilePhone == Text);
                    if (user != null)
                    {
                        checkEmailAndMobieNumber.Message = "Mobile Phone Is Used";
                        checkEmailAndMobieNumber.IsValid = false;
                    }
                    else
                    {
                        checkEmailAndMobieNumber.Message = "Accepted Mobile Phone";
                        checkEmailAndMobieNumber.IsValid = true;
                    }
                }
                else if (Regex.Match(Text, "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$").Success)
                {
                    user = await _userRepository
                      .GetAll()
                      .Where(o => o.IsDisabled == false)
                      .FirstOrDefaultAsync(x => x.Email.ToLower() == Text.ToLower());
                    if (user != null)
                    {
                        checkEmailAndMobieNumber.Message = "Email Is Used";
                        checkEmailAndMobieNumber.IsValid = false;
                    }
                    else
                    {
                        checkEmailAndMobieNumber.Message = "Accepted Email";
                        checkEmailAndMobieNumber.IsValid = true;
                    }
                }
                else
                {
                    checkEmailAndMobieNumber.Message = "Enter Valid Email Or Mobile Phone";
                    checkEmailAndMobieNumber.IsValid = false;
                }
            }
            return checkEmailAndMobieNumber;
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

            if (dto.UploadRequests != null)
            {
                created.PhotoUri = await _uploadService.UploadImageAsync(dto.UploadRequests);
            }

            var id = _currentUserService.UserId;
            var user = await _userRepository.GetById(int.Parse(id));
            if (user != null)
            {
                if (user.Role == "SuperAdmin")
                {
                    if (dto.Role == "Gym" || dto.Role == "Store")
                    {
                        created.Password = BC.HashPassword(dto.Password);
                        _userRepository.Create(created);
                        await _userRepository.SaveChangesAsync();
                    }
                }
                if (user.Role == "Gym")
                {
                    if (dto.Role == "Member" || dto.Role == "Coach")
                    {
                        created.Password = BC.HashPassword(dto.Password);
                        _userRepository.Create(created);
                        await _userRepository.SaveChangesAsync();
                    }
                }
            }
            return _mapper.Map<GetUserDto>(created);
        }

        public async Task<bool> DeleteUser(int id)
        {
            var originalUser = await _userRepository.GetById(id);
            if (originalUser == null) return false;

            originalUser.IsDisabled = true;
            originalUser.DisabledOn = DateTime.Now;
            originalUser.DisabledBy = _currentUserService.UserId;
            _userRepository.Update(originalUser);
            await _userRepository.SaveChangesAsync();

            return true;
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
            user.GymId = dto.GymId;
            user.DOB = dto.DOB;
            user.MembershipStatus = dto.MembershipStatus;
            user.MembershipExpDate = dto.MembershipExpDate;
            user.MembershipStartDate = user.MembershipStartDate;
            user.PhotoUri = user.PhotoUri;
            if (dto.UploadRequests != null)
            {
                user.PhotoUri = await _uploadService.UploadImageAsync(dto.UploadRequests);
            }

            if (!string.IsNullOrEmpty(dto.Password))
                user.Password = BC.HashPassword(dto.Password);
            _userRepository.Update(user);

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
                    .Where(o => o.Role == filter.Role || filter.Role == null)
                    .Where(o => o.Email == filter.Email || filter.Email == null)
                    .Where(o => o.MembershipExpDate == filter.MembershipExpDate || filter.MembershipExpDate == null)
                    .Where(o => o.MembershipStartDate == filter.MembershipStartDate || filter.MembershipStartDate == null)
                    .Where(o => o.MembershipStatus == filter.MembershipStatus || filter.MembershipStatus == null)
                    .Where(o => o.NameEn == filter.NameEn || filter.NameEn == null)
                    .Where(o => o.NameAr == filter.NameAr || filter.NameAr == null)
                    .Where(o => o.GymId == filter.GymId || filter.GymId == 0)
                    .Where(o => o.IsDisabled == false);
            }
            else if (_currentUserService.Role == "Gym")
            {
                if (filter.Role == "Gym" || filter.Role == "Store" || filter.Role == "Coach")
                {
                    users = _userRepository
                    .GetAll()
                    .Where(o => o.Role == filter.Role || filter.Role == null)
                    .Where(o => o.Email == filter.Email || filter.Email == null)
                    .Where(o => o.GymId == filter.GymId || filter.GymId == 0)
                    .Where(o => o.MembershipExpDate == filter.MembershipExpDate || filter.MembershipExpDate == null)
                    .Where(o => o.MembershipStartDate == filter.MembershipStartDate || filter.MembershipStartDate == null)
                    .Where(o => o.MembershipStatus == filter.MembershipStatus || filter.MembershipStatus == null)
                    .Where(o => o.NameEn == filter.NameEn || filter.NameEn == null)
                    .Where(o => o.NameAr == filter.NameAr || filter.NameAr == null)
                    .Where(o => o.GymId == int.Parse(_currentUserService.UserId))
                    .Where(o => o.IsDisabled == false);
                }
            }
            else if (_currentUserService.Role == "Store")
            {

            }
            else if (_currentUserService.Role == "Member")
            {
                if (filter.Role == "Gym" || filter.Role == "Store" || filter.Role == "Coach")
                {
                    users = _userRepository
                    .GetAll()
                    .Where(o => o.Role == filter.Role)
                    .Where(o => o.GymId == filter.GymId || filter.GymId == 0)
                    .Where(o => o.IsDisabled == false);
                }

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
                GymId = user.GymId,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
                PhotoUri = user.PhotoUri,
                Gender = user.Gender,
                DOB = user.DOB.Value,
                MembershipStatus = user.MembershipStatus,
                MembershipExpDate = user.MembershipExpDate,
                MembershipStartDate = user.MembershipStartDate,
            };
            return userDto;
        }

        public async Task<User> GetUser(int id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task<int> NumberOfMembersInTheGym()
        {
            int Num = 0;
            IQueryable<User> users = null;

            if (_currentUserService.Role == "Gym")
            {
                var id = _currentUserService.UserId;
                var user = await _userRepository.GetById(int.Parse(id));
                if (user != null)
                {
                    users = _userRepository
                   .GetAll()
                   .Where(o => o.Role == "Member")
                   .Where(o => o.IsInGym == true)
                   .Where(o => o.GymId == user.Id)
                   .Where(o => o.IsDisabled == false);
                    Num = users.Count();
                } 
            }
            else if (_currentUserService.Role == "Member")
            {
                var id = _currentUserService.UserId;
                var user = await _userRepository.GetById(int.Parse(id));
                if (user != null)
                {
                    users = _userRepository
                   .GetAll()
                   .Where(o => o.Role == "Member")
                   .Where(o => o.IsInGym == true)
                   .Where(o => o.GymId == user.GymId)
                   .Where(o => o.IsDisabled == false);
                    Num = users.Count();
                }
            }

            return Num;
        }
    }
}
