﻿namespace Astravon.Modules.User.Domain.Entity;

public record CommentEntity
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime DateComment { get; set; }

}