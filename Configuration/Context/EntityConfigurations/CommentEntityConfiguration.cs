using Astravon.Modules.User.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Astravon.Configuration.Context.EntityConfigurations;

public class CommentEntityConfiguration : IEntityTypeConfiguration<CommentEntity>
{

    public void Configure(EntityTypeBuilder<CommentEntity> builder)
    {
        builder.ToTable("Comment");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.PostId).IsRequired();
        builder.Property(c => c.UserId).IsRequired();
        builder.Property(c => c.Content).HasMaxLength(1000).IsRequired();
        builder.Property(c => c.DateComment).IsRequired();
    }
    
}