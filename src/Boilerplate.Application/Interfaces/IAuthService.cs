using Boilerplate.Application.DTOs.Auth;
using Boilerplate.Domain.Entities;
using System.Threading.Tasks;

namespace Boilerplate.Application.Interfaces
{
    public interface IAuthService
    {
        Task<JwtDto> GenerateTokenAsync(User user);
        Task<JwtDto> RefreshToken(JwtDto jwtDto);
    }
}
