using System;
using System.Threading.Tasks;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace Andoromeda.Framework.Logger
{
    public class MySqlLogger : BaseLogger, IDisposable
    {
        private readonly string _dbUser = "kyubey2%log-rw";
        private readonly string _dbPassword = "bce01ca5-7572-4962-811d-d4fdfe4b785b";
        private readonly string _dbConnectionString;
        private readonly LogSemaphore _semaphore = new LogSemaphore();
        private MySqlConnection _conn;

        public MySqlLogger(string catalog) : base(catalog)
        {
            _dbConnectionString = $"Server=kyubey2.mysqldb.chinacloudapi.cn;Port=3306;Database=logs;Uid={_dbUser};Pwd={_dbPassword};";
            Connect();
        }

        public MySqlLogger(string catalog, string connectionString) : base(catalog)
        {
            _dbConnectionString = connectionString;
            Connect();
        }

        private void Connect()
        {
            _conn = new MySqlConnection(_dbConnectionString);
            _conn.Open();
            StartPushLogsAsync();
        }

        public override void Log(LogLevel level, string message)
        {
            var trace = new StackTrace();
            var frame = trace.GetFrame(1);
            var file = frame.GetFileName();
            var method = frame.GetMethod();
            var line = frame.GetFileLineNumber();
            var className = method.ReflectedType.Name;

            _semaphore.LogAttain(new LogItem
            {
                Catalog = Catalog,
                Message = message,
                Position = $"{file} line: {line}, {className}.{method}",
                Level = level,
                Timestamp = DateTime.UtcNow
            });
        }

        private async Task StartPushLogsAsync()
        {
            while(true)
            {
                var log = await _semaphore.WaitForLogAttainAsync();
                using (var cmd = new MySqlCommand("INSERT INTO `Logs` (`Catalog`, `Level`, `Message`, `Position`, `Timestamp`) VALUES (@p1, @p2, @p3, @p4, @p5)", _conn))
                {
                    cmd.Parameters.Add(new MySqlParameter("@p1", log.Catalog));
                    cmd.Parameters.Add(new MySqlParameter("@p2", log.Level));
                    cmd.Parameters.Add(new MySqlParameter("@p3", log.Message));
                    cmd.Parameters.Add(new MySqlParameter("@p4", log.Position));
                    cmd.Parameters.Add(new MySqlParameter("@p5", log.Timestamp));
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public void Dispose()
        {
            _conn?.Dispose();
        }
    }
}
