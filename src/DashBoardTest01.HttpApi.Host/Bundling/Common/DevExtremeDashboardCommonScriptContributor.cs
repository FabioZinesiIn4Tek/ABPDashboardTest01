using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JQuery;
using Volo.Abp.Modularity;

namespace DashBoardTest01.Bundling.Common
{

    [DependsOn(
        typeof(JQueryScriptContributor)
    )]
    public class DevExtremeDashboardCommonScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/devextreme/js/jquery-ui.min.js");
            context.Files.AddIfNotContains("/libs/devexpress-dashboard/js/knockout-latest.js");
            context.Files.AddIfNotContains("/libs/devextreme/js/dx.all.js"); // Has to be added after jquery and knockout
        }
    }
}
