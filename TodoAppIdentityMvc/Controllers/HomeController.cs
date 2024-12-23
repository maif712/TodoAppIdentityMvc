using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAppIdentityMvc.Core.Interfaces;
using TodoAppIdentityMvc.Models.Entities;
using TodoAppIdentityMvc.Utiles;
using static System.Reflection.Metadata.BlobBuilder;

namespace TodoAppIdentityMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITodoService _todoService;

        public HomeController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        public async Task<IActionResult> Index(string filterString)
        {
            var todos = await _todoService.GetUserTodoAsync(User);
            switch (filterString)
            {
                case StaticeFilterStrings.TODAY:
                    /* This approach will work only if todos has already been loaded into memory (e.g., if it's a List<T>).
                     * If it's a database query, DateTime.Date is not translatable to SQL, so you'll need to use a range comparison instead. */
                    todos = todos.Where(t => t.CreatedAt.Date == DateTime.Now.Date).ToList();
                    break;
                case StaticeFilterStrings.COMPLETED:
                    todos = todos.Where(t => t.IsCompleted == true).ToList();
                    break;
                case StaticePriorities.HIGH:
                    todos = todos.Where(t => t.Priority == StaticePriorities.HIGH).ToList();
                    break;
                case StaticePriorities.MEDIUM:
                    todos = todos.Where(t => t.Priority == StaticePriorities.MEDIUM).ToList();
                    break;
                case StaticePriorities.LOW:
                    todos = todos.Where(t => t.Priority == StaticePriorities.LOW).ToList();
                    break;
                default:
                    break;
            }
            return View(todos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
