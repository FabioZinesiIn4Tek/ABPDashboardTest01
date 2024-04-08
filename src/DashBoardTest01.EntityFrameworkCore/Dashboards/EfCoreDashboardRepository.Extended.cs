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
    public class EfCoreDashboardRepository : EfCoreDashboardRepositoryBase, IDashboardRepository
    {
        public EfCoreDashboardRepository(IDbContextProvider<DashBoardTest01DbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}