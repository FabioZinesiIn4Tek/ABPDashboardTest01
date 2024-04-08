using System;
using DashBoardTest01.Shared;
using Volo.Abp.AutoMapper;
using DashBoardTest01.Dashboards;
using AutoMapper;

namespace DashBoardTest01;

public class DashBoardTest01ApplicationAutoMapperProfile : Profile
{
    public DashBoardTest01ApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Dashboard, DashboardDto>();
        CreateMap<Dashboard, DashboardExcelDto>();
    }
}