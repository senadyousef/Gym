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
using Amazon.S3.Model;


namespace Boilerplate.Application.Services
{
    public class CartsService : ICartsService
    {
        private ICartsRepository _CartsRepository;
        private IMapper _mapper;
        private ICurrentUserService _currentUserService;
        private IBillRepository _billRepository;

        public CartsService(ICartsRepository CartsRepository, IMapper mapper,
            ICurrentUserService currentUserService, IBillRepository billRepository)
        {
            _CartsRepository = CartsRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _billRepository = billRepository;
        }
        public async Task<GetCartsDto> CreateCarts(CreateCartsDto Carts)
        {
            IQueryable<Bill> Bill = null;

            Bill = _billRepository
               .GetAll()
               .Where(o => o.IsDisabled == false)
               .Where(o => o.UserId == Carts.UserId)
               .Where(o => o.Status == "Open"); 

            var newCarts = new Carts
            {
                UserId = Carts.UserId,
                ItemsId = Carts.ItemsId,
                BillId = Carts.BillId,
                CreatedOn = DateTime.Now,
                IsDisabled = false
            };

            if (Bill.Count() > 0)
            {
                newCarts.BillId = Bill.First().Id;
            }
            else
            {
                var newBill = new Bill
                {
                    UserId = Carts.UserId,
                    Amount = 0,
                    Status = "Open",
                    CreatedOn = DateTime.Now,
                    IsDisabled = false
                };

                _billRepository.Create(newBill);
                await _billRepository.SaveChangesAsync();

                var BillDto = new GetBillDto
                {
                    Id = newBill.Id,
                    UserId = newBill.UserId,
                    Amount = newBill.Amount,
                    Status = newBill.Status,
                };
                newCarts.BillId = BillDto.Id;
            }

            _CartsRepository.Create(newCarts);
            await _CartsRepository.SaveChangesAsync();

            var CartsDto = new GetCartsDto
            {
                Id = newCarts.Id,
                UserId = Carts.UserId,
                ItemsId = Carts.ItemsId,
                BillId = newCarts.BillId,
            };
            return CartsDto;
        }
        public async Task<bool> DeleteCarts(int id)
        {
            var originalCarts = await _CartsRepository.GetById(id);
            if (originalCarts == null) return false;

            originalCarts.IsDisabled = true;
            originalCarts.DisabledOn = DateTime.Now;
            originalCarts.DisabledBy = _currentUserService.UserId;
            _CartsRepository.Update(originalCarts);
            await _CartsRepository.SaveChangesAsync();

            return true;
        }
        public async Task<GetCartsDto> GetCartsById(int id)
        {
            var ids = _currentUserService.UserId;
            var Carts = await _CartsRepository.GetById(id);

            if (Carts == null)
                return null;

            var CartsDto = new GetCartsDto
            {
                Id = Carts.Id,
                UserId = Carts.UserId,
                ItemsId = Carts.ItemsId,
                BillId = Carts.BillId,
            };
            return CartsDto;
        }

        public async Task<GetCartsDto> UpdateCarts(int id, UpdateCartsDto updatedCarts)
        {
            var originalCarts = await _CartsRepository.GetById(id);
            if (originalCarts == null) return null;

            originalCarts.UserId = updatedCarts.UserId;
            originalCarts.ItemsId = updatedCarts.ItemsId;
            originalCarts.BillId = updatedCarts.BillId;

            var CartsDto = new GetCartsDto
            {
                Id = originalCarts.Id,
                UserId = originalCarts.UserId,
                ItemsId = originalCarts.ItemsId,
                BillId = originalCarts.BillId,
            };
            _CartsRepository.Update(originalCarts);
            await _CartsRepository.SaveChangesAsync();

            return CartsDto;
        }

        public async Task<PaginatedList<GetCartsDto>> GetAllCartsWithPageSize(GetCartsFilter filter)
        {
            var id = _currentUserService.UserId;

            filter ??= new GetCartsFilter();
            IQueryable<Carts> Carts = null;

            Carts = _CartsRepository
               .GetAll()
               .Where(o => o.IsDisabled == false);

            return await _mapper.ProjectTo<GetCartsDto>(Carts).ToPaginatedListAsync(filter.CurrentPage, filter.PageSize);
        }
        public async Task<AllList<GetCartsDto>> GetAllCarts(GetCartsFilter filter)
        {
            var id = _currentUserService.UserId;

            filter ??= new GetCartsFilter();
            IQueryable<Carts> Carts = null;

            Carts = _CartsRepository
               .GetAll()
               .Where(o => o.IsDisabled == false)
               .Where(o => o.UserId == filter.UserId || filter.UserId == 0)
               .Where(o => o.ItemsId == filter.ItemsId || filter.ItemsId == 0);

            return await _mapper.ProjectTo<GetCartsDto>(Carts).ToAllListAsync(filter.CurrentPage);
        }

        //public void De()
        //{
        //    // Connection string for your SQL Server
        //    string connectionString = "Data Source=your_server;Initial Catalog=your_database;Integrated Security=True";

        //    // ID of the row you want to delete
        //    int rowIdToDelete = 123;

        //    // SQL query to delete a row based on the ID
        //    string deleteQuery = "DELETE FROM YourTableName WHERE ID = @RowId";

        //    // Create a SqlConnection
        //    using (sqlc connection = new SqlConnection(connectionString))
        //    {
        //        // Open the connection
        //        connection.Open();

        //        // Create a SqlCommand with the delete query and connection
        //        using (SqlCommand command = new SqlCommand(deleteQuery, connection))
        //        {
        //            // Add a parameter for the row ID
        //            command.Parameters.AddWithValue("@RowId", rowIdToDelete);

        //            // Execute the delete command
        //            int rowsAffected = command.ExecuteNonQuery();

        //            // Check the number of rows affected to see if the delete was successful
        //            if (rowsAffected > 0)
        //            {
        //                Console.WriteLine("Row deleted successfully.");
        //            }
        //            else
        //            {
        //                Console.WriteLine("Row with specified ID not found.");
        //            }
        //        }
        //    }
        //}
    }
}