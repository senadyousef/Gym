using Boilerplate.Application.DTOs;
using Boilerplate.Application.Filters;
using Boilerplate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Application.Interfaces
{
    public interface IChildrenService
    {
        public Task<PaginatedList<GetChildrenDto>> GetAllChildrenWithPageSize(GetChildrenFilter filter);
        public Task<AllList<GetChildrenDto>> GetAllChildren(GetChildrenFilter filter);

        public Task<GetChildrenDto> CreateChildren(CreateChildrenDto Children);
        public Task<GetChildrenDto> GetChildrenById(int id);

        public Task<GetChildrenDto> UpdateChildren(int id, UpdateChildrenDto updatedChildren);

        public Task<bool> DeleteChildren(int id);

    }

    public class GetChildrenDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string Role { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public DateTime BOD { get; set; }
        public string MembershipStatus { get; set; }
        public DateTime MembershipExpDate { get; set; }
        public string MobilePhone { get; set; }
        public PushToken PushToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string PhotoUri { get; set; }
    }
    public class CreateChildrenDto
    {
        public int UserId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string Role { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public DateTime BOD { get; set; }
        public string MembershipStatus { get; set; }
        public DateTime MembershipExpDate { get; set; }
        public string MobilePhone { get; set; }
        public PushToken PushToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string PhotoUri { get; set; }
        public UploadPhotoRequest UploadRequests { get; set; }
    }

    public class UpdateChildrenDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string Role { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public DateTime BOD { get; set; }
        public string MembershipStatus { get; set; }
        public DateTime MembershipExpDate { get; set; }
        public string MobilePhone { get; set; }
        public PushToken PushToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string PhotoUri { get; set; }
        public UploadPhotoRequest UploadRequests { get; set; }
    }

    public class GetChildrenFilter : PaginationInfoFilter
    {
        public int UserId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string Role { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public DateTime BOD { get; set; }
        public string MembershipStatus { get; set; }
        public DateTime MembershipExpDate { get; set; }
        public string MobilePhone { get; set; }
        public PushToken PushToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string PhotoUri { get; set; }
    }
}
