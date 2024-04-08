using Volo.Abp.Modularity;

namespace DashBoardTest01;

/* Inherit from this class for your domain layer tests. */
public abstract class DashBoardTest01DomainTestBase<TStartupModule> : DashBoardTest01TestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
