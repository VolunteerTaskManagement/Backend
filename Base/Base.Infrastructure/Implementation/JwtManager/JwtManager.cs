using Base.Application.Contracts;
using Base.Application.Contracts.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Base.Infrastructure.Implementation
{
    public class JwtManager(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : IJwtManager
    {
        public string CreateToken(UserDTO user)
        {
            var claims = new List<Claim>
            {
                new Claim("userName", user.UserName),
                new Claim("name", user.Name),
                new Claim("id", user.Id.ToString()),
                new Claim("role", user.Role),
             };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("JwtSettings:SecretKey")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("JwtSettings:Issuer"),
                audience: configuration.GetValue<string>("JwtSettings:Audience"),
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public TokenResponseDto RefreshTokensAsync(UserDTO user, string refreshToken)
        {
            var isValid = ValidateRefreshTokenAsync(user, refreshToken);
            if (!isValid)
                throw new Exception("رفرش توکن منقضی شده یا نامعتبر است!");

            return CreateTokenResponse(user);
        }

        private TokenResponseDto CreateTokenResponse(UserDTO user)
        {
            return new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = GenerateRefreshToken()
            };
        }


        private bool ValidateRefreshTokenAsync(UserDTO user, string refreshToken)
        {
            //var user = await context.Users.FindAsync(userId);
            if (user is null || user.RefreshToken != refreshToken
                || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return false;
            }

            return true;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public string GetToken()
        {
            //var token = httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString() ;
            //var arr = token.Split();
            //if (arr[0].ToLower() == "bearer")
            //    return arr[1];
            //return arr[0];
            try
            {
                var token = httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

                var arr = token?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (arr?.Length == 2 && arr[0].Equals("Bearer", StringComparison.OrdinalIgnoreCase))
                    return arr[1];

                return null;
            }
            catch
            {
                return null;
            }
        }

        public long? GetUserId(string token = null)
        {
            try
            {
                token = token ?? GetToken();

                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);

                var id = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == "id").Value;
                return long.Parse(id);
            }
            catch
            {
                return null;
            }
        }

        public string GetUserName(string token = null)
        {
            try
            {
                token = token ?? GetToken();

                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);

                var userName = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == "userName").Value;
                return userName;
            }
            catch
            {
                return null;
            }
        }

        public string GetName(string token = null)
        {
            try
            {
                token = token ?? GetToken();

                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);

                var userName = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == "name").Value;
                return userName;
            }
            catch
            {
                return null;
            }
        }

        public string GetRole(string token = null)
        {
            try
            {
                token = token ?? GetToken();

                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);

                var role = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == "role").Value;
                return role;
            }
            catch
            {
                return null;
            }
        }
        //private async Task<string> GenerateAndSaveRefreshTokenAsync(UserDTO user)
        //{
        //    var refreshToken = GenerateRefreshToken();
        //    user.RefreshToken = refreshToken;
        //    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        //    await context.SaveChangesAsync();
        //    return refreshToken;
        //}
    }
}