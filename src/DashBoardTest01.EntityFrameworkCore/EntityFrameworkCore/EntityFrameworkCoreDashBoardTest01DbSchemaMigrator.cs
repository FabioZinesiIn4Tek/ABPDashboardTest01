using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DashBoardTest01.Data;
using Volo.Abp.DependencyInjection;

namespace DashBoardTest01.EntityFrameworkCore;

public class EntityFrameworkCoreDashBoardTest01DbSchemaMigrator
    : IDashBoardTest01DbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreDashBoardTest01DbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the DashBoardTest01DbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<DashBoardTest01DbContext>()
            .Database
            .MigrateAsync();
    }
}
