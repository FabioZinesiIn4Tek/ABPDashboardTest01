using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace DashBoardTest01.Dashboards
{
    public abstract class DashboardCreateDtoBase
    {
        public string? Description { get; set; }
    }
}