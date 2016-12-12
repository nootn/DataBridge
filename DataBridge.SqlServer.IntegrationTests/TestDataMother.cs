using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using DataBridge.Core;
using DataBridge.SqlServer.Interface;
using Moq;

namespace DataBridge.SqlServer.IntegrationTests
{
    public static class TestDataMother
    {
        public static class SourceDatabaseConnectionString
        {
            public static string Valid => ConfigurationManager.AppSettings["SourceDatabaseConnectionStringValid"];

            public static string InvalidEmpty => "";

            public static string InvalidNotInCorrectFormat => "asdasdasdasdasd";

            public static string InvalidDoesNotExist
                => ConfigurationManager.AppSettings["SourceDatabaseConnectionStringInvalidNotExists"];
        }

        public static class SourceMsdbDatabaseConnectionString
        {
            public static string Valid => ConfigurationManager.AppSettings["SourceMsdbDatabaseConnectionStringValid"];
        }

        public static class SourceDatabaseChangeTrackingRetentionInDays
        {
            public static int Valid => 1;

            public static int InvalidZero => 0;
        }

        public static class SourceTables
        {
            public static ISqlServerSourceTableCollection NoTables
            {
                get
                {
                    var items = new List<ISqlServerSourceTable>();

                    var collection = new Mock<ISqlServerSourceTableCollection>();
                    collection.Setup(_ => _.GetEnumerator()).Returns(items.ToList().GetEnumerator());

                    return collection.Object;
                }
            }

            public static ISqlServerSourceTableCollection OneNonExistentTable
            {
                get
                {
                    var validIncludeCols =
                        GetMockBasicConfigElementCollection(new[] { "Id", "LastUpdatedOn", "Name", "DateOfBirth" });

                    var items = new List<ISqlServerSourceTable>();
                    var validTable = new Mock<ISqlServerSourceTable>();
                    validTable.Setup(_ => _.ChangeDetectionMode)
                        .Returns((int) TableSyncSettings.ChangeDetectionModes.AsSoonAsPossible);
                    validTable.Setup(_ => _.Id).Returns("dbo.TestDoesntExist");
                    validTable.Setup(_ => _.Name).Returns("TestDoesntExist");
                    validTable.Setup(_ => _.PollIntervalInMilliseconds).Returns(1000*30);
                    validTable.Setup(_ => _.QualityCheckIntervalInMilliseconds).Returns(1000*180);
                    validTable.Setup(_ => _.QualityCheckRecordBatchSize).Returns(1);
                    validTable.Setup(_ => _.Schema).Returns("dbo");
                    validTable.Setup(_ => _.PrimaryKeyColumn).Returns(validIncludeCols.Object.First().Name);
                    validTable.Setup(_ => _.LastUpdatedAtColumn).Returns(validIncludeCols.Object.Skip(1).First().Name);
                    validTable.Setup(_ => _.ColumnsToInclude).Returns(validIncludeCols.Object);
                    items.Add(validTable.Object);

                    var collection = new Mock<ISqlServerSourceTableCollection>();
                    collection.Setup(_ => _.GetEnumerator()).Returns(items.ToList().GetEnumerator());

                    return collection.Object;
                }
            }

            public static ISqlServerSourceTableCollection OneValidTable
            {
                get
                {
                    var validIncludeCols =
                        GetMockBasicConfigElementCollection(new[] {"Id", "LastUpdatedOn", "Name", "DateOfBirth"});

                    var items = new List<ISqlServerSourceTable>();
                    var validTable = new Mock<ISqlServerSourceTable>();
                    validTable.Setup(_ => _.ChangeDetectionMode)
                        .Returns((int) TableSyncSettings.ChangeDetectionModes.AsSoonAsPossible);
                    validTable.Setup(_ => _.Id).Returns("dbo.Test1");
                    validTable.Setup(_ => _.Name).Returns("Test1");
                    validTable.Setup(_ => _.PollIntervalInMilliseconds).Returns(1000*30);
                    validTable.Setup(_ => _.QualityCheckIntervalInMilliseconds).Returns(1000*180);
                    validTable.Setup(_ => _.QualityCheckRecordBatchSize).Returns(1);
                    validTable.Setup(_ => _.Schema).Returns("dbo");
                    validTable.Setup(_ => _.PrimaryKeyColumn).Returns(validIncludeCols.Object.First().Name);
                    validTable.Setup(_ => _.LastUpdatedAtColumn).Returns(validIncludeCols.Object.Skip(1).First().Name);
                    validTable.Setup(_ => _.ColumnsToInclude).Returns(validIncludeCols.Object);
                    items.Add(validTable.Object);

                    var collection = new Mock<ISqlServerSourceTableCollection>();
                    collection.Setup(_ => _.GetEnumerator()).Returns(items.ToList().GetEnumerator());

                    return collection.Object;
                }
            }

        }

        internal static Mock<IBasicConfigElementCollection> GetMockBasicConfigElementCollection(string[] nameValues)
        {
            var includeItems = new List<IBasicConfigElement>();
            foreach (var val in nameValues)
            {
                var valMock = new Mock<IBasicConfigElement>();
                valMock.Setup(_ => _.Name).Returns(val);
                includeItems.Add(valMock.Object);
            }
            var mockIncludeItems = new Mock<IBasicConfigElementCollection>();
            mockIncludeItems.Setup(_ => _.GetEnumerator()).Returns(includeItems.ToList().GetEnumerator());

            return mockIncludeItems;
        }
    }
}