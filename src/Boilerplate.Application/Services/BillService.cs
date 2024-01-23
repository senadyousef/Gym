using AutoMapper;
using Boilerplate.Application.DTOs;
using Boilerplate.Application.Interfaces;
using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace Boilerplate.Application.Services
{
    public class BillService : IBillService
    {
        private IUploadService _uploadService;
        private IBillRepository _BillRepository;
        private IItemsRepository _itemsRepository;
        private IMapper _mapper;
        private ICurrentUserService _currentUserService;

        public BillService(IBillRepository BillRepository, IMapper mapper,
            ICurrentUserService currentUserService, IItemsRepository itemsRepository,
             IUploadService uploadService)
        {
            _BillRepository = BillRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _uploadService = uploadService;
            _itemsRepository = itemsRepository;
        }
        public async Task<GetBillDto> CreateBill(CreateBillDto Bill)
        {
            var newBill = new Bill
            {
                UserId = Bill.UserId,
                Amount = Bill.Amount,
                Status = Bill.Status,
                CreatedOn = DateTime.Now,
                IsDisabled = false
            };

            _BillRepository.Create(newBill);
            await _BillRepository.SaveChangesAsync();

            var BillDto = new GetBillDto
            {
                Id = newBill.Id,
                UserId = Bill.UserId,
                Amount = Bill.Amount,
                Status = Bill.Status,
            };
            return BillDto;
        }
        public async Task<bool> DeleteBill(int id)
        {
            var originalBill = await _BillRepository.GetById(id);
            if (originalBill == null) return false;

            originalBill.IsDisabled = true;
            originalBill.DisabledOn = DateTime.Now;
            originalBill.DisabledBy = _currentUserService.UserId;
            _BillRepository.Update(originalBill);
            await _BillRepository.SaveChangesAsync();

            return true;
        }
        public async Task<GetBillDto> GetBillById(int id)
        {
            var ids = _currentUserService.UserId;
            var Bill = await _BillRepository.GetById(id);

            if (Bill == null)
                return null;

            var BillDto = new GetBillDto
            {
                Id = Bill.Id,
                UserId = Bill.UserId,
                Amount = Bill.Amount,
                Status = Bill.Status,
            };
            return BillDto;
        }

        public async Task<GetBillDto> UpdateBill(int id, UpdateBillDto updatedBill)
        {
            var originalBill = await _BillRepository.GetById(id);
            if (originalBill == null) return null;

            originalBill.UserId = updatedBill.UserId;
            originalBill.Amount = updatedBill.Amount;
            originalBill.Status = updatedBill.Status;

            var BillDto = new GetBillDto
            {
                Id = originalBill.Id,
                UserId = originalBill.UserId,
                Amount = originalBill.Amount,
                Status = originalBill.Status,
            };
            _BillRepository.Update(originalBill);
            await _BillRepository.SaveChangesAsync();

            return BillDto;
        }

        public async Task<PaginatedList<GetBillDto>> GetAllBillWithPageSize(GetBillFilter filter)
        {
            var id = _currentUserService.UserId;

            filter ??= new GetBillFilter();
            IQueryable<Bill> Bill = null;

            Bill = _BillRepository
               .GetAll()
               .Where(o => o.IsDisabled == false);

            return await _mapper.ProjectTo<GetBillDto>(Bill).ToPaginatedListAsync(filter.CurrentPage, filter.PageSize);
        }
        public async Task<AllList<GetBillDto>> GetAllBill(GetBillFilter filter)
        {
            var id = _currentUserService.UserId;

            filter ??= new GetBillFilter();
            IQueryable<Bill> Bill = null;

            Bill = _BillRepository
               .GetAll()
               .Where(o => o.IsDisabled == false)
               .Where(o => o.UserId == filter.UserId || filter.UserId == 0)
               .Where(o => o.Status == filter.Status || filter.Status == null || filter.Status == "");

            return await _mapper.ProjectTo<GetBillDto>(Bill).ToAllListAsync(filter.CurrentPage);
        }
    }
}