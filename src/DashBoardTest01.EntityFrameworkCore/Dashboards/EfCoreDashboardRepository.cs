using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using DashBoardTest01.EntityFrameworkCore;

namespace DashBoardTest01.Dashboards
{
    public abstract class EfCoreDashboardRepositoryBase : EfCoreRepository<DashBoardTest01DbContext, Dashboard, Guid>
    {
        public EfCoreDashboardRepositoryBase(IDbContextProvider<DashBoardTest01DbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task<List<Dashboard>> GetListAsync(
            string? filterText = null,
            string? description = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, description);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? DashboardConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? description = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, description);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Dashboard> ApplyFilter(
            IQueryable<Dashboard> query,
            string? filterText = null,
            string? description = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Description!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.Description.Contains(description));
        }
    }
}