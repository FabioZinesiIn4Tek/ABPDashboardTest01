using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace DashBoardTest01.Dashboards
{
    public abstract class DashboardUpdateDtoBase : IHasConcurrencyStamp
    {
        public string? Description { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;
    }
}