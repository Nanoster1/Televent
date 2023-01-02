using static Televent.Service.Configurations.AppConfig.AppConfigDefaults;

namespace Televent.Service.Configurations.AppConfig;

public static class AppConfigBootstrap
{
    public static void SetUpAppConfiguration(HostBuilderContext context, IConfigurationBuilder builder)
    {
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile(GetConfigPath(MainConfigFile), optional: false, reloadOnChange: true);
        builder.AddEnvironmentVariables();
    }
}