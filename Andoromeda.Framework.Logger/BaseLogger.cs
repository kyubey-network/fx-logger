namespace Andoromeda.Framework.Logger
{
    public abstract class BaseLogger : ILogger
    {
        private string _catalog;

        public BaseLogger(string catalog)
        {
            _catalog = catalog;
        }

        public string Catalog => _catalog;

        public abstract void Log(LogLevel level, string message);

        public void LogError(string message)
        {
            Log(LogLevel.Error, message);
        }

        public void LogInfo(string message)
        {
            Log(LogLevel.Info, message);
        }

        public void LogWarn(string message)
        {
            Log(LogLevel.Warn, message);
        }
    }
}
