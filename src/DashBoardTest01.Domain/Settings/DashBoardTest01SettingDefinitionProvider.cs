using Volo.Abp.Settings;

namespace DashBoardTest01.Settings;

public class DashBoardTest01SettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(DashBoardTest01Settings.MySetting1));
    }
}
