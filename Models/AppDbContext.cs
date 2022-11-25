
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectFinalEngineer.Models.AggregateCategory;
using ProjectFinalEngineer.Models.AggregateContact;
using ProjectFinalEngineer.Models.AggregatePost;
using ProjectFinalEngineer.Models.AggregatePostCategory;

namespace ProjectFinalEngineer.Models;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName.Substring(6));
            }
        }

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasIndex(c => c.Slug).IsUnique();
        });
        modelBuilder.Entity<PostCategory>().HasKey(p => new { p.PostID, p.CategoryID });
        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasIndex(p => p.Slug)
                  .IsUnique();
        });

    }

    public DbSet<Contact> Contacts { get; set; }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Post> Posts { set; get; }
    public DbSet<PostCategory> PostCategories { set; get; }

}
