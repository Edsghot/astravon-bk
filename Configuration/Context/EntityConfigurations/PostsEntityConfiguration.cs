using Astravon.Modules.User.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Astravon.Configuration.Context.EntityConfigurations;

public class PostsEntityConfiguration : IEntityTypeConfiguration<PostEntity>
{
    public void Configure(EntityTypeBuilder<PostEntity> builder)
    {
        builder.ToTable("Post");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.UserId).IsRequired();
        builder.Property(p => p.PublicationDate).IsRequired();
        builder.Property(p => p.PostUrl).HasMaxLength(200).IsRequired();
        builder.Property(p => p.UpdatePost).IsRequired();
        builder.Property(p => p.Content).HasMaxLength(1000).IsRequired();
        builder.Property(p => p.UrlMedia).HasMaxLength(200);
    }
}