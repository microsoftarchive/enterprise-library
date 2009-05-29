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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel
{
    /// <summary>
    /// A interface describing objects that raise events when a
    /// container's type registrations need to updated due to
    /// a configuration source change.
    /// </summary>
    public interface IContainerReconfiguringEventSource
    {
        /// <summary>
        /// The event raised when a container must be reconfigured.
        /// </summary>
        event EventHandler<ContainerReconfiguringEventArgs> ContainerReconfiguring;
    }
}
