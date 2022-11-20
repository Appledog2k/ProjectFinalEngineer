using Microsoft.EntityFrameworkCore;

namespace ProjectFinalEngineer.Models;

public class AppDbContext : DbContext
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

        //* Xử lý table name loại bỏ tiền tố ASP.NET
        // foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        // {
        //     var tableName = entityType.GetTableName();
        //     if (tableName.StartsWith("AspNet"))
        //     {
        //         entityType.SetTableName(tableName.Substring(6));
        //     }
        // }
    }
}
