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
    public class PersonalTrainersClassesService : IPersonalTrainersClassesService
    {
        private IUploadService _uploadService;
        private IPersonalTrainersClassesRepository _PersonalTrainersClassesRepository;
        private IMapper _mapper;
        private ICurrentUserService _currentUserService;

        public PersonalTrainersClassesService(IPersonalTrainersClassesRepository PersonalTrainersClassesRepository, IMapper mapper,
            ICurrentUserService currentUserService,
             IUploadService uploadService)
        {
            _PersonalTrainersClassesRepository = PersonalTrainersClassesRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _uploadService = uploadService;
        }
        public async Task<GetPersonalTrainersClassesDto> CreatePersonalTrainersClasses(CreatePersonalTrainersClassesDto PersonalTrainersClasses)
        {

            var newPersonalTrainersClasses = new PersonalTrainersClasses
            {
                PersonalTrainer = PersonalTrainersClasses.PersonalTrainer,
                Trainee = PersonalTrainersClasses.Trainee,
                Time = PersonalTrainersClasses.Time,
                CreatedOn = DateTime.Now,
                IsDisabled = false
            };

            var PersonalTrainersClassesDto = new GetPersonalTrainersClassesDto
            {
                Id = newPersonalTrainersClasses.Id,
                PersonalTrainer = PersonalTrainersClasses.PersonalTrainer,
                Trainee = PersonalTrainersClasses.Trainee,
                Time = PersonalTrainersClasses.Time,
            };

            _PersonalTrainersClassesRepository.Create(newPersonalTrainersClasses);
            await _PersonalTrainersClassesRepository.SaveChangesAsync();
            return PersonalTrainersClassesDto;
        }
        public async Task<bool> DeletePersonalTrainersClasses(int id)
        {
            var originalPersonalTrainersClasses = await _PersonalTrainersClassesRepository.GetById(id);
            if (originalPersonalTrainersClasses == null) return false;

            originalPersonalTrainersClasses.IsDisabled = true;
            originalPersonalTrainersClasses.DisabledOn = DateTime.Now;
            originalPersonalTrainersClasses.DisabledBy = _currentUserService.UserId;
            _PersonalTrainersClassesRepository.Update(originalPersonalTrainersClasses);
            await _PersonalTrainersClassesRepository.SaveChangesAsync();

            return true;
        }


        public async Task<GetPersonalTrainersClassesDto> GetPersonalTrainersClassesById(int id)
        {
            var ids = _currentUserService.UserId;
            //var PersonalTrainersClasses = await _PersonalTrainersClassesRepository.GetById(id);
            var PersonalTrainersClasses = await _PersonalTrainersClassesRepository.GetAll()
                .FirstOrDefaultAsync(o => o.Id == id);
            if (PersonalTrainersClasses == null)
                return null;
            var PersonalTrainersClassesDto = new GetPersonalTrainersClassesDto
            {

                Id = PersonalTrainersClasses.Id,
                PersonalTrainer = PersonalTrainersClasses.PersonalTrainer,
                Trainee = PersonalTrainersClasses.Trainee,
                Time = PersonalTrainersClasses.Time,
            };
            return PersonalTrainersClassesDto;
        }

        public async Task<GetPersonalTrainersClassesDto> UpdatePersonalTrainersClasses(int id, UpdatePersonalTrainersClassesDto updatedPersonalTrainersClasses)
        {
            var originalPersonalTrainersClasses = await _PersonalTrainersClassesRepository.GetById(id);
            if (originalPersonalTrainersClasses == null) return null;

            originalPersonalTrainersClasses.PersonalTrainer = updatedPersonalTrainersClasses.PersonalTrainer;
            originalPersonalTrainersClasses.Trainee = updatedPersonalTrainersClasses.Trainee;
            originalPersonalTrainersClasses.Time = updatedPersonalTrainersClasses.Time;

            var PersonalTrainersClassesDto = new GetPersonalTrainersClassesDto
            {
                Id = originalPersonalTrainersClasses.Id,
                PersonalTrainer = originalPersonalTrainersClasses.PersonalTrainer,
                Trainee = originalPersonalTrainersClasses.Trainee,
                Time = originalPersonalTrainersClasses.Time,
            };
            _PersonalTrainersClassesRepository.Update(originalPersonalTrainersClasses);
            await _PersonalTrainersClassesRepository.SaveChangesAsync();

            return PersonalTrainersClassesDto;
        }

        public async Task<PaginatedList<GetPersonalTrainersClassesDto>> GetAllPersonalTrainersClassesWithPageSize(GetPersonalTrainersClassesFilter filter)
        {
            var id = _currentUserService.UserId;

            filter ??= new GetPersonalTrainersClassesFilter();
            IQueryable<PersonalTrainersClasses> PersonalTrainersClasses = null;


            PersonalTrainersClasses = _PersonalTrainersClassesRepository
               .GetAll()
               .Where(o => o.IsDisabled == false);

            return await _mapper.ProjectTo<GetPersonalTrainersClassesDto>(PersonalTrainersClasses).ToPaginatedListAsync(filter.CurrentPage, filter.PageSize);
        }

        public async Task<AllList<GetPersonalTrainersClassesDto>> GetAllPersonalTrainersClasses(GetPersonalTrainersClassesFilter filter)
        {
            var id = _currentUserService.UserId;

            filter ??= new GetPersonalTrainersClassesFilter();
            IQueryable<PersonalTrainersClasses> PersonalTrainersClasses = null;

            PersonalTrainersClasses = _PersonalTrainersClassesRepository
               .GetAll()
               .Where(o => o.IsDisabled == false);

            return await _mapper.ProjectTo<GetPersonalTrainersClassesDto>(PersonalTrainersClasses).ToAllListAsync(filter.CurrentPage);
        }

    }
}




