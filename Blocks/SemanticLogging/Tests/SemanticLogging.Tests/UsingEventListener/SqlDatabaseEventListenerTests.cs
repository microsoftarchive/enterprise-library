#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Semantic Logging Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System.Data.SqlClient;
using System.Diagnostics.Tracing;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Sinks;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Tests.TestObjects;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Tests.Properties;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Tests.EventListeners
{
    [TestClass]
    public class when_receiving_many_events_with_imperative_flush : LocalDatabaseContext
    {
        private const int NumberOfEntries = 10000;

        protected ObservableEventListener eventListener;
        protected SinkSubscription<SqlDatabaseSink> subscription;

        protected override string GetLocalDatabaseFileName()
        {
            return "sqldbtests";
        }

        protected override void Given()
        {
            base.Given();

            this.LocalDbConnection.ChangeDatabase(this.dbName);

            using (var cmd = new SqlCommand(Resources.CreateTracesTable, this.LocalDbConnection))
            {
                cmd.ExecuteNonQuery();
            }

            this.eventListener = new ObservableEventListener();
            this.subscription = this.eventListener.LogToSqlDatabase("test", this.GetSqlConnectionString(), bufferingCount: NumberOfEntries); //@"Data Source=.\sqlexpress;Initial Catalog=SemanticLoggingTests;Integrated Security=True"
            this.eventListener.EnableEvents(TestEventSource.Log, EventLevel.LogAlways);
        }

        protected override void OnCleanup()
        {
            this.eventListener.DisableEvents(TestEventSource.Log);
            this.eventListener.Dispose();
            base.OnCleanup();
        }

        [TestMethod]
        public void then_all_events_should_be_flushed()
        {
            for (int i = 0; i < NumberOfEntries; i++)
            {
                TestEventSource.Log.FastEvent(i);
            }

            this.subscription.Sink.FlushAsync().Wait();

            var count = 0;

            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Traces", this.LocalDbConnection))
            {
                count = (int)cmd.ExecuteScalar();
            }

            Assert.AreEqual<int>(NumberOfEntries, count);
        }
    }
}
