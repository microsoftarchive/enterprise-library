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

using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Filters.Tests
{
    [ConfigurationElementType(typeof(CustomLogFilterData))]
    public class MockCustomLogFilter : MockCustomProviderBase, ILogFilter
    {
        public MockCustomLogFilter(NameValueCollection attributes)
            : base(attributes)
        {
        }

        public bool Filter(LogEntry log)
        {
            return true;
        }

        public string Name
        {
            get { return string.Empty; }
        }
    }
}
