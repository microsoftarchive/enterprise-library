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

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    /// <summary>
    /// Summary description for MockException.
    /// </summary>
    public class MockException : ArgumentNullException
    {
        public readonly string FieldString = "MockFieldString";
		private string setOnlyString;
		private const string setOnlyStringValue = "SetOnlyString";
		private const string mockException = "MOCK EXCEPTION";
		private const string mockPropertyString = "MockPropertyString";

        public MockException() : base(mockException)
        {
			setOnlyString = string.Empty;
        }

		public MockException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

        public string PropertyString
        {
            get { return mockPropertyString; }
        }

		public string SetOnlyString
		{
			set { setOnlyString = setOnlyStringValue; }
		}

		
    }
}

