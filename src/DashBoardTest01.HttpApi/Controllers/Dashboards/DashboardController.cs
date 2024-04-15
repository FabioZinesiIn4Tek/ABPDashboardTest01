using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using DashBoardTest01.Dashboards;
using Volo.Abp.Content;
using DashBoardTest01.Shared;

namespace DashBoardTest01.Controllers.Dashboards
{
    [RemoteService]
    [Area("app")]
    [ControllerName("Dashboard")]
    [Route("api/app/dashboards")]

    public abstract class DashboardControllerBase : AbpController
    {
        protected IDashboardsAppService _dashboardsAppService;

        public DashboardControllerBase(IDashboardsAppService dashboardsAppService)
        {
            _dashboardsAppService = dashboardsAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<DashboardDto>> GetListAsync(GetDashboardsInput input)
        {
            return _dashboardsAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<DashboardDto> GetAsync(Guid id)
        {
            return _dashboardsAppService.GetAsync(id);
        }

        [HttpPost]
        public virtual Task<DashboardDto> CreateAsync(DashboardCreateDto input)
        {
            return _dashboardsAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<DashboardDto> UpdateAsync(Guid id, DashboardUpdateDto input)
        {
            return _dashboardsAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _dashboardsAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(DashboardExcelDownloadDto input)
        {
            return _dashboardsAppService.GetListAsExcelFileAsync(input);
        }

        [HttpGet]
        [Route("download-token")]
        public virtual Task<DashBoardTest01.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            return _dashboardsAppService.GetDownloadTokenAsync();
        }
    }
}