namespace Objects.Global.Logging.Interface
{
    public interface ILogMessage
    {
        string Message { get; }
        LogMessage.LogType Type { get; }

        string LogKey { get; }
    }
}