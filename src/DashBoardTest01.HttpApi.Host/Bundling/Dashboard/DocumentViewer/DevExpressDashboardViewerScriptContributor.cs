using System.Collections.Generic;
using DashBoardTest01.Bundling.Dashboard.ThirdParty;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace DashBoardTest01.Bundling.Dashboard.DocumentViewer
{

    [DependsOn(typeof(DevExpressDashboardThirdPartyScriptContributor))]
    public class DevExpressDocumentViewerScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/devexpress-analytics-core/js/dx-analytics-core.min.js");
            context.Files.AddIfNotContains("/libs/devexpress-analytics-core/js/dx-querybuilder.min.js");
            context.Files.AddIfNotContains("/libs/devexpress-dashboard/js/dx-dashboard.min.js");
        }
    }
}
