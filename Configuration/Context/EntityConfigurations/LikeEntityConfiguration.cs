using Astravon.Modules.User.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Astravon.Configuration.Context.EntityConfigurations;

public class LikeEntityConfiguration : IEntityTypeConfiguration<LikeEntity>
{
    public void Configure(EntityTypeBuilder<LikeEntity> builder)
    {
        builder.ToTable("Like");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.PostId).IsRequired();
        builder.Property(c => c.UserId).IsRequired();
    }
}