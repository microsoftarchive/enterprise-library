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
    public class MockThrowingExceptionHandler : IExceptionHandler
    {
		private const string handlerFailed = "Handler Failed";

        public MockThrowingExceptionHandler()
        {
        }

#if !SILVERLIGHT
		public MockThrowingExceptionHandler(Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.CustomHandlerData customHandlerData)
			: this()
		{		
		}
#endif

        public Exception HandleException(Exception exception, Guid correlationID)
        {
            throw new NotImplementedException(handlerFailed);
        }
    }
}

