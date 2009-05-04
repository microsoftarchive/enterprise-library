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
using System.Linq.Expressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport.TraceListeners
{
    [Assembler(typeof(MockTraceListenerAssembler))]
    public class MockTraceListenerData : TraceListenerData
    {
        public MockTraceListenerData()
        {
        }

        public MockTraceListenerData(string name)
            : base(name, typeof(MockTraceListener), TraceOptions.None, SourceLevels.All)
        {
        }

        protected override System.Linq.Expressions.Expression<System.Func<TraceListener>> GetCreationExpression()
        {
            return () => new MockTraceListener(this.Name);
        }
    }

    public class MockTraceListenerAssembler : IAssembler<TraceListener, TraceListenerData>
    {
        public TraceListener Assemble(IBuilderContext context,
                                      TraceListenerData objectConfiguration,
                                      IConfigurationSource configurationSource,
                                      ConfigurationReflectionCache reflectionCache)
        {
            return new MockTraceListener();
        }
    }
}
