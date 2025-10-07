using Asclepius.Auth.Domain;
using Asclepius.Auth.Domain.UserObject;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Asclepius.Auth.Data;

public sealed class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        //Сделал, чтобы не нужно было создавать миграции для оценки проекта
        // Database.EnsureCreated();
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.Email)
                .HasConversion(e => e.Value, s => new Email(s))
                .HasColumnName("Email")
                .IsRequired();
            entity.Property(u => u.Password)
                .HasConversion(e => e.Hash, s => new Password(s, true))
                .HasColumnName("Password")
                .IsRequired();

            entity.HasIndex(u => u.Id).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => new { u.LastName, u.FirstName });

            entity.HasMany(u => u.Roles).WithMany(r => r.Users);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasData(Role.Create("Admin"), Role.Create("Operator"), Role.Create("Staff"), Role.Create("Doctor"),
                Role.Create("Premium"));

            entity.HasIndex(r => r.Name).IsUnique();
            entity.HasIndex(r => r.Id).IsUnique();
        });

        base.OnModelCreating(modelBuilder);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
    }
}