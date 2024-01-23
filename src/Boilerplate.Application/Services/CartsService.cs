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
    public class CartsService : ICartsService
    {
        private IUploadService _uploadService;
        private ICartsRepository _CartsRepository;
        private IItemsRepository _itemsRepository;
        private IMapper _mapper;
        private ICurrentUserService _currentUserService;

        public CartsService(ICartsRepository CartsRepository, IMapper mapper,
            ICurrentUserService currentUserService, IItemsRepository itemsRepository,
             IUploadService uploadService)
        {
            _CartsRepository = CartsRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _uploadService = uploadService;
            _itemsRepository = itemsRepository;
        }
        public async Task<GetCartsDto> CreateCarts(CreateCartsDto Carts)
        {
            var newCarts = new Carts
            {
                UserId = Carts.UserId,
                ItemsId = Carts.ItemsId,
                CreatedOn = DateTime.Now,
                IsDisabled = false
            };

            _CartsRepository.Create(newCarts);
            await _CartsRepository.SaveChangesAsync();

            var CartsDto = new GetCartsDto
            {
                Id = newCarts.Id,
                UserId = Carts.UserId,
                ItemsId = Carts.ItemsId,
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
            };
            return CartsDto;
        }

        public async Task<GetCartsDto> UpdateCarts(int id, UpdateCartsDto updatedCarts)
        {
            var originalCarts = await _CartsRepository.GetById(id);
            if (originalCarts == null) return null;

            originalCarts.UserId = updatedCarts.UserId;
            originalCarts.ItemsId = updatedCarts.ItemsId;

            var CartsDto = new GetCartsDto
            {
                Id = originalCarts.Id,
                UserId = originalCarts.UserId,
                ItemsId = originalCarts.ItemsId,
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
               .Where(o => o.ItemsId == filter.ItemsId || filter.ItemsId == 0) ;

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