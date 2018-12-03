using Andoromeda.Framework.Logger;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MySqlLoggerExtensions
    {
        public static IServiceCollection AddMySqlLogger(this IServiceCollection self, string catalog)
        {
            return self.AddSingleton<ILogger, MySqlLogger>(x => new MySqlLogger(catalog));
        }

        public static IServiceCollection AddMySqlLogger(this IServiceCollection self, string catalog, string connectionString)
        {
            return self.AddSingleton<ILogger, MySqlLogger>(x => new MySqlLogger(catalog, connectionString));
        }
    }
}
