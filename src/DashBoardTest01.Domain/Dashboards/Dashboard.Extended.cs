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
    public class Dashboard : DashboardBase
    {
        //<suite-custom-code-autogenerated>
        protected Dashboard()
        {

        }

        public Dashboard(Guid id, string? description = null)
            : base(id, description)
        {
        }
        //</suite-custom-code-autogenerated>

        //Write your custom code...
    }
}