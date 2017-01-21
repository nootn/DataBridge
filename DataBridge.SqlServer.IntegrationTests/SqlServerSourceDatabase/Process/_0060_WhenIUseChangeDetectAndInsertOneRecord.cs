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
    public class _0060_WhenIUseChangeDetectAndInsertOneRecord : SpecsFor<SqlServer.SqlServerSourceDatabase>
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
            TestDatabase.InsertOneRowIntoTableOne(TestDataMother.SourceDatabaseConnectionString.Valid);
        }

        [Test]
        public void then_one_change_is_detected_after_some_time()
        {
            var mockDest = GetMockFor<IDestinationOfData>();
            
            var numTries = 5;
            for (var i = 1; i <= numTries; i++)
            {
                try
                {
                    mockDest.Verify(
                        _ =>
                            _.PossibleChangesFound(It.IsAny<SourceTableConfiguration>(),
                                It.IsAny<List<TableRowData>>()), Times.Once);
                }
                catch (MockException ex)
                {
                    if (i >= numTries)
                    {
                        throw;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
                
            }
        }
    }
}