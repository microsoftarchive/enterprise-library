//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    internal class StartScavengingMsg : IQueueMessage
    {
        private BackgroundScheduler callback;

        public StartScavengingMsg(BackgroundScheduler callback)
        {
            this.callback = callback;
        }

        public void Run()
        {
            callback.DoStartScavenging();
        }
    }
}