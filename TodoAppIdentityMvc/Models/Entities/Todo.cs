using System.ComponentModel.DataAnnotations;

namespace TodoAppIdentityMvc.Models.Entities
{
    public class Todo
    {
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string Priority { get; set; }
        public bool IsCompleted { get; set; }
        public bool InArchive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AppUserId { get; set; }

        // Navigation property
        public virtual AppUser AppUser { get; set; }
        public virtual Archive Archive { get; set; }
    }
}