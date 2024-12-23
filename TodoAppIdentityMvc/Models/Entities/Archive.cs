namespace TodoAppIdentityMvc.Models.Entities
{
    public class Archive
    {
        public string Id { get; set; }
        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public string TodoId { get; set; }
        public virtual Todo Todo { get; set; }
        public DateTime ArchivedAt { get; set; }
    }
}
