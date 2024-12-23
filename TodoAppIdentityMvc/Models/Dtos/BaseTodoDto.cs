using System.ComponentModel.DataAnnotations;

namespace TodoAppIdentityMvc.Models.Dtos
{
    public class BaseTodoDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsCompleted { get; set; }
        public bool InArchive { get; set; }

    }
}
