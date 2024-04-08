using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace DashBoardTest01.Dashboards
{
    public partial interface IDashboardRepository : IRepository<Dashboard, Guid>
    {
        Task<List<Dashboard>> GetListAsync(
            string? filterText = null,
            string? description = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string? filterText = null,
            string? description = null,
            CancellationToken cancellationToken = default);
    }
}