using Microsoft.Extensions.Logging;
using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace Library.BL.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class LoggerExtensions
    {
        public static Microsoft.Extensions.Logging.ILogger<T> LoggingInstance<T>()
        {
            Log.Logger = Log.Logger ?? new LoggerConfiguration().WriteTo.Console().CreateLogger();
            ILoggerFactory factory = new LoggerFactory().AddSerilog(Log.Logger);
            return factory.CreateLogger<T>();
        }

        public static Microsoft.Extensions.Logging.ILogger<T> TestLoggingInstance<T>()
        {
            Log.Logger = Log.Logger ?? new LoggerConfiguration().CreateLogger();
            ILoggerFactory factory = new LoggerFactory().AddSerilog(Log.Logger);
            return factory.CreateLogger<T>();
        }
    }
}
