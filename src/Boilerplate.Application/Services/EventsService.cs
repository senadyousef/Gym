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
    public class EventsService : IEventsService
    {
        private IUploadService _uploadService;
        private IEventsRepository _EventsRepository;
        private IMapper _mapper;
        private ICurrentUserService _currentUserService;

        public EventsService(IEventsRepository EventsRepository, IMapper mapper,
            ICurrentUserService currentUserService,
             IUploadService uploadService)
        {
            _EventsRepository = EventsRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _uploadService = uploadService;
        }
        public async Task<GetEventsDto> CreateEvents(CreateEventsDto Events)
        { 
            var newEvents = new Events
            {
                UserId = Events.UserId,
                NameAr = Events.NameAr,
                NameEn = Events.NameEn,
                DescriptionEn = Events.DescriptionEn,
                DescriptionAr = Events.DescriptionAr,
                PhotoUri = _uploadService.UploadAsync(Events.UploadRequests),
                Date = Events.Date,
                From = Events.From,
                To = Events.To,
                Capacity = Events.Capacity,
                Booked = Events.Booked,
                Type = Events.Type,
                CreatedOn = DateTime.Now,
                IsDisabled = false
            };
             
            var EventsDto = new GetEventsDto
            {
                UserId = Events.UserId,
                NameAr = Events.NameAr,
                NameEn = Events.NameEn,
                DescriptionEn = Events.DescriptionEn,
                DescriptionAr = Events.DescriptionAr,
                PhotoUri = _uploadService.UploadAsync(Events.UploadRequests),
                Date = Events.Date,
                From = Events.From,
                To = Events.To,
                Capacity = Events.Capacity,
                Booked = Events.Booked,
                Type = Events.Type,
            };

            _EventsRepository.Create(newEvents);
            await _EventsRepository.SaveChangesAsync();
            return EventsDto;
        }
        public async Task<bool> DeleteEvents(int id)
        {
            var originalEvents = await _EventsRepository.GetById(id);
            if (originalEvents == null) return false;

            originalEvents.IsDisabled = true;
            originalEvents.DisabledOn = DateTime.Now;
            originalEvents.DisabledBy = _currentUserService.UserId;
            _EventsRepository.Update(originalEvents);
            await _EventsRepository.SaveChangesAsync();

            return true;
        }
         
        public async Task<GetEventsDto> GetEventsById(int id)
        {
            var ids = _currentUserService.UserId; 
            var Events = await _EventsRepository.GetAll()
                 
                .FirstOrDefaultAsync(o => o.Id == id);
            if (Events == null)
                return null;
            var EventsDto = new GetEventsDto
            { 
                Id = Events.Id,
                UserId = Events.UserId,
                NameAr = Events.NameAr,
                NameEn = Events.NameEn,
                DescriptionEn = Events.DescriptionEn,
                DescriptionAr = Events.DescriptionAr,
                PhotoUri = Events.PhotoUri,
                Date = Events.Date,
                From = Events.From,
                To = Events.To,
                Capacity = Events.Capacity,
                Booked = Events.Booked,
                Type = Events.Type,
            };
            return EventsDto;
        }

        public async Task<GetEventsDto> UpdateEvents(int id, UpdateEventsDto updatedEvents)
        {
            var originalEvents = await _EventsRepository.GetById(id);
            if (originalEvents == null) return null;
               
            originalEvents.NameAr = updatedEvents.NameAr;
            originalEvents.NameEn = updatedEvents.NameEn;
            originalEvents.UserId = updatedEvents.UserId;
            originalEvents.DescriptionEn = updatedEvents.DescriptionEn;
            originalEvents.DescriptionAr = updatedEvents.DescriptionAr;
            originalEvents.Date = updatedEvents.Date;
            originalEvents.From = updatedEvents.From;
            originalEvents.To = updatedEvents.To;
            originalEvents.Capacity = updatedEvents.Capacity;
            originalEvents.Booked = updatedEvents.Booked;
            originalEvents.Type = updatedEvents.Type;
            originalEvents.PhotoUri = _uploadService.UploadAsync(updatedEvents.UploadRequests);
              
            var EventsDto = new GetEventsDto
            { 
                Id = originalEvents.Id,
                UserId = originalEvents.UserId,
                NameAr = originalEvents.NameAr,
                NameEn = originalEvents.NameEn,
                DescriptionEn = originalEvents.DescriptionEn,
                DescriptionAr = originalEvents.DescriptionAr,
                PhotoUri = originalEvents.PhotoUri,
                Date = originalEvents.Date,
                From = originalEvents.From,
                To = originalEvents.To,
                Capacity = originalEvents.Capacity,
                Booked = originalEvents.Booked,
                Type = originalEvents.Type,
            };
            _EventsRepository.Update(originalEvents);
            await _EventsRepository.SaveChangesAsync();

            return EventsDto;
        }

        public async Task<PaginatedList<GetEventsDto>> GetAllEventsWithPageSize(GetEventsFilter filter)
        {
            var id = _currentUserService.UserId;

            filter ??= new GetEventsFilter();
            IQueryable<Events> Events = null;


            Events = _EventsRepository
               .GetAll()
               .Where(o => o.IsDisabled == false);

            return await _mapper.ProjectTo<GetEventsDto>(Events).ToPaginatedListAsync(filter.CurrentPage, filter.PageSize);
        }

        public async Task<AllList<GetEventsDto>> GetAllEvents(GetEventsFilter filter)
        {
            var id = _currentUserService.UserId;

            filter ??= new GetEventsFilter();
            IQueryable<Events> Events = null;

            Events = _EventsRepository
               .GetAll()
               .Where(o => o.IsDisabled == false)
               .Where(o => o.NameEn.Contains(filter.NameEn) || filter.NameEn == null)
               .Where(o => o.NameAr.Contains(filter.NameAr) || filter.NameAr == null);

            return await _mapper.ProjectTo<GetEventsDto>(Events).ToAllListAsync(filter.CurrentPage);
        }

    }
}




