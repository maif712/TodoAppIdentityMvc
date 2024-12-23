using Microsoft.AspNetCore.Identity;

namespace TodoAppIdentityMvc.Models.Entities
{
    public class AppUser : IdentityUser
    {
        public List<Todo>? Todos { get; set; }
        public List<Archive>? Archives { get; set; }
    }
}
