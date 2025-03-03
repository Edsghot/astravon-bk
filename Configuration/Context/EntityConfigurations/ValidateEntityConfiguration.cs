using Astravon.Modules.User.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Astravon.Configuration.Context.EntityConfigurations;

public class ValidateEntityConfiguration : IEntityTypeConfiguration<ValidateEntity>
{
    public void Configure(EntityTypeBuilder<ValidateEntity> builder)
    {
        builder.ToTable("Validate");
        builder.HasKey(u => u.IdValidate);
        builder.Property(u => u.Email).HasMaxLength(100).IsRequired();
        builder.Property(u => u.Code).HasMaxLength(100).IsRequired();
    }
}