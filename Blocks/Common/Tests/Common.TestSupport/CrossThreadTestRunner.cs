//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Reflection;
using System.Security.Permissions;
using System.Threading;

namespace Microsoft.Practices.EnterpriseLibrary.Common.TestSupport
{
    public class CrossThreadTestRunner
    {
        private ThreadStart userDelegate;
        private Exception lastException;

        public CrossThreadTestRunner(ThreadStart userDelegate)
        {
            this.userDelegate = userDelegate;
        }

        public void Run()
        {
            Thread t = new Thread(new ThreadStart(this.MultiThreadedWorker));

            t.Start();
            t.Join();

            if (lastException != null)
            {
                ThrowExceptionPreservingStack(lastException);
            }
        }

        [ReflectionPermission(SecurityAction.Demand)]
        private static void ThrowExceptionPreservingStack(Exception exception)
        {
            FieldInfo remoteStackTraceString = typeof(Exception).GetField("_remoteStackTraceString", BindingFlags.Instance | BindingFlags.NonPublic);
            remoteStackTraceString.SetValue(exception, exception.StackTrace + Environment.NewLine);
            throw exception;
        }

        private void MultiThreadedWorker()
        {
            try
            {
                userDelegate.Invoke();
            }
            catch (Exception e)
            {
                lastException = e;
            }
        }
    }
}

