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
    public class BranchesService : IBranchesService
    {
        private IUploadService _uploadService;
        private IBranchesRepository _BranchesRepository;
        private IMapper _mapper;
        private ICurrentUserService _currentUserService;

        public BranchesService(IBranchesRepository BranchesRepository, IMapper mapper,
            ICurrentUserService currentUserService,
             IUploadService uploadService)
        {
            _BranchesRepository = BranchesRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _uploadService = uploadService;
        }
        public async Task<GetBranchesDto> CreateBranches(CreateBranchesDto Branches)
        {

            var newBranches = new Branches
            {
                NameEn = Branches.NameEn,
                NameAr = Branches.NameAr, 
                CreatedOn = DateTime.Now,
                IsDisabled = false
            }; 
             
            _BranchesRepository.Create(newBranches);
            await _BranchesRepository.SaveChangesAsync();

            var BranchesDto = new GetBranchesDto
            {
                Id = newBranches.Id,
                NameEn = Branches.NameEn,
                NameAr = Branches.NameAr,
            };
            return BranchesDto;
        }
        public async Task<bool> DeleteBranches(int id)
        {
            var originalBranches = await _BranchesRepository.GetById(id);
            if (originalBranches == null) return false;

            originalBranches.IsDisabled = true;
            originalBranches.DisabledOn = DateTime.Now;
            originalBranches.DisabledBy = _currentUserService.UserId;
            _BranchesRepository.Update(originalBranches);
            await _BranchesRepository.SaveChangesAsync(); 
            return true;
        }


        public async Task<GetBranchesDto> GetBranchesById(int id)
        {
            var ids = _currentUserService.UserId; 
            var Branches = await _BranchesRepository.GetAll() 
                .FirstOrDefaultAsync(o => o.Id == id);
            if (Branches == null)
                return null;
            var BranchesDto = new GetBranchesDto
            { 
                Id = Branches.Id,
                NameEn = Branches.NameEn,
                NameAr = Branches.NameAr, 
            };
            return BranchesDto;
        }

        public async Task<GetBranchesDto> UpdateBranches(int id, UpdateBranchesDto updatedBranches)
        {
            var originalBranches = await _BranchesRepository.GetById(id);
            if (originalBranches == null) return null;

            originalBranches.NameAr = updatedBranches.NameAr;
            originalBranches.NameEn = updatedBranches.NameEn;  
            var BranchesDto = new GetBranchesDto
            { 
                Id = originalBranches.Id, 
                NameEn = originalBranches.NameEn,
                NameAr = originalBranches.NameAr, 
            };
            _BranchesRepository.Update(originalBranches);
            await _BranchesRepository.SaveChangesAsync(); 
            return BranchesDto;
        }

        public async Task<PaginatedList<GetBranchesDto>> GetAllBranchesWithPageSize(GetBranchesFilter filter)
        {
            var id = _currentUserService.UserId;

            filter ??= new GetBranchesFilter();
            IQueryable<Branches> Branches = null;
             
            Branches = _BranchesRepository
               .GetAll()
               .Where(o => o.IsDisabled == false);

            return await _mapper.ProjectTo<GetBranchesDto>(Branches).ToPaginatedListAsync(filter.CurrentPage, filter.PageSize);
        }

        public async Task<AllList<GetBranchesDto>> GetAllBranches(GetBranchesFilter filter)
        {
            var id = _currentUserService.UserId;

            filter ??= new GetBranchesFilter();
            IQueryable<Branches> Branches = null;
             
            Branches = _BranchesRepository
               .GetAll()
               .Where(o => o.IsDisabled == false) 
               .Where(o => o.NameEn.Contains(filter.NameEn) || filter.NameEn == null)
               .Where(o => o.NameAr.Contains(filter.NameAr) || filter.NameAr == null); 

            return await _mapper.ProjectTo<GetBranchesDto>(Branches).ToAllListAsync(filter.CurrentPage);
        } 
    }
}




