using Volo.Abp.Application.Dtos;
using System;

namespace DashBoardTest01.Dashboards
{
    public abstract class GetDashboardsInputBase : PagedAndSortedResultRequestDto
    {

        public string? FilterText { get; set; }

        public string? Description { get; set; }

        public GetDashboardsInputBase()
        {

        }
    }
}