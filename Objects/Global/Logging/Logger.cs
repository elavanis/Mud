using Objects.Global.Logging.Interface;
using Objects.Mob;
using Objects.Mob.Interface;
using Objects.Room.Interface;
using Objects.Zone.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Objects.Global.Logging.LogMessage;

namespace Objects.Global.Logging
{
    //TODO ADD UNIT TESTS TO LOGGER
    [ExcludeFromCodeCoverage]
    public class Logger : ILogger
    {
        public ILogSettings Settings { get; set; } = new LogSettings();

        private static ConcurrentQueue<ILogMessage> Queue = new ConcurrentQueue<ILogMessage>();

        public void Log(IMobileObject performer, LogSettings.LogLevel logLevel, string message)
        {
            if (performer != null)
            {
                if (performer is INonPlayerCharacter npc)
                {
                    if (Settings.ZoneLevel(npc.Zone) <= logLevel)
                    {
                        Queue.Enqueue(new LogMessage(FormatLineForLog(message), LogType.NonPlayerCharacter, string.Format("{0}-{1}", npc.Zone, npc.Id)));
                    }
                }
                else
                {
                    if (performer is IPlayerCharacter pc)
                    {
                        if (Settings.PlayerLevel(pc.Name) <= logLevel)
                        {
                            Queue.Enqueue(new LogMessage(FormatLineForLog(message), LogType.PlayerCharacter, pc.Name));
                        }
                    }
                }

                //room will be null if the player is logging out
                if (performer.Room != null)
                {
                    IZone zone = GlobalReference.GlobalValues.World.Zones[performer.Room.Zone];
                    Log(zone, logLevel, message);
                }
            }
            else
            {
                Log(logLevel, message);
            }
        }

        public void Log(IZone zone, LogSettings.LogLevel logLevel, string message)
        {
            if (Settings.ZoneLevel(zone.Id) <= logLevel)
            {
                Queue.Enqueue(new LogMessage(FormatLineForLog(message), LogType.Zone, zone.Id.ToString()));
            }
        }

        public void Log(LogSettings.LogLevel logLevel, string message)
        {
            if (Settings.GlobalLevel <= logLevel)
            {
                Queue.Enqueue(new LogMessage(FormatLineForLog(message), LogType.System, null));
            }
        }

        private string FormatLineForLog(string message)
        {
            return string.Format("{0} -- {1}", DateTime.Now.ToString(), message, Environment.NewLine);
        }

        public void FlushLogs()
        {
            Dictionary<string, List<string>> localMessageQueue = new Dictionary<string, List<string>>();
            ILogMessage message = null;
            while (Queue.TryDequeue(out message))
            {
                string directory;
                string fileName;
                switch (message.Type)
                {
                    case LogType.NonPlayerCharacter:
                        directory = Path.Combine(Settings.LogDirectory, "NonPlayerCharacter", DateTime.Now.ToString("yyyy_MM_dd"));
                        fileName = Path.Combine(directory, message.LogKey + ".log");
                        break;
                    case LogType.PlayerCharacter:
                        directory = Path.Combine(Settings.LogDirectory, "PlayerCharacter", DateTime.Now.ToString("yyyy_MM_dd"));
                        fileName = Path.Combine(directory, message.LogKey + ".log");
                        break;
                    case LogType.Zone:
                        directory = Path.Combine(Settings.LogDirectory, "Zone", DateTime.Now.ToString("yyyy_MM_dd"));
                        fileName = Path.Combine(directory, message.LogKey + ".log");
                        break;
                    case LogType.System:
                    default:
                        directory = Path.Combine(Settings.LogDirectory, "System", DateTime.Now.ToString("yyyy_MM_dd"));
                        fileName = Path.Combine(directory, "System.log");
                        break;
                }

                localMessageQueue.TryGetValue(fileName, out List<string> localQueue);
                if (localQueue == null)
                {
                    GlobalReference.GlobalValues.FileIO.EnsureDirectoryExists(directory);
                    localQueue = new List<string>();
                    localMessageQueue.Add(fileName, localQueue);
                }
                localQueue.Add(message.Message);

            }

            Parallel.ForEach(localMessageQueue.Keys, fileName =>
            {
                GlobalReference.GlobalValues.FileIO.AppendFile(fileName, localMessageQueue[fileName]);
            });
        }
    }
}
