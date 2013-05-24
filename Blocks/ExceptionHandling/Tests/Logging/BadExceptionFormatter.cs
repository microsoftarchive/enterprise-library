//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Specialized;
using System.Reflection;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Tests
{
    public class BadExceptionFormatter : ExceptionFormatter
    {
        public BadExceptionFormatter()
            : base(null, Guid.Empty)
        {
        }

        protected override void WriteDescription()
        {
        }

        protected override void WriteDateTime(DateTime utcNow)
        {
        }

        protected override void WriteExceptionType(Type exceptionType)
        {
        }

        protected override void WriteMessage(string message)
        {
        }

        protected override void WriteSource(string source)
        {
        }

        protected override void WriteHelpLink(string helpLink)
        {
        }

        protected override void WriteStackTrace(string stackTrace)
        {
        }

        protected override void WritePropertyInfo(PropertyInfo propertyInfo, object value)
        {
        }

        protected override void WriteFieldInfo(FieldInfo field, object value)
        {
        }

        protected override void WriteAdditionalInfo(NameValueCollection additionalInfo)
        {
        }
    }
}
