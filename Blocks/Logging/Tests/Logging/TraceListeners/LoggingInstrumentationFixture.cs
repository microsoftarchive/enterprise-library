//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners
{
    public class GivenALoggingStackResolvedFromAContainer : ArrangeActAssert
    {
        private IUnityContainer container;
        private TestLoggingInstrumentationProvider instrumentationProvider;
        protected LogWriter logWriter;

        protected override void Arrange()
        {
            var settings =
                new LoggingSettings
                {
                    TraceSources =
                    {
                        new TraceSourceData("category1", SourceLevels.All)
                        {
                            TraceListeners = 
                            {
                                new TraceListenerReferenceData("file")
                            }
                        },
                        new TraceSourceData("category2", SourceLevels.All)
                        {
                            TraceListeners = 
                            {
                                new TraceListenerReferenceData("file")
                            }
                        },
                        new TraceSourceData("category3", SourceLevels.All)
                        {
                            TraceListeners = 
                            {
                                new TraceListenerReferenceData("file2")
                            }
                        }
                    },
                    TraceListeners =
                    {
                        new FlatFileTraceListenerData("file", "trace.log", null),
                        new FlatFileTraceListenerData("file2", "trace.log", null)
                    }
                };

            var configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(LoggingSettings.SectionName, settings);

            this.container =
                EnterpriseLibraryContainer.CreateDefaultContainer(configurationSource).GetInstance<IUnityContainer>();
            this.instrumentationProvider = new TestLoggingInstrumentationProvider();
            this.container.RegisterInstance<ILoggingInstrumentationProvider>(this.instrumentationProvider);
            this.logWriter = this.container.Resolve<LogWriter>();
        }

        protected override void Teardown()
        {
            this.container.Dispose();
        }

        [TestClass]
        public class WhenLoggingToASingleCategory : GivenALoggingStackResolvedFromAContainer
        {
            protected override void Act()
            {
                this.logWriter.Write("message", "category1");
            }

            [TestMethod]
            public void ThenFiresTraceListenerEntryEventOnce()
            {
                Assert.AreEqual(1, this.instrumentationProvider.TraceListenerEntriesWritten);
            }
        }

        [TestClass]
        public class WhenLoggingToMultipleCategoriesWithSharedListener : GivenALoggingStackResolvedFromAContainer
        {
            protected override void Act()
            {
                this.logWriter.Write("message", new[] { "category1", "category2" });
            }

            [TestMethod]
            public void ThenFiresTraceListenerEntryEventOnce()
            {
                Assert.AreEqual(1, this.instrumentationProvider.TraceListenerEntriesWritten);
            }
        }

        [TestClass]
        public class WhenLoggingToMultipleCategoriesWithDifferentListeners : GivenALoggingStackResolvedFromAContainer
        {
            protected override void Act()
            {
                this.logWriter.Write("message", new[] { "category1", "category3" });
            }

            [TestMethod]
            public void ThenFiresTraceListenerEntryEventTwice()
            {
                Assert.AreEqual(2, this.instrumentationProvider.TraceListenerEntriesWritten);
            }
        }

        protected class TestLoggingInstrumentationProvider : ILoggingInstrumentationProvider
        {
            public int TraceListenerEntriesWritten { get; private set; }

            public void FireLockAcquisitionError(string message)
            {
                throw new NotImplementedException();
            }

            public void FireConfigurationFailureEvent(Exception configurationException)
            {
                throw new NotImplementedException();
            }

            public void FireFailureLoggingErrorEvent(string message, Exception exception)
            {
                throw new NotImplementedException();
            }

            public void FireLogEventRaised()
            {
            }

            public void FireTraceListenerEntryWrittenEvent()
            {
                TraceListenerEntriesWritten++;
            }

            public void FireReconfigurationErrorEvent(Exception exception)
            {
                throw new NotImplementedException();
            }
        }

    }
}
