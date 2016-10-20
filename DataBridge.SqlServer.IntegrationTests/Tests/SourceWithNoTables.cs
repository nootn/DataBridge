using System;
using System.Configuration;
using DataBridge.SqlServer.Configuration;
using NUnit.Framework;
using Serilog;
using Shouldly;

namespace DataBridge.SqlServer.IntegrationTests.Tests
{
    [TestFixture]
    public class SourceWithNoTables : FreshDatabaseIntegrationTestBase
    {
        [Test]
        public void ThrowsNoTableError()
        {
            try
            {
                var source = new SqlServerSourceDatabase(Log.ForContext<SqlServerSourceDatabase>(), 
                    ConfigurationManager.GetSection("ThrowsNoTableError") as SqlServerSourceConfigSection);
                source.Process();
                Assert.Fail("Should have thrown exception");
            }
            catch (Exception ex)
            {
                ex.Message.ShouldContain("There were no tables to configure - nothing to do!");
            }
        }
    }
}
