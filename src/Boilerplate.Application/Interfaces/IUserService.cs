using Boilerplate.Domain.Entities;
using System;
using System.Threading.Tasks;
using Boilerplate.Application.DTOs;
using Boilerplate.Application.DTOs.User;
using Boilerplate.Application.Filters;
using System.Collections.Generic;


namespace Boilerplate.Application.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<User> Authenticate(string email, string password);
        Task<CheckEmailAndMobieNumber> CheckEmailAndMobieNumber(string email);
        Task<bool> CheckPassword(int id, string Password);
        Task<GetUserDto> CreateUser(CreateUserDto dto);
        Task<bool> DeleteUser(int id);
        Task<bool> UpdateUser(User user);
        Task<User> GetUser(int id);
        Task<GetUserDto> UpdatePassword(int id, UpdatePasswordDto dto);
        Task<PaginatedList<GetUserExtendedDto>> GetAllUser(GetUsersFilter filter);
        Task<GetUserDto> GetUserById(int id);
        Task<GetUserExtendedDto> GetExtendedUserById(int id);
        Task<bool> EditUser(CreateUserDto dto);
        Task<int> NumberOfMembersInTheGym();
    }

    public class CheckEmailAndMobieNumber
    {
        public string Message { get; set; }
        public bool IsValid { get; set; }
    }
}
