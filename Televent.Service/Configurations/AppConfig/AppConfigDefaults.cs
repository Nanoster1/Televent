namespace Televent.Service.Configurations.AppConfig;

public static class AppConfigDefaults
{
    public const string ConfigFolder = "Resources/Configurations";
    public const string MainConfigFile = "appsettings.json";

    public static string GetConfigPath(string fileName)
    {
        return Path.Combine(ConfigFolder, fileName);
    }
}