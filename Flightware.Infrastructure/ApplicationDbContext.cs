using Flightware.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Flightware.Infrastructure;

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
            builder.HasKey(e => e.Id);
            builder.Property(user => user.Username).IsRequired();
            builder.Property(user => user.Email).IsRequired();
            builder.HasIndex(user => user.Email).IsUnique();
        });
    }
}
