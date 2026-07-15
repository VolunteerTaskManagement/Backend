using Base.Application.Contracts.DTOs;

namespace Base.Application.Contracts
{
    public interface IJwtManager
    {
        string CreateToken(UserDTO user);
        TokenResponseDto RefreshTokensAsync(UserDTO user, string refreshToken);
        public string GenerateRefreshToken();
        string GetToken();
        long? GetUserId(string token = null);
        string GetUserName(string token = null);
        string GetRole(string token = null);
        string GetName(string token = null);
    }
}