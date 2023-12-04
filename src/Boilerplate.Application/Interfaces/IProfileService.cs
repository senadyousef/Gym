using Boilerplate.Domain.Entities;
using System;
using System.Threading.Tasks;
using Boilerplate.Application.DTOs;
using Boilerplate.Application.DTOs.User;
using Boilerplate.Application.Filters;


namespace Boilerplate.Application.Interfaces
{
    public interface IProfileService : IDisposable
    {
        Task<GetUserDto> GetMyProfile(); 
        Task<GetUserDto> UpdateMyPassword(UpdatePasswordDto dto);
        Task<GetUserDto> UpdateMyDisplayName(UpdateDisplayNameDto dto);
        Task<GetUserDto> UpdateMyMobile(UpdateMobileDto dto);
        Task<GetUserDto> UpdateMyProfilePicture(UploadRequest dto);

    }
}
