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
using System.Management.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport
{
    [Serializable]
    public class CustomLogEntry : LogEntry
    {
        public CustomLogEntry()
            : base()
        {
        }

        public string AcmeCoField1 = string.Empty;
        public string AcmeCoField2 = string.Empty;
        public string AcmeCoField3 = string.Empty;

        private string propertyValue = "myPropertyValue";

        public string MyProperty
        {
            get { return propertyValue; }
            set { propertyValue = value; }
        }

        public string MyPropertyThatReturnsNull
        {
            get { return null; }
            set { }
        }

        [IgnoreMember]
        public string PropertyNotReadable
        {
            set { }
        }

        [IgnoreMember]
        public string this[int index]
        {
            get { return null; }
            set { }
        }

        public string MyPropertyThatThrowsException
        {
            get { throw new Exception(); }
            set { }
        }
    }
}
