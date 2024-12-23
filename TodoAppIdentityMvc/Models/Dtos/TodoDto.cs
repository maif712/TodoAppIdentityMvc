using System.ComponentModel.DataAnnotations;

namespace TodoAppIdentityMvc.Models.Dtos
{
    public class TodoDto
    {
        [Required]
        public string Title { get; set; }
        public string Priority { get; set; }
    }
}
