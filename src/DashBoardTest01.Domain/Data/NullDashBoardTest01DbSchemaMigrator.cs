using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace DashBoardTest01.Data;

/* This is used if database provider does't define
 * IDashBoardTest01DbSchemaMigrator implementation.
 */
public class NullDashBoardTest01DbSchemaMigrator : IDashBoardTest01DbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
