using DashBoardTest01.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace DashBoardTest01.Permissions;

public class DashBoardTest01PermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(DashBoardTest01Permissions.GroupName);

        myGroup.AddPermission(DashBoardTest01Permissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
        myGroup.AddPermission(DashBoardTest01Permissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(DashBoardTest01Permissions.MyPermission1, L("Permission:MyPermission1"));

        var dashboardPermission = myGroup.AddPermission(DashBoardTest01Permissions.Dashboards.Default, L("Permission:Dashboards"));
        dashboardPermission.AddChild(DashBoardTest01Permissions.Dashboards.Create, L("Permission:Create"));
        dashboardPermission.AddChild(DashBoardTest01Permissions.Dashboards.Edit, L("Permission:Edit"));
        dashboardPermission.AddChild(DashBoardTest01Permissions.Dashboards.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<DashBoardTest01Resource>(name);
    }
}