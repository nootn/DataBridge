using System.Configuration;
using DataBridge.SqlServer.Configuration;
using NUnit.Framework;
using Serilog;

namespace DataBridge.SqlServer.IntegrationTests
{
    public abstract class FreshDatabaseIntegrationTestBase
    {
        [SetUp]
        public void RunBeforeAnyTests()
        {
            LogConfig.ConfigureLogging();
            Log.Debug("Running Test Setup");
        }
    }
}