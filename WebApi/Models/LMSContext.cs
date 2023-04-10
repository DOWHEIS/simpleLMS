using Microsoft.EntityFrameworkCore;

namespace WebApi.Models;

public class LMSContext : DbContext
{
    public LMSContext(DbContextOptions<LMSContext> options) : base(options)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "LMS.db");
    }

    public DbSet<Assignment> Assignments { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<Module> Modules { get; set; } = null!;
    public string DbPath { get; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
      
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>()
            .HasMany(e => e.Modules)
            .WithOne(e => e.Course);
        
        modelBuilder.Entity<Module>()
            .HasMany(e => e.Assignments)
            .WithOne(e => e.Module);
    }
}