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
    /// An implementation of <see cref="IContainerReconfiguringEventSource"/> that does
    /// nothing. Saves null checking everywhere.
    /// </summary>
    public class NullContainerReconfiguringEventSource : IContainerReconfiguringEventSource
    {
        #region IContainerReconfiguringEventSource Members

        /// <summary>
        /// The event raised when the configuration source changes.
        /// </summary>
        /// <remarks>With this implementation the event is never raised.</remarks>
#pragma warning disable 67 // Event is never used
        public event EventHandler<ContainerReconfiguringEventArgs> ContainerReconfiguring;
#pragma warning restore 67
        #endregion
    }
}
