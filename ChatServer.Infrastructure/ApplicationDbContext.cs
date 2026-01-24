using System.Text.Json;
using ChatServer.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatServer.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; init; }  
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(user => user.Id);
            
            builder.Property(user => user.Username).IsRequired().HasMaxLength(50);
            builder.Property(user => user.Email).IsRequired().HasMaxLength(100);
            builder.Property(user => user.DisplayName).IsRequired().HasMaxLength(100);
            builder.Property(user => user.PasswordHash).IsRequired().HasMaxLength(500);
            
            builder.Property(user => user.Roles)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null)
                         ?? new List<string>())
                .HasColumnType("jsonb");
            
            builder.HasIndex(user => user.Email).IsUnique();
        });
    }
}
