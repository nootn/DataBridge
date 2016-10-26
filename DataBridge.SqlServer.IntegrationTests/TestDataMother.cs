using System.Collections.Generic;
using System.Configuration;
using System.Runtime.Remoting.Messaging;
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

        public static class SourceTables
        {
            public static ISqlServerSourceTableCollection NoTables
            {
                get
                {
                    var enumerator = new Mock<IEnumerator<ISqlServerSourceTable>>();
                    enumerator.Setup(_ => _.MoveNext()).Returns(false);

                    var collection = new Mock<ISqlServerSourceTableCollection>();
                    collection.Setup(_ => _.GetEnumerator()).Returns(enumerator.Object);

                    return collection.Object;
                }
            }
        }
    }
}