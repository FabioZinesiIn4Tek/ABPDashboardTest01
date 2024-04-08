using Volo.Abp.Application.Dtos;
using System;

namespace DashBoardTest01.Dashboards
{
    public abstract class DashboardExcelDownloadDtoBase
    {
        public string DownloadToken { get; set; } = null!;

        public string? FilterText { get; set; }

        public string? Description { get; set; }

        public DashboardExcelDownloadDtoBase()
        {

        }
    }
}