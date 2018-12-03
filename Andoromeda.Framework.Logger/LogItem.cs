using System;

namespace Andoromeda.Framework.Logger
{
    public class LogItem
    {
        public string Message { get; set; }

        public DateTime Timestamp { get; set; }

        public string Catalog { get; set; }

        public string Position { get; set; }

        public LogLevel Level { get; set; }
    }
}
