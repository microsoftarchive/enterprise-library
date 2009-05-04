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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    public class MockThrowingExceptionHandler : IExceptionHandler
    {
		private const string handlerFailed = "Handler Failed";

        public MockThrowingExceptionHandler()
        {
        }

		public MockThrowingExceptionHandler(CustomHandlerData customHandlerData)
			: this()
		{		
		}

        public Exception HandleException(Exception exception, Guid correlationID)
        {
            throw new NotImplementedException(handlerFailed);
        }
    }
}

