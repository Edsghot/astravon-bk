using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Astragon.Modules.Teacher.Domain.Entity;

namespace Astragon.Configuration.Context.EntityConfigurations;


public class AcademicFormationEntityConfiguration : IEntityTypeConfiguration<AcademicFormationEntity>
{
    public void Configure(EntityTypeBuilder<AcademicFormationEntity> builder)
    {
        builder.ToTable("AcademicFormations");

        builder.HasKey(af => af.Id);

        builder.Property(af => af.Degree)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(af => af.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(af => af.StudyCenter)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(af => af.CountryOfStudy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(af => af.Source)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne<TeacherEntity>()
            .WithMany()
            .HasForeignKey(af => af.IdTeacher)
            .OnDelete(DeleteBehavior.Cascade);
    }
}