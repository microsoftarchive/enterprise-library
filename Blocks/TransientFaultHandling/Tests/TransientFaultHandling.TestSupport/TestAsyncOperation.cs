#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Transient Fault Handling Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.TestSupport
{
    using System;
    using System.Threading;

    public class TestAsyncOperation
    {
        public TestAsyncOperation(Exception exceptionToThrow)
        {
            this.ExceptionToThrow = exceptionToThrow;
        }

        public int BeginMethodCount { get; private set; }
        public int EndMethodCount { get; private set; }
        public Exception ExceptionToThrow { get; set; }

        public IAsyncResult BeginMethod(AsyncCallback callback, object state)
        {
            this.BeginMethodCount++;
            var asyncResult = new TestAsyncResult();
            ThreadPool.QueueUserWorkItem(_ => callback(asyncResult), null);
            return asyncResult;
        }

        public bool EndMethod(IAsyncResult asyncResult)
        {
            this.EndMethodCount++;

            if (this.ExceptionToThrow != null)
            {
                throw this.ExceptionToThrow;
            }

            return true;
        }
    }

    public class TestAsyncResult : IAsyncResult
    {
        public bool IsCompleted { get; set; }

        public WaitHandle AsyncWaitHandle { get; set; }

        public object AsyncState { get; set; }

        public bool CompletedSynchronously { get; set; }
    }
}
