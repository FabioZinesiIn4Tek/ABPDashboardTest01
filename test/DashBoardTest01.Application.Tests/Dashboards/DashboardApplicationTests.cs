using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Xunit;

namespace DashBoardTest01.Dashboards
{
    public abstract class DashboardsAppServiceTests<TStartupModule> : DashBoardTest01ApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IDashboardsAppService _dashboardsAppService;
        private readonly IRepository<Dashboard, Guid> _dashboardRepository;

        public DashboardsAppServiceTests()
        {
            _dashboardsAppService = GetRequiredService<IDashboardsAppService>();
            _dashboardRepository = GetRequiredService<IRepository<Dashboard, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _dashboardsAppService.GetListAsync(new GetDashboardsInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Id == Guid.Parse("ddb2ffd1-f9a6-4eec-a3b4-575efea0adfb")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("ce243670-ded9-4bd1-b51f-3d9d426c040f")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _dashboardsAppService.GetAsync(Guid.Parse("ddb2ffd1-f9a6-4eec-a3b4-575efea0adfb"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("ddb2ffd1-f9a6-4eec-a3b4-575efea0adfb"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new DashboardCreateDto
            {
                Description = "9e663f35fe02"
            };

            // Act
            var serviceResult = await _dashboardsAppService.CreateAsync(input);

            // Assert
            var result = await _dashboardRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Description.ShouldBe("9e663f35fe02");
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new DashboardUpdateDto()
            {
                Description = "63985804608d443c8e825b4b89ecc81b2c3a00139d624e4e8c48d7e1a7074aeb7fa69a09c4ae4d43ae15ea47"
            };

            // Act
            var serviceResult = await _dashboardsAppService.UpdateAsync(Guid.Parse("ddb2ffd1-f9a6-4eec-a3b4-575efea0adfb"), input);

            // Assert
            var result = await _dashboardRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Description.ShouldBe("63985804608d443c8e825b4b89ecc81b2c3a00139d624e4e8c48d7e1a7074aeb7fa69a09c4ae4d43ae15ea47");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _dashboardsAppService.DeleteAsync(Guid.Parse("ddb2ffd1-f9a6-4eec-a3b4-575efea0adfb"));

            // Assert
            var result = await _dashboardRepository.FindAsync(c => c.Id == Guid.Parse("ddb2ffd1-f9a6-4eec-a3b4-575efea0adfb"));

            result.ShouldBeNull();
        }
    }
}