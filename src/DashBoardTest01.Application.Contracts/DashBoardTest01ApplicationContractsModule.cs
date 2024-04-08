using Volo.Abp.Account;
using Volo.Abp.AuditLogging;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.LanguageManagement;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TextTemplateManagement;
using Volo.Saas.Host;
using Volo.Abp.Gdpr;
using Volo.Abp.OpenIddict;

namespace DashBoardTest01;

[DependsOn(
    typeof(DashBoardTest01DomainSharedModule),
    typeof(AbpFeatureManagementApplicationContractsModule),
    typeof(AbpIdentityApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationContractsModule),
    typeof(AbpSettingManagementApplicationContractsModule),
    typeof(SaasHostApplicationContractsModule),
    typeof(AbpAuditLoggingApplicationContractsModule),
    typeof(AbpOpenIddictProApplicationContractsModule),
    typeof(AbpAccountPublicApplicationContractsModule),
    typeof(AbpAccountAdminApplicationContractsModule),
    typeof(LanguageManagementApplicationContractsModule),
    typeof(AbpGdprApplicationContractsModule),
    typeof(TextTemplateManagementApplicationContractsModule)
)]
public class DashBoardTest01ApplicationContractsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        DashBoardTest01DtoExtensions.Configure();
    }
}
