using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using DashBoardTest01.Shared;

namespace DashBoardTest01.Dashboards
{
    public partial interface IDashboardsAppService : IApplicationService
    {

        Task<PagedResultDto<DashboardDto>> GetListAsync(GetDashboardsInput input);

        Task<DashboardDto> GetAsync(Guid id);

        Task DeleteAsync(Guid id);

        Task<DashboardDto> CreateAsync(DashboardCreateDto input);

        Task<DashboardDto> UpdateAsync(Guid id, DashboardUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(DashboardExcelDownloadDto input);

        Task<DashBoardTest01.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}