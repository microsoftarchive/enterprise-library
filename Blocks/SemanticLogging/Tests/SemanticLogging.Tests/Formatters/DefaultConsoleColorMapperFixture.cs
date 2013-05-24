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

using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Tests.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Tests.Formatters
{
    [TestClass]
    public class DefaultConsoleColorMapperFixture
    {
        [TestClass]
        public class given_default_colors : ContextBase
        {
            private IConsoleColorMapper Sut;
            private Dictionary<EventLevel, ConsoleColor?> Results;

            protected override void Given()
            {
                Sut = new DefaultConsoleColorMapper();
                Results = new Dictionary<EventLevel, ConsoleColor?>();
            }

            protected override void When()
            {
                foreach (EventLevel level in Enum.GetValues(typeof(EventLevel)))
                {
                    Results.Add(level, Sut.Map(level));
                }
            }

            [TestMethod]
            public void then_all_eventlevels_should_be_mapped()
            {
                Assert.AreEqual(DefaultConsoleColorMapper.LogAlways, Results[EventLevel.LogAlways]);
                Assert.AreEqual(DefaultConsoleColorMapper.Critical, Results[EventLevel.Critical]);
                Assert.AreEqual(DefaultConsoleColorMapper.Error, Results[EventLevel.Error]);
                Assert.AreEqual(DefaultConsoleColorMapper.Warning, Results[EventLevel.Warning]);
                Assert.AreEqual(DefaultConsoleColorMapper.Verbose, Results[EventLevel.Verbose]);
                Assert.AreEqual(DefaultConsoleColorMapper.Informational, Results[EventLevel.Informational]);
            }
        }
    }
}
