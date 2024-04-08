using System.Collections.Generic;
using DashBoardTest01.Bundling.Dashboard.DocumentViewer;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace DashBoardTest01.Bundling.Dashboard.DocumentDesigner
{

    [DependsOn(
        typeof(DevExpressDocumentViewerStyleContributor)
    )]
    public class DevExpressDocumentDesignerStyleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/devexpress-analytics-core/css/dx-querybuilder.css");
            //context.Files.AddIfNotContains("/libs/devexpress-dashboard/css/dx-reportdesigner.css");
        }
    }
}
