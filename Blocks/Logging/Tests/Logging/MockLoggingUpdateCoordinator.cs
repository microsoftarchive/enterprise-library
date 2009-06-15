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
using System.Collections.Generic;
using System.Threading;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    public class MockLoggingUpdateCoordinator : ILoggingUpdateCoordinator
    {
      public void RaiseLoggingUpdate(IServiceLocator serviceLocator)
        {
            object context = AddedLoggingUpdateHandler.PrepareForUpdate(serviceLocator);
            AddedLoggingUpdateHandler.CommitUpdate(context);
        }

        public void RegisterLoggingUpdateHandler(ILoggingUpdateHandler loggingUpdateHandler)
        {
            AddedLoggingUpdateHandler = loggingUpdateHandler;
        }

        public void UnregisterLoggingUpdateHandler(ILoggingUpdateHandler loggingUpdateHandler)
        {
            RemovedLoggingUpdateHandler = loggingUpdateHandler;
        }

        public void ExecuteReadOperation(Action action)
        {
            action();
        }

        public void ExecuteWriteOperation(Action action)
        {
            action();
        }

        public ILoggingUpdateHandler AddedLoggingUpdateHandler;
        public ILoggingUpdateHandler RemovedLoggingUpdateHandler;
    }
}
