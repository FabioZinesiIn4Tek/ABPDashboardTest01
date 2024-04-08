using System.Collections.Generic;
using DashBoardTest01.Bundling.Common;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace DashBoardTest01.Bundling.Dashboard.DocumentViewer
{
    [DependsOn(
    typeof(DevExtremeCommonStyleContributor)
)]
    public class DevExpressDocumentViewerStyleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            
            context.Files.AddIfNotContains("/libs/devexpress-analytics-core/css/dx-analytics.common.css");
            context.Files.AddIfNotContains("/libs/devexpress-analytics-core/css/dx-analytics.light.css");
            context.Files.AddIfNotContains("/libs/devexpress-analytics-core/css/dx-querybuilder.css");

            context.Files.AddIfNotContains("/libs/devexpress-dashboard/css/dx-dashboard.light.min.css");
        }
    }
}
