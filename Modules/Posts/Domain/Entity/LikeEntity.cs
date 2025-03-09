namespace Astravon.Modules.User.Domain.Entity;

public record LikeEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
}