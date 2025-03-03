using Astravon.Configuration.Context.EntityConfigurations;
using Astravon.Modules.User.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Astravon.Configuration.Context;

public class MySqlContext : DbContext
{
    public MySqlContext(DbContextOptions<MySqlContext> options) : base(options)
    {
    }

    public DbSet<UserEntity> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString =
            "Server=unambavirtual.com;Database=dbjhfjuv_astravon;User=dbjhfjuv_edsghot;Password=Repro321.;";

        optionsBuilder.UseMySql(
            connectionString,
            new MySqlServerVersion(new Version(8, 0, 21))
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ValidateEntityConfiguration());
    }
}