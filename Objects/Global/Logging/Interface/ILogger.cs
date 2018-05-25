using Objects.Mob.Interface;
using Objects.Zone.Interface;

namespace Objects.Global.Logging.Interface
{
    public interface ILogger
    {
        ILogSettings Settings { get; set; }
        void FlushLogs();
        void Log(IMobileObject performer, LogSettings.LogLevel level, string message);
        void Log(IZone zone, LogSettings.LogLevel level, string message);
        void Log(LogSettings.LogLevel level, string message);

    }
}