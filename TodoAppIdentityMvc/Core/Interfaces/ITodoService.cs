using System.Security.Claims;
using TodoAppIdentityMvc.Models.Dtos;
using TodoAppIdentityMvc.Models.Entities;
using TodoAppIdentityMvc.Utiles;

namespace TodoAppIdentityMvc.Core.Interfaces
{
    public interface ITodoService
    {
        Task<List<Todo>> GetUserTodoAsync(ClaimsPrincipal user);
        Task<ServiceResponse<BaseTodoDto>> GetTodoByIdAsync(string id);
        Task<GeneralServiceRespone> CreateTodoAsync(ClaimsPrincipal User, TodoDto todoDto);
        Task<GeneralServiceRespone> EditTodoAsync(string id, BaseTodoDto editTodoDto);
        Task<GeneralServiceRespone> CompleteTodoAsync(string id);
        Task<GeneralServiceRespone> DeleteTodoAsync(string id);
        Task<GeneralServiceRespone> AddToArchiveAsync(string tId, ClaimsPrincipal user);
        Task<List<Todo>> GetUserArchivedTodoAsync(ClaimsPrincipal user);
        Task<List<Todo>> SerachTodoAsync(string searchString, ClaimsPrincipal user);
    }
}
