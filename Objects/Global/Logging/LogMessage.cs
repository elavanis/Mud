using Objects.Global.Logging.Interface;
using System.Diagnostics.CodeAnalysis;

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
