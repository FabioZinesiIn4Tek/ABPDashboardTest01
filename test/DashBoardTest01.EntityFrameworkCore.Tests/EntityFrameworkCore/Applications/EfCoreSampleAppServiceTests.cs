using DashBoardTest01.Samples;
using Xunit;

namespace DashBoardTest01.EntityFrameworkCore.Applications;

[Collection(DashBoardTest01TestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<DashBoardTest01EntityFrameworkCoreTestModule>
{

}
