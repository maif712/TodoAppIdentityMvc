using Microsoft.AspNetCore.Mvc;
using TodoAppIdentityMvc.Core.Interfaces;
using TodoAppIdentityMvc.Models.Dtos;
using TodoAppIdentityMvc.Utiles;

namespace TodoAppIdentityMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return View(registerDto);
            }
            var response = await _accountService.RegisterAsync(registerDto);
            if (response.IsSuccess)
            {
                return RedirectToAction("Login");
            }

            TempData[StaticAlertMessage.ERROR] = response.Error;
            return View(registerDto);

        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }

            var response = await _accountService.LoginAsync(loginDto);
            if (response.IsSuccess)
            {
                return RedirectToAction("Index", "Home");
            }

            TempData[StaticAlertMessage.ERROR] = response.Error;
            return View(loginDto);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
