﻿using AutoMapper;
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
    public class UserEventsService : IUserEventsService
    {
        private IUploadService _uploadService;
        private IUserEventsRepository _UserEventsRepository;
        private IMapper _mapper;
        private ICurrentUserService _currentUserService;

        public UserEventsService(IUserEventsRepository UserEventsRepository, IMapper mapper,
            ICurrentUserService currentUserService,
             IUploadService uploadService)
        {
            _UserEventsRepository = UserEventsRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _uploadService = uploadService;
        }
        public async Task<GetUserEventsDto> CreateUserEvents(CreateUserEventsDto UserEvents)
        {
            UserEvents _UserEvents = null;
            _UserEvents = _UserEventsRepository
              .GetAll()
              .Where(o => o.UserId == UserEvents.UserId)
              .Where(o => o.EventsId == UserEvents.EventsId)
              .Where(o => o.IsDisabled == false).FirstOrDefault();
            if (_UserEvents != null)
            {
                return null;
            }
            var newUserEvents = new UserEvents
            {
                UserId = UserEvents.UserId,
                EventsId = UserEvents.EventsId,
                CreatedOn = DateTime.Now,
                IsDisabled = false
            };

            _UserEventsRepository.Create(newUserEvents);
            await _UserEventsRepository.SaveChangesAsync();

            var UserEventsDto = new GetUserEventsDto
            {
                UserId = UserEvents.UserId,
                EventsId = UserEvents.EventsId,
            };

            return UserEventsDto;
        }
        public async Task<bool> DeleteUserEvents(int id)
        {
            var originalUserEvents = await _UserEventsRepository.GetById(id);
            if (originalUserEvents == null) return false;

            originalUserEvents.IsDisabled = true;
            originalUserEvents.DisabledOn = DateTime.Now;
            originalUserEvents.DisabledBy = _currentUserService.UserId;
            _UserEventsRepository.Update(originalUserEvents);
            await _UserEventsRepository.SaveChangesAsync();

            return true;
        }

        public async Task<GetUserEventsDto> GetUserEventsById(int id)
        {
            var ids = _currentUserService.UserId;
            var UserEvents = await _UserEventsRepository.GetAll()

                .FirstOrDefaultAsync(o => o.Id == id);
            if (UserEvents == null)
                return null;
            var UserEventsDto = new GetUserEventsDto
            {
                Id = UserEvents.Id,
                UserId = UserEvents.UserId,
                EventsId = UserEvents.EventsId,
            };
            return UserEventsDto;
        }

        public async Task<GetUserEventsDto> UpdateUserEvents(int id, UpdateUserEventsDto updatedUserEvents)
        {
            var originalUserEvents = await _UserEventsRepository.GetById(id);
            if (originalUserEvents == null) return null;

            originalUserEvents.UserId = updatedUserEvents.UserId;
            originalUserEvents.EventsId = updatedUserEvents.EventsId;

            var UserEventsDto = new GetUserEventsDto
            {
                Id = originalUserEvents.Id,
                UserId = originalUserEvents.UserId,
                EventsId = originalUserEvents.EventsId,
            };
            _UserEventsRepository.Update(originalUserEvents);
            await _UserEventsRepository.SaveChangesAsync();

            return UserEventsDto;
        }

        public async Task<PaginatedList<GetUserEventsDto>> GetAllUserEventsWithPageSize(GetUserEventsFilter filter)
        {
            var id = _currentUserService.UserId;

            filter ??= new GetUserEventsFilter();
            IQueryable<UserEvents> UserEvents = null;


            UserEvents = _UserEventsRepository
               .GetAll()
               .Where(o => o.IsDisabled == false)
               .Include(o => o.Events);

            return await _mapper.ProjectTo<GetUserEventsDto>(UserEvents).ToPaginatedListAsync(filter.CurrentPage, filter.PageSize);
        }

        public async Task<AllList<GetUserEventsDto>> GetAllUserEvents(GetUserEventsFilter filter)
        {
            var id = _currentUserService.UserId;

            filter ??= new GetUserEventsFilter();
            IQueryable<UserEvents> UserEvents = null;

            UserEvents = _UserEventsRepository
               .GetAll()
               .Where(o => o.IsDisabled == false)
               .Where(o => o.EventsId == filter.EventsId || filter.EventsId == 0)
               .Where(o => o.UserId == filter.UserId || filter.UserId == 0)
               .Include(o => o.Events);

            return await _mapper.ProjectTo<GetUserEventsDto>(UserEvents).ToAllListAsync(filter.CurrentPage);
        }

    }
}




