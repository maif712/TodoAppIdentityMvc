using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TodoAppIdentityMvc.Core.Data;
using TodoAppIdentityMvc.Core.Interfaces;
using TodoAppIdentityMvc.Models.Dtos;
using TodoAppIdentityMvc.Models.Entities;
using TodoAppIdentityMvc.Utiles;

namespace TodoAppIdentityMvc.Core.Services
{
    public class TodoService : ITodoService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<TodoService> _logger;
        public TodoService(ApplicationDbContext context, UserManager<AppUser> userManager, ILogger<TodoService> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<List<Todo>> GetUserTodoAsync(ClaimsPrincipal user)
        {
            try
            {
                var uId = _userManager.GetUserId(user);
                if (string.IsNullOrEmpty(uId))
                {
                    return null;
                }
                var todos = await _context.Todos
                    .Where(t => t.AppUserId == uId)
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync();

                //switch (filterString)
                //{
                //    case "today":
                //        todos = await _context.Todos.Where(t => EF.Functions.DateDiffDay(t.CreatedAt, DateTime.Today) == 0).ToListAsync();
                //        break;
                //    default:
                //        break;
                //}
                return todos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong, try again later.");
                return new List<Todo>();
            }
        }

        public async Task<ServiceResponse<BaseTodoDto>> GetTodoByIdAsync(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return ServiceResponse<BaseTodoDto>.Failure("Id can not be null");
                }

                var todo = await _context.Todos.FindAsync(id);
                if (todo == null)
                {
                    return ServiceResponse<BaseTodoDto>.Failure("No todo found.");
                }

                BaseTodoDto dto = new BaseTodoDto()
                {
                    Title = todo.Title,
                    Priority = todo.Priority,
                    IsCompleted = todo.IsCompleted,
                    InArchive = todo.InArchive,
                    CreatedAt = todo.CreatedAt,
                };

                return ServiceResponse<BaseTodoDto>.Success(dto);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while removing todo.");
                return ServiceResponse<BaseTodoDto>.Failure("An error occurred while removing todo.");
            }
        }
        public async Task<GeneralServiceRespone> CreateTodoAsync(ClaimsPrincipal User, TodoDto todoDto)
        {
            try
            {
                var uId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(uId))
                {
                    return new GeneralServiceRespone()
                    {
                        IsSuccess = false,
                        Error = "No user found",
                    };
                }

                Todo newTodo = new Todo()
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = todoDto.Title,
                    Priority = todoDto.Priority,
                    AppUserId = uId,
                    CreatedAt = DateTime.Now,
                    InArchive = false,
                    IsCompleted = false,
                };

                await _context.AddAsync(newTodo);
                await _context.SaveChangesAsync();
                return new GeneralServiceRespone()
                {
                    IsSuccess = true,
                    Message = "Todo created successfuly."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating todo.");
                return new GeneralServiceRespone()
                {
                    IsSuccess = false,
                    Error = "An error occurred while creating todo."
                };
            }
        }
        public async Task<GeneralServiceRespone> EditTodoAsync(string id, BaseTodoDto editTodoDto)
        {

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return new GeneralServiceRespone()
                    {
                        IsSuccess = false,
                        Error = "Id can not be null."
                    };
                }

                var todo = await _context.Todos.FindAsync(id);
                if (todo == null)
                {
                    return new GeneralServiceRespone()
                    {
                        IsSuccess = false,
                        Error = "No todo found."
                    };
                }

                todo.Title = editTodoDto.Title;
                todo.Priority = editTodoDto.Priority;
                _context.Todos.Update(todo);
                await _context.SaveChangesAsync();
                return new GeneralServiceRespone()
                {
                    IsSuccess = true,
                    Message = "Todo updated successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating todo.");
                return new GeneralServiceRespone()
                {
                    IsSuccess = false,
                    Error = "An error occurred while updating todo."
                };
            }
        }
        public async Task<GeneralServiceRespone> DeleteTodoAsync(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return new GeneralServiceRespone()
                    {
                        IsSuccess = false,
                        Error = "ID can not be null"
                    };
                }

                // Find the related Archive entries for the Todo
                var archivedEntries = _context.Archives
                    .Where(a => a.TodoId == id)
                    .ToList();

                // Remove the Archive entries
                if (archivedEntries.Any())
                {
                    _context.Archives.RemoveRange(archivedEntries);
                }

                var todo = await _context.Todos.FindAsync(id);
                if (todo == null)
                {
                    return new GeneralServiceRespone()
                    {
                        IsSuccess = false,
                        Error = "No todo found."
                    };
                }

                _context.Todos.Remove(todo);
                await _context.SaveChangesAsync();
                return new GeneralServiceRespone()
                {
                    IsSuccess = true,
                    Message = "Todo removed successfully."
                };
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while removing todo.");
                return new GeneralServiceRespone()
                {
                    IsSuccess = false,
                    Error = "An error occurred while removing todo."
                };
            }
        }

        public async Task<GeneralServiceRespone> CompleteTodoAsync(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return new GeneralServiceRespone()
                    {
                        IsSuccess = false,
                        Error = "Id can not be null."
                    };
                }

                var todo = await _context.Todos.FindAsync(id);
                if (todo == null)
                {
                    return new GeneralServiceRespone()
                    {
                        IsSuccess = false,
                        Error = "No todo found."
                    };
                }

                todo.IsCompleted = !todo.IsCompleted;
                _context.Todos.Update(todo);
                await _context.SaveChangesAsync();
                return new GeneralServiceRespone()
                {
                    IsSuccess = true,
                    Message = "Todo completed successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while completing todo.");
                return new GeneralServiceRespone()
                {
                    IsSuccess = false,
                    Error = "An error occurred while completing todo."
                };
            }
        }

        public async Task<GeneralServiceRespone> AddToArchiveAsync(string tId, ClaimsPrincipal user)
        {
            try
            {
                var uId = _userManager.GetUserId(user);
                if (string.IsNullOrEmpty(uId))
                {
                    return new GeneralServiceRespone()
                    {
                        IsSuccess = false,
                        Error = "No User found"
                    };
                }

                if (string.IsNullOrEmpty(tId))
                {
                    return new GeneralServiceRespone()
                    {
                        IsSuccess = false,
                        Error = "Id can not be null."
                    };
                }

                var todo = await _context.Todos.FindAsync(tId);
                if (todo == null)
                {
                    return new GeneralServiceRespone()
                    {
                        IsSuccess = false,
                        Error = "No todo found."
                    };
                }

                todo.InArchive = true;
                Archive newArchive = new Archive()
                {
                    Id = Guid.NewGuid().ToString(),
                    AppUserId = uId,
                    TodoId = tId,
                };

                await _context.AddAsync(newArchive);
                await _context.SaveChangesAsync();
                return new GeneralServiceRespone()
                {
                    IsSuccess = true,
                    Message = "Todo successfully added to archive."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while archiving todo.");
                return new GeneralServiceRespone()
                {
                    IsSuccess = false,
                    Error = "An error occurred while archiving todo."
                };
            }
        }

        public async Task<List<Todo>> GetUserArchivedTodoAsync(ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated == false) return new List<Todo>();
            var uId = _userManager.GetUserId(user);
            if (string.IsNullOrEmpty(uId))
            {
                return null;
            }

            var archivedTodos = await _context.Archives
                .Include(a => a.Todo)
                .Where(a => a.AppUserId == uId)
                .OrderByDescending(a => a.ArchivedAt)
                .Select(a => a.Todo)
                .ToListAsync();

            
            return archivedTodos;

        }

        public async Task<List<Todo>> SerachTodoAsync(string searchString, ClaimsPrincipal user)
        {
            if(!user.Identity.IsAuthenticated) return new List<Todo>();
            var uId = _userManager.GetUserId(user);
            if (string.IsNullOrEmpty(uId)) return new List<Todo>();
            if(string.IsNullOrEmpty(searchString))
            {
                return new List<Todo>();
            }

            var todos = await _context.Todos
                .Where(t =>t.AppUserId == uId && t.Title.Contains(searchString.ToLower()))
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return todos;
        }
    }
}
