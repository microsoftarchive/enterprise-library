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

using System.Collections;
using System.Threading;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests
{
    public static class ThreadStressHelper
    {
        public static void ThreadStress(ThreadStart testMethod, int threadCount)
        {
            ArrayList threads = new ArrayList();
            for (int i = 0; i < threadCount; i++)
            {
                threads.Add(new Thread(testMethod));
            }

            foreach (Thread thread in threads)
            {
                thread.Start();
            }
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
        }
    }
}

