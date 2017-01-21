using NUnit.Framework;
using Serilog;
using Serilog.Events;

namespace DataBridge.SqlServer.IntegrationTests
{
    [SetUpFixture]
    public class TestConfig
    {
        [SetUp]
        public void Begin()
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithThreadId()
                .WriteTo.Seq("http://localhost:5341", LogEventLevel.Debug)
                .CreateLogger();
        }

        [TearDown]
        public void End()
        {
            Log.CloseAndFlush();
        }
    }
}