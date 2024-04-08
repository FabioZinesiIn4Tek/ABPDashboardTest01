using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using DashBoardTest01.Dashboards;
using DashBoardTest01.EntityFrameworkCore;
using Xunit;

namespace DashBoardTest01.EntityFrameworkCore.Domains.Dashboards
{
    public class DashboardRepositoryTests : DashBoardTest01EntityFrameworkCoreTestBase
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardRepositoryTests()
        {
            _dashboardRepository = GetRequiredService<IDashboardRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _dashboardRepository.GetListAsync(
                    description: "6da452a2fa6c4490bed344b19071d085c2d3bfe424e449d9"
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("ddb2ffd1-f9a6-4eec-a3b4-575efea0adfb"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _dashboardRepository.GetCountAsync(
                    description: "a1c8cc5912854f388e23e630c5ae00949a0d23ddfc894256945292c9e7e700"
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}