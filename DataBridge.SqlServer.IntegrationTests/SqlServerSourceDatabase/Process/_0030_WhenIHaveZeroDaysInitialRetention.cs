using System;
using DataBridge.SqlServer.Interface;
using NUnit.Framework;
using Should;
using SpecsFor;

namespace DataBridge.SqlServer.IntegrationTests.SqlServerSourceDatabase.Process
{
    public class _0030_WhenIHaveZeroDaysInitialRetention : SpecsFor<SqlServer.SqlServerSourceDatabase>
    {
        private Exception _caughtException;

        protected override void Given()
        {
            TestDatabase.EnsureCleanDatabaseExists(TestDataMother.SourceDatabaseConnectionString.Valid, TestDataMother.SourceMsdbDatabaseConnectionString.Valid);

            GetMockFor<ISqlServerSource>()
                .Setup(_ => _.SourceTables)
                .Returns(TestDataMother.SourceTables.OneNonExistentTable);

            GetMockFor<ISqlServerSource>()
                .Setup(_ => _.SourceDatabaseConnectionString.Value)
                .Returns(TestDataMother.SourceDatabaseConnectionString.Valid);

            GetMockFor<ISqlServerSource>()
                .Setup(_ => _.ChangeTrackingRetentionInitialValueInDays)
                .Returns(TestDataMother.SourceDatabaseChangeTrackingRetentionInDays.InvalidZero);
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
            _caughtException.Message.ShouldEqual("value '0' is not greater than limit '0'.");
        }
    }
}