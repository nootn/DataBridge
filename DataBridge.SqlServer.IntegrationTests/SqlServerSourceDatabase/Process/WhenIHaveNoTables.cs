﻿using System;
using DataBridge.Core;
using DataBridge.SqlServer.Interface;
using NUnit.Framework;
using Should;
using SpecsFor;

namespace DataBridge.SqlServer.IntegrationTests.SqlServerSourceDatabase.Process
{
    public class WhenIHaveNoTables : SpecsFor<SqlServer.SqlServerSourceDatabase>
    {
        private Exception _caughtException;

        protected override void Given()
        {
            GetMockFor<ISqlServerSource>()
                .Setup(_ => _.SourceDatabaseConnectionString.Value)
                .Returns(TestDataMother.SourceDatabaseConnectionString.Valid);

            GetMockFor<ISqlServerSource>()
                .Setup(_ => _.SourceTables)
                .Returns(TestDataMother.SourceTables.NoTables);
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
        public void then_the_correct_exception_is_thrown()
        {
            _caughtException.Message.ShouldStartWith(SourceDatabaseBase.ExceptionMessageNoTablesConfiguredPrefix);
        }
        
    }
}