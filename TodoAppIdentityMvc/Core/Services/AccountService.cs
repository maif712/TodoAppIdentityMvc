using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TodoAppIdentityMvc.Core.Interfaces;
using TodoAppIdentityMvc.Models.Dtos;
using TodoAppIdentityMvc.Models.Entities;
using TodoAppIdentityMvc.Utiles;

namespace TodoAppIdentityMvc.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<AccountService> _logger;


        public AccountService(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ILogger<AccountService> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<GeneralServiceRespone> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                var isUserExists = await _userManager.FindByNameAsync(registerDto.UserName);
                if (isUserExists != null)
                {
                    return new GeneralServiceRespone()
                    {
                        IsSuccess = false,
                        Message = "Registration failed",
                        Error = "User name already taken."
                    };
                }

                AppUser newUser = new AppUser()
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                };

                var result = await _userManager.CreateAsync(newUser, registerDto.Password);
                if (result.Succeeded)
                {
                    return new GeneralServiceRespone()
                    {
                        IsSuccess = true,
                        Message = "Registration successful."
                    };
                }

                return new GeneralServiceRespone()
                {
                    IsSuccess = false,
                    Message = "",
                    Error = "Registration falied."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during registration");
                return new GeneralServiceRespone()
                {
                    IsSuccess = false,
                    Message = "",
                    Error = "An error occurred during registration."
                };
            }
            
        }
        public async Task<GeneralServiceRespone> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(loginDto.UserName, loginDto.Password, loginDto.RememberMe, true);
                if (result.Succeeded)
                {
                    return new GeneralServiceRespone()
                    {
                        IsSuccess = true,
                        Message = "Login was successful."
                    };
                }

                return new GeneralServiceRespone()
                {
                    IsSuccess = false,
                    Message = "Login failed.",
                    Error = "Username or password is wrong, try again!"
                };
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred during login");
                return new GeneralServiceRespone()
                {
                    IsSuccess = false,
                    Message = "",
                    Error = "An error occurred during login."
                }; ;
            }
        }
        public async Task<ServiceResponse<AppUser>> GetUserInfoAsync(ClaimsPrincipal user)
        {
            try
            {
                var uId = _userManager.GetUserId(user);
                if (string.IsNullOrEmpty(uId))
                {
                    return ServiceResponse<AppUser>.Failure("Id can not be null");
                }

                var userInfo = await _userManager.Users
                    .Include(u => u.Todos)
                    .FirstOrDefaultAsync(u => u.Id == uId);
                if (userInfo == null)
                {
                    return ServiceResponse<AppUser>.Failure("No user found.");
                }

                return ServiceResponse<AppUser>.Success(userInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting user info.");
                return ServiceResponse<AppUser>.Failure("Database error");
            }
        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        
    }
}
