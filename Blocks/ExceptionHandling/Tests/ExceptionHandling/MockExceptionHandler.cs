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
#if !SILVERLIGHT
    [Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ConfigurationElementType(typeof(Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.CustomHandlerData))]
#endif
    public class MockExceptionHandler : IExceptionHandler
    {
        public static int handleExceptionCount = 0;
        public static string lastMessage;
        public static Guid handlingInstanceID;

        public int instanceHandledExceptionCount = 0;

#if !SILVERLIGHT
        public static System.Collections.Specialized.NameValueCollection attributes;

		public MockExceptionHandler(System.Collections.Specialized.NameValueCollection attributes)
		{
			MockExceptionHandler.attributes = attributes;
		}
#endif

        public MockExceptionHandler()
        { }


        public static void Clear()
        {
            handleExceptionCount = 0;
            lastMessage = String.Empty;
            handlingInstanceID = Guid.Empty;
#if !SILVERLIGHT
			attributes = null;
#endif
        }

        public static string FormatExceptionMessage(string message, Guid handlingInstanceID)
        {
            return ExceptionUtility.FormatExceptionMessage(message, handlingInstanceID);
        }

        public Exception HandleException(Exception ex, Guid handlingInstanceID)
        {
            instanceHandledExceptionCount++;
            handleExceptionCount++;

            lastMessage = ex.Message;
            MockExceptionHandler.handlingInstanceID = handlingInstanceID;
            return ex;
        }
    }
}

