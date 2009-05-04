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
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters.Tests
{
    [ConfigurationElementType(typeof(CustomFormatterData))]
    public class MockCustomLogFormatter
        : MockCustomProviderBase, ILogFormatter
    {
        public MockCustomLogFormatter(NameValueCollection attributes)
            : base(attributes)
        {
        }

        public string Format(LogEntry log)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
