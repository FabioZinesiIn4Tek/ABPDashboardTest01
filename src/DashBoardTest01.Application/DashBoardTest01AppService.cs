using DashBoardTest01.Localization;
using Volo.Abp.Application.Services;

namespace DashBoardTest01;

/* Inherit your application services from this class.
 */
public abstract class DashBoardTest01AppService : ApplicationService
{
    protected DashBoardTest01AppService()
    {
        LocalizationResource = typeof(DashBoardTest01Resource);
    }
}
