using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace DashBoardTest01;

[Dependency(ReplaceServices = true)]
public class DashBoardTest01BrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "DashBoardTest01";
}
