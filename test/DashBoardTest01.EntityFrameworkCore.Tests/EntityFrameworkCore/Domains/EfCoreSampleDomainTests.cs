using DashBoardTest01.Samples;
using Xunit;

namespace DashBoardTest01.EntityFrameworkCore.Domains;

[Collection(DashBoardTest01TestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<DashBoardTest01EntityFrameworkCoreTestModule>
{

}
