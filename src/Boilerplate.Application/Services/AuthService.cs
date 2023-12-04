using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Boilerplate.Application.DTOs;
using Boilerplate.Application.DTOs.Auth;
using Boilerplate.Application.Interfaces;
using Boilerplate.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace Boilerplate.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly TokenConfiguration _appSettings;
        private readonly IUserService _userService;

        public AuthService(IOptions<TokenConfiguration> appSettings, IUserService userService)
        {
            _appSettings = appSettings.Value;
            _userService = userService;
        }

        /// <summary>
        /// Generates a token from the user information
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<JwtDto> GenerateTokenAsync(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var claims = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            });

             
            if (user.NameEn !=null)
                claims.AddClaim(new Claim(ClaimTypes.Name, user.NameEn));

            if (user.MobilePhone != null)
                claims.AddClaim(new Claim(ClaimTypes.MobilePhone, user.MobilePhone));

            if (user.PhotoUri != null)
                claims.AddClaim(new Claim(type: "ProfilePictureDataUrl", value: user.PhotoUri));

            var expDate = DateTime.UtcNow.AddHours(1);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = expDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _appSettings.Audience,
                Issuer = _appSettings.Issuer
            };


            var token = tokenHandler.CreateToken(tokenDescriptor);
            var RefreshToken = GenerateRefreshToken();

            user.RefreshToken = RefreshToken;
            user.RefreshTokenExpiryTime = expDate;
            await _userService.UpdateUser(user);

            return new JwtDto
            {
                RefreshToken= RefreshToken,
                Token = tokenHandler.WriteToken(token),
                ExpDate = expDate
            };
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret)),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };


            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public async Task<JwtDto> RefreshToken(JwtDto jwtDto)
        {
            string accessToken = jwtDto.Token;
            string refreshToken = jwtDto.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            var Id = principal.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.NameIdentifier).Value;

            var user = await _userService.GetUser(int.Parse(Id));

            if (user == null || user.RefreshToken != refreshToken )
            {
                return null;
            }

            var newAccessToken =await GenerateTokenAsync(user);
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = newAccessToken.ExpDate;

            await _userService.UpdateUser(user);
            newAccessToken.RefreshToken = newRefreshToken;
            return newAccessToken;

        }
    }
}
