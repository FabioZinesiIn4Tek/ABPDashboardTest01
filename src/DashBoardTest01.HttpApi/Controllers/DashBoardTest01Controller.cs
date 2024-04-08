using DashBoardTest01.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace DashBoardTest01.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class DashBoardTest01Controller : AbpControllerBase
{
    protected DashBoardTest01Controller()
    {
        LocalizationResource = typeof(DashBoardTest01Resource);
    }
}
