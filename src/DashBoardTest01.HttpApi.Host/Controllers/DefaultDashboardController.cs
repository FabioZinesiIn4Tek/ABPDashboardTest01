using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardWeb;
using Microsoft.AspNetCore.DataProtection;


namespace DashBoardTest01.Controllers
{
    public class DefaultDashboardController : DashboardController
    {
        public DefaultDashboardController(DashboardConfigurator configurator, IDataProtectionProvider? dataProtectionProvider = null)
            : base(configurator, dataProtectionProvider)
        {
        }
    }
}
