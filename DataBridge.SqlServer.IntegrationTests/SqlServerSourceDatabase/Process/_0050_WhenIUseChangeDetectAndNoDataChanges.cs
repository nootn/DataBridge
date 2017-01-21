using System;
using System.Collections.Generic;
using System.Threading;
using DataBridge.Core;
using DataBridge.Core.Interface;
using DataBridge.SqlServer.Interface;
using Moq;
using NUnit.Framework;
using Should;
using SpecsFor;

namespace DataBridge.SqlServer.IntegrationTests.SqlServerSourceDatabase.Process
{
    public class _0050_WhenIUseChangeDetectAndNoDataChanges : SpecsFor<SqlServer.SqlServerSourceDatabase>
    {
        protected override void Given()
        {
            TestDatabase.EnsureCleanDatabaseExists(TestDataMother.SourceDatabaseConnectionString.Valid, TestDataMother.SourceMsdbDatabaseConnectionString.Valid);

            GetMockFor<ISqlServerSource>()
                .Setup(_ => _.SourceTables)
                .Returns(TestDataMother.SourceTables.OneValidTable);

            GetMockFor<ISqlServerSource>()
                .Setup(_ => _.SourceDatabaseConnectionString.Value)
                .Returns(TestDataMother.SourceDatabaseConnectionString.Valid);

            GetMockFor<ISqlServerSource>()
                .Setup(_ => _.ChangeTrackingRetentionInitialValueInDays)
                .Returns(TestDataMother.SourceDatabaseChangeTrackingRetentionInDays.Valid);
        }

        protected override void When()
        {
            SUT.Process();
        }

        [Test]
        public void then_no_changes_are_detected_after_some_time()
        {
            for (var i = 0; i < 3; i++)
            {
                GetMockFor<IDestinationOfData>().Verify(_ => _.PossibleChangesFound(It.IsAny<SourceTableConfiguration>(), It.IsAny<List<TableRowData>>()), Times.Never());
                Thread.Sleep(1000);
            }
            
        }
    }
}