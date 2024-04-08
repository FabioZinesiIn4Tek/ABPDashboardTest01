using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace DashBoardTest01.Dashboards
{
    public abstract class DashboardDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string? Description { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;

    }
}