using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using DashBoardTest01.Dashboards;

namespace DashBoardTest01.Controllers.Dashboards
{
    [RemoteService]
    [Area("app")]
    [ControllerName("Dashboard")]
    [Route("api/app/dashboards")]

    public class DashboardController : DashboardControllerBase, IDashboardsAppService
    {
        public DashboardController(IDashboardsAppService dashboardsAppService) : base(dashboardsAppService)
        {
        }
    }
}