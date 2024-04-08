using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace DashBoardTest01.Dashboards
{
    public abstract class DashboardBase : FullAuditedAggregateRoot<Guid>
    {
        [CanBeNull]
        public virtual string? Description { get; set; }

        protected DashboardBase()
        {

        }

        public DashboardBase(Guid id, string? description = null)
        {

            Id = id;
            Description = description;
        }

    }
}