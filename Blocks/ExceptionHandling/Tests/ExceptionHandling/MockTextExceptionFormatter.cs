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
using System.IO;
using System.Reflection;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{

    public class MockTextExceptionFormatter : TextExceptionFormatter
    {
        public NameValueCollection properties = new NameValueCollection();
        public NameValueCollection fields = new NameValueCollection();

        public MockTextExceptionFormatter(TextWriter writer, Exception exception, Guid handlingInstanceId)
            : base(writer, exception, handlingInstanceId)
        {
        }

        protected override void WritePropertyInfo(PropertyInfo propertyInfo, object propertyValue)
        {
            if (propertyValue != null)
            {
                properties[propertyInfo.Name] = propertyValue.ToString();
            }
        }

        protected override void WriteFieldInfo(FieldInfo field, object fieldValue)
        {
            if (fieldValue != null)
            {
                fields[field.Name] = fieldValue.ToString();
            }
        }
    }
}

