namespace Objects.Global.Logging.Interface
{
    public interface ILogSettings
    {
        LogSettings.LogLevel GlobalLevel { get; set; }
        LogSettings.LogLevel PlayerLevel(string playerName);
        LogSettings.LogLevel ZoneLevel(int zoneLevel);
        string LogDirectory { get; set; }
    }
}