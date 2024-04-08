using Volo.Abp.Modularity;

namespace DashBoardTest01;

[DependsOn(
    typeof(DashBoardTest01ApplicationModule),
    typeof(DashBoardTest01DomainTestModule)
)]
public class DashBoardTest01ApplicationTestModule : AbpModule
{

}
