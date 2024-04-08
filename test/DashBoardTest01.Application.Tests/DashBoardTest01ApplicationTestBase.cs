using Volo.Abp.Modularity;

namespace DashBoardTest01;

public abstract class DashBoardTest01ApplicationTestBase<TStartupModule> : DashBoardTest01TestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
