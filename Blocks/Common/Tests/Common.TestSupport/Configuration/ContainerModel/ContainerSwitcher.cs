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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel
{
    /// <summary>
    /// A helper class that will swap out the global EnterpriseLibraryContainer
    /// for the duration of the using block.
    /// </summary>
    public class ContainerSwitcher : IDisposable
    {
        private readonly IServiceLocator originalContainer;
        private readonly bool shouldDisposeNewContainer;

        public ContainerSwitcher(IServiceLocator newContainer, bool shouldDisposeContainerWhenDone)
        {
            originalContainer = EnterpriseLibraryContainer.Current;
            shouldDisposeNewContainer = shouldDisposeContainerWhenDone;

            EnterpriseLibraryContainer.Current = newContainer;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            var newContainer = EnterpriseLibraryContainer.Current;
            EnterpriseLibraryContainer.Current = originalContainer;

            if(shouldDisposeNewContainer)
            {
                newContainer.Dispose();
            }
        }
    }
}
