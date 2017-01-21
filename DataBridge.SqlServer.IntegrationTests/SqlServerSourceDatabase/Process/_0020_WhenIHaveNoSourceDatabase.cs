using System;
using DataBridge.SqlServer.Interface;
using NUnit.Framework;
using Serilog;
using Should;
using SpecsFor;

namespace DataBridge.SqlServer.IntegrationTests.SqlServerSourceDatabase.Process
{
    public class _0020_WhenIHaveNoSourceDatabase : SpecsFor<SqlServer.SqlServerSourceDatabase>
    {
        private Exception _caughtException;

        protected override void Given()
        {
            SUT.OverrideLogger(Log.Logger);
            GetMockFor<ISqlServerSource>()
                .Setup(_ => _.SourceTables)
                .Returns(TestDataMother.SourceTables.OneValidTable);

            GetMockFor<ISqlServerSource>()
                .Setup(_ => _.SourceDatabaseConnectionString.Value)
                .Returns(TestDataMother.SourceDatabaseConnectionString.InvalidEmpty);
        }

        protected override void When()
        {
            try
            {
                SUT.Process();
            }
            catch (Exception ex)
            {
                _caughtException = ex;
            }
        }

        [Test]
        public void then_an_exception_is_thrown()
        {
            _caughtException.ShouldNotBeNull();
        }

        [Test]
        public void then_the_correct_exception_is_thrown()
        {
            _caughtException.Message.ShouldStartWith(
                SqlServer.SqlServerSourceDatabase.ExceptionMessageInvalidSourceDatabase);
        }
    }
}