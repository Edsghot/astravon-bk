﻿using Astravon.Modules.User.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Astravon.Configuration.Context.EntityConfigurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("User");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.FirstName)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(u => u.LastName).HasMaxLength(300).IsRequired();
        builder.Property(u => u.Mail).HasMaxLength(100).IsRequired();
        builder.Property(u => u.Password).HasMaxLength(100).IsRequired();

    }
}