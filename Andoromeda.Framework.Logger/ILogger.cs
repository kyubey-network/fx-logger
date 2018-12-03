namespace Andoromeda.Framework.Logger
{
    public enum LogLevel
    {
        Info,
        Warn,
        Error
    }

    public interface ILogger
    {
        string Catalog { get; }

        void Log(LogLevel level, string message);

        void LogInfo(string message);

        void LogWarn(string message);

        void LogError(string message);
    }
}