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
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Coordinates logging operations with updates to the logging stack.
    /// </summary>
    /// <remarks>
    /// Use and modification of logging objects must be performed through the <see cref="ILoggingUpdateCoordinator"/>.
    /// </remarks>
    public interface ILoggingUpdateCoordinator
    {
        ///<summary>
        /// Registers a logging update handler for responding to updated events.
        ///</summary>
        ///<param name="loggingUpdateHandler"></param>
        void RegisterLoggingUpdateHandler(ILoggingUpdateHandler loggingUpdateHandler);

        ///<summary>
        /// Unregisters a logging update handler for responding to updated events.
        ///</summary>
        ///<param name="loggingUpdateHandler"></param>
        void UnregisterLoggingUpdateHandler(ILoggingUpdateHandler loggingUpdateHandler);

        /// <summary>
        /// Executes the supplied <see cref="Action"/> when no updates are being performed.
        /// </summary>
        /// <remarks>No updates to the logging objects should be performed by the supplied action.</remarks>
        /// <param name="action">The <see cref="Action"/> to execute.</param>
        void ExecuteReadOperation(Action action);

        /// <summary>
        /// Executes the supplied <see cref="Action"/> in isolation.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> to execute.</param>
        void ExecuteWriteOperation(Action action);
    }

    /// <summary>
    /// Contract for objects registered for notifications from a <see cref="ILoggingUpdateCoordinator"/>.
    /// </summary>
    public interface ILoggingUpdateHandler
    {
        /// <summary>
        /// Prepares to update it's internal state, but does not commit this until <see cref="CommitUpdate"/>
        /// </summary>
        /// <returns>
        /// A new version of the internal state.
        /// </returns>
        object PrepareForUpdate(IServiceLocator serviceLocator);

        /// <summary>
        /// Commits the update of internal state.
        /// </summary>
        /// <param name="context">
        /// The new internal state, as returned by the <see cref="PrepareForUpdate"/> method.
        /// </param>
        void CommitUpdate(object context);
    }
}
