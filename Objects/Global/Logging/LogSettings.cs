using Objects.Global.Logging.Interface;
using Objects.Zone.Interface;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Objects.Global.Logging
{
    //TODO ADD UNIT TESTS TO SETTINGS
    [ExcludeFromCodeCoverage]
    public class LogSettings : ILogSettings
    {
        private ConcurrentDictionary<string, LogLevel> _playerLogLevel { get; } = new ConcurrentDictionary<string, LogLevel>();
        private ConcurrentDictionary<int, LogLevel> _zoneLogLevel { get; } = new ConcurrentDictionary<int, LogLevel>();

        public LogLevel GlobalLevel { get; set; } = LogLevel.INFO;

        public string LogDirectory { get; set; }

        public LogSettings()
        {
            LoadAppConfigSettings();
        }

        public LogLevel PlayerLevel(string playerName)
        {
            if (_playerLogLevel.TryGetValue(playerName, out LogLevel level))
            {
                return level;
            }
            else
            {
                return GlobalLevel;
            }
        }
        public void SetPlayerLevel(string playerName, LogLevel level)
        {
            _playerLogLevel.AddOrUpdate(playerName, level, (k, v) => v = level);
        }

        public LogLevel ZoneLevel(int zoneId)
        {
            if (_zoneLogLevel.TryGetValue(zoneId, out LogLevel level))
            {
                return level;
            }
            else
            {
                return GlobalLevel;
            }
        }

        public void SetZoneLevel(int zoneId, LogLevel level)
        {
            _zoneLogLevel.AddOrUpdate(zoneId, level, (k, v) => v = level);
        }

        private void LoadAppConfigSettings()
        {
            foreach (IZone zone in GlobalReference.GlobalValues.World.Zones.Values)
            {
                //TODO Make zone logging configs work with .core

                //try
                //{
                //    string zoneLevel = ConfigurationManager.AppSettings["Zone" + zone.Id + "LogLevel"];
                //    if (zoneLevel != null)
                //    {
                //        LogLevel level;
                //        Enum.TryParse(zoneLevel, out level);
                //        SetZoneLevel(zone.Id, level);
                //    }
                //}
                //catch
                //{

                //}
            }

        }

        public enum LogLevel
        {
            ALL,
            DEBUGVERBOSE,
            DEBUG,
            INFO,
            WARN,
            ERROR,
            FATAL,
            Off
        }
    }
}
