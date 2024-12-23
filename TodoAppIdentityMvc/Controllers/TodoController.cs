using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoAppIdentityMvc.Core.Interfaces;
using TodoAppIdentityMvc.Models.Dtos;
using TodoAppIdentityMvc.Utiles;

namespace TodoAppIdentityMvc.Controllers
{
    public class TodoController : Controller
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        public IActionResult Create()
        {
            return PartialView("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(TodoDto todoDto)
        {
            if (!ModelState.IsValid)
            {
                return View(todoDto);
            }
            var response = await _todoService.CreateTodoAsync(User, todoDto);
            if (response.IsSuccess)
            {
                TempData[StaticAlertMessage.SUCCESS] = response.Message;
                return RedirectToAction("Index", "Home");
            }
            TempData[StaticAlertMessage.ERROR] = response.Error;
            return View(response);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var response = await _todoService.GetTodoByIdAsync(id);
            if (response.IsSuccess)
            {
                return PartialView("Edit", response.Data);
            }
            TempData[StaticAlertMessage.ERROR] = response.ErrorMessage;
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, BaseTodoDto editTodoDto)
        {
            if (!ModelState.IsValid)
            {
                return View(editTodoDto);
            }
            var response = await _todoService.EditTodoAsync(id, editTodoDto);
            if (response.IsSuccess)
            {
                TempData[StaticAlertMessage.SUCCESS] = response.Message;
                return RedirectToAction("Index", "Home");
            }

            TempData[StaticAlertMessage.ERROR] = response.Error;
            return View(editTodoDto);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var response = await _todoService.GetTodoByIdAsync(id);
            if (response.IsSuccess)
            {
                return View(response.Data);
            }
            TempData[StaticAlertMessage.ERROR] = response.ErrorMessage;
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteTodo(string id)
        {
            var response = await _todoService.DeleteTodoAsync(id);
            if(response.IsSuccess)
            {
                TempData[StaticAlertMessage.SUCCESS] = response.Message;
                return RedirectToAction("Index", "Home");
            }

            TempData[StaticAlertMessage.ERROR] = response.Error;
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Complete(string id)
        {
            var response = await _todoService.CompleteTodoAsync(id);
            if (response.IsSuccess)
            {
                return RedirectToAction("Index", "Home");
            }

            TempData[StaticAlertMessage.ERROR] = response.Error;
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> AddToArchvie(string id)
        {
            var response = await _todoService.AddToArchiveAsync(id, User);
            if (response.IsSuccess)
            {
                TempData[StaticAlertMessage.SUCCESS] = response.Message;
                return RedirectToAction("Index", "Home");
            }

            TempData[StaticAlertMessage.ERROR] = response.Error;
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Archive()
        {
            var todos = await _todoService.GetUserArchivedTodoAsync(User);
            return View(todos);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Search(string searchString)
        {
            var todos = await _todoService.SerachTodoAsync(searchString, User);
            return View(todos);
        }

    }
}
