using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using DashBoardTest01.Permissions;
using DashBoardTest01.Dashboards;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using DashBoardTest01.Shared;

namespace DashBoardTest01.Dashboards
{
    [RemoteService(IsEnabled = false)]
    [Authorize(DashBoardTest01Permissions.Dashboards.Default)]
    public abstract class DashboardsAppServiceBase : ApplicationService
    {
        protected IDistributedCache<DashboardExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        protected IDashboardRepository _dashboardRepository;
        protected DashboardManager _dashboardManager;

        public DashboardsAppServiceBase(IDashboardRepository dashboardRepository, DashboardManager dashboardManager, IDistributedCache<DashboardExcelDownloadTokenCacheItem, string> excelDownloadTokenCache)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _dashboardRepository = dashboardRepository;
            _dashboardManager = dashboardManager;
        }

        public virtual async Task<PagedResultDto<DashboardDto>> GetListAsync(GetDashboardsInput input)
        {
            var totalCount = await _dashboardRepository.GetCountAsync(input.FilterText, input.Description);
            var items = await _dashboardRepository.GetListAsync(input.FilterText, input.Description, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<DashboardDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Dashboard>, List<DashboardDto>>(items)
            };
        }

        public virtual async Task<DashboardDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Dashboard, DashboardDto>(await _dashboardRepository.GetAsync(id));
        }

        [Authorize(DashBoardTest01Permissions.Dashboards.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _dashboardRepository.DeleteAsync(id);
        }

        [Authorize(DashBoardTest01Permissions.Dashboards.Create)]
        public virtual async Task<DashboardDto> CreateAsync(DashboardCreateDto input)
        {

            var dashboard = await _dashboardManager.CreateAsync(
            input.Description
            );

            return ObjectMapper.Map<Dashboard, DashboardDto>(dashboard);
        }

        [Authorize(DashBoardTest01Permissions.Dashboards.Edit)]
        public virtual async Task<DashboardDto> UpdateAsync(Guid id, DashboardUpdateDto input)
        {

            var dashboard = await _dashboardManager.UpdateAsync(
            id,
            input.Description, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Dashboard, DashboardDto>(dashboard);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(DashboardExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _dashboardRepository.GetListAsync(input.FilterText, input.Description);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Dashboard>, List<DashboardExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Dashboards.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public virtual async Task<DashBoardTest01.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new DashboardExcelDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new DashBoardTest01.Shared.DownloadTokenResultDto
            {
                Token = token
            };
        }
    }
}