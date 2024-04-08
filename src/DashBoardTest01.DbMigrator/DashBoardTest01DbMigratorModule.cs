using DashBoardTest01.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace DashBoardTest01.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(DashBoardTest01EntityFrameworkCoreModule),
    typeof(DashBoardTest01ApplicationContractsModule)
)]
public class DashBoardTest01DbMigratorModule : AbpModule
{
}
