using Volo.Abp.Modularity;

namespace DashBoardTest01;

[DependsOn(
    typeof(DashBoardTest01DomainModule),
    typeof(DashBoardTest01TestBaseModule)
)]
public class DashBoardTest01DomainTestModule : AbpModule
{

}
