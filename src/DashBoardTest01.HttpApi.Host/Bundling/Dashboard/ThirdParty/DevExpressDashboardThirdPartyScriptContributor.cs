using System.Collections.Generic;
using DashBoardTest01.Bundling.Common;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace DashBoardTest01.Bundling.Dashboard.ThirdParty
{

    [DependsOn(
        typeof(DevExtremeDashboardCommonScriptContributor)
    )]
    public class DevExpressDashboardThirdPartyScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            // cldrj related scripts
            context.Files.AddIfNotContains("/libs/devexpress-dashboard/cldrjs/cldr.js");
            context.Files.AddIfNotContains("/libs/devexpress-dashboard/cldrjs/event.js");
            context.Files.AddIfNotContains("/libs/devexpress-dashboard/cldrjs/supplemental.js");
            context.Files.AddIfNotContains("/libs/devexpress-dashboard/cldrjs/unresolved.js");
            // globalize related scripts
            context.Files.AddIfNotContains("/libs/devexpress-dashboard/globalize/globalize.js");
            context.Files.AddIfNotContains("/libs/devexpress-dashboard/globalize/currency.js");
            context.Files.AddIfNotContains("/libs/devexpress-dashboard/globalize/date.js");
            context.Files.AddIfNotContains("/libs/devexpress-dashboard/globalize/message.js");
            context.Files.AddIfNotContains("/libs/devexpress-dashboard/globalize/number.js");
            // ace-builds related scripts
            context.Files.AddIfNotContains("/libs/devexpress-dashboard/ace-builds/ace.js");
            context.Files.AddIfNotContains("/libs/devexpress-dashboard/ace-builds/ext-language_tools.js");
            context.Files.AddIfNotContains("/libs/devexpress-dashboard/ace-builds/theme-ambiance.js");
            context.Files.AddIfNotContains("/libs/devexpress-dashboard/ace-builds/theme-dreamweaver.js");
            context.Files.AddIfNotContains("/libs/devexpress-dashboard/ace-builds/text.js");
        }
    }
}
