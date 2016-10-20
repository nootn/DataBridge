using System.Configuration;
using Serilog;

namespace DataBridge.SqlServer.IntegrationTests
{
    public class LogConfig
    {
        public static void ConfigureLogging()
        {
            var log = new LoggerConfiguration()
                .WriteTo.NUnitOutput()
                .WriteTo.Seq(ConfigurationManager.AppSettings["SeqServerUrl"])
                .CreateLogger();
            Log.Logger = log;
        }
    }
}
