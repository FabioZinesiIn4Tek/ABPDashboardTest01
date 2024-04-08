using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace DashBoardTest01.Dashboards
{
    public abstract class DashboardManagerBase : DomainService
    {
        protected IDashboardRepository _dashboardRepository;

        public DashboardManagerBase(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public virtual async Task<Dashboard> CreateAsync(
        string? description = null)
        {

            var dashboard = new Dashboard(
             GuidGenerator.Create(),
             description
             );

            return await _dashboardRepository.InsertAsync(dashboard);
        }

        public virtual async Task<Dashboard> UpdateAsync(
            Guid id,
            string? description = null, [CanBeNull] string? concurrencyStamp = null
        )
        {

            var dashboard = await _dashboardRepository.GetAsync(id);

            dashboard.Description = description;

            dashboard.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _dashboardRepository.UpdateAsync(dashboard);
        }

    }
}