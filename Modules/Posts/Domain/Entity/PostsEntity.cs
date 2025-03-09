namespace Astravon.Modules.User.Domain.Entity
{
    public record PostEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime PublicationDate { get; set; }
        public string PostUrl { get; set; }
        public DateTime UpdatePost { get; set; }
        public string Content { get; set; }
        public string UrlMedia { get; set; }
        
    }
}
