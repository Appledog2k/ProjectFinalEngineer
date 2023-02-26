using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProjectFinalEngineer.Models.AggregateCategory;
using ProjectFinalEngineer.Models.AggregateComment;
using ProjectFinalEngineer.Models.AggregateContact;
using ProjectFinalEngineer.Models.AggregateKnowledge;
using ProjectFinalEngineer.Models.AggregatePost;
using ProjectFinalEngineer.Models.AggregatePostCategory;
using ProjectFinalEngineer.Models.AggregateUser;
using ProjectFinalEngineer.Models.RoomingHouse;

namespace ProjectFinalEngineer.EntityFramework;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        base.OnConfiguring(builder);
        builder.ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.NavigationBaseIncludeIgnored));

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName.Substring(6));
            }
        }
        modelBuilder.Entity<PostCategory>().HasKey(p => new { p.PostID, p.CategoryID });
        modelBuilder.Entity<KnowledgeCategory>().HasKey(p => new { p.KnowledgeID, p.CategoryID });
        modelBuilder.Entity<RommingHouseArea>().HasKey(p => new { p.AreaID, p.RommingHouseID });


    }

    public DbSet<RommingHouseArea> RommingHouseAreas { get; set; }
    public DbSet<RoomingHouse> RoomingHouses { get; set; }
    public DbSet<Area> Areas { get; set; }

    public DbSet<KnowledgeCategory> KnowledgeCategories { get; set; }
    public DbSet<Knowledge> Knowledges { get; set; }



    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Post> Posts { set; get; }
    public DbSet<PostCategory> PostCategories { set; get; }
    public DbSet<AppUser> AppUsers { set; get; }

}
