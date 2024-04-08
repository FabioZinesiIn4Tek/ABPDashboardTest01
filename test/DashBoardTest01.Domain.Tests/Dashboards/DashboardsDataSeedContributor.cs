using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using DashBoardTest01.Dashboards;

namespace DashBoardTest01.Dashboards
{
    public class DashboardsDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public DashboardsDataSeedContributor(IDashboardRepository dashboardRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _dashboardRepository = dashboardRepository;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _dashboardRepository.InsertAsync(new Dashboard
            (
                id: Guid.Parse("ddb2ffd1-f9a6-4eec-a3b4-575efea0adfb"),
                description: "6da452a2fa6c4490bed344b19071d085c2d3bfe424e449d9"
            ));

            await _dashboardRepository.InsertAsync(new Dashboard
            (
                id: Guid.Parse("ce243670-ded9-4bd1-b51f-3d9d426c040f"),
                description: "a1c8cc5912854f388e23e630c5ae00949a0d23ddfc894256945292c9e7e700"
            ));

            await _unitOfWorkManager!.Current!.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}