using Objects.Global.Logging.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Global.Logging
{
    public class LogMessage : ILogMessage
    {
        [ExcludeFromCodeCoverage]
        public string Message { get; }

        [ExcludeFromCodeCoverage]
        public LogType Type { get; }

        [ExcludeFromCodeCoverage]
        public string LogKey { get; }

        public LogMessage(string message, LogType type, string logKey)
        {
            Message = message;
            Type = type;
            LogKey = logKey;
        }

        public enum LogType
        {
            PlayerCharacter,
            NonPlayerCharacter,
            Zone,
            System
        }
    }
}
