using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DashBoardTest01.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class DashBoardTest01DbContextFactory : IDesignTimeDbContextFactory<DashBoardTest01DbContext>
{
    public DashBoardTest01DbContext CreateDbContext(string[] args)
    {
        DashBoardTest01EfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<DashBoardTest01DbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new DashBoardTest01DbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../DashBoardTest01.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
