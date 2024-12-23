using System.Security.Claims;
using TodoAppIdentityMvc.Models.Dtos;
using TodoAppIdentityMvc.Models.Entities;
using TodoAppIdentityMvc.Utiles;

namespace TodoAppIdentityMvc.Core.Interfaces
{
    public interface IAccountService
    {
        Task<GeneralServiceRespone> LoginAsync(LoginDto loginDto);
        Task<GeneralServiceRespone> RegisterAsync(RegisterDto registerDto);
        Task<ServiceResponse<AppUser>> GetUserInfoAsync(ClaimsPrincipal user);
        Task LogoutAsync();
    }
}
