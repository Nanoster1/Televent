namespace Televent.Service.Configurations.AppConfig;

public static class AppConfigDefaults
{
    public const string ConfigFolder = "Resources";
    public const string MainConfigFile = "Configurations/appsettings.json";
    public const string BuildingsData = "Data/buildings.json";

    public static string GetConfigPath(string fileName)
    {
        return Path.Combine(ConfigFolder, fileName);
    }
}