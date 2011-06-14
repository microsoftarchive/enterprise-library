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
using System.Linq.Expressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Configuration for the NotificationTraceListener.
    /// </summary>
    public class NotificationTraceListenerData : TraceListenerData
    {
        /// <summary>
        /// Gets the creation expression used to produce a <see cref="TypeRegistration"/> during
        /// <see cref="TraceListenerData.GetRegistrations"/>.
        /// </summary>
        /// <returns>A <see cref="Expression"/> that creates an <see cref="NotificationTraceListener"/></returns>
        protected override Expression<Func<Diagnostics.TraceListener>> GetCreationExpression()
        {
            return () => new NotificationTraceListener(Container.Resolved<ITraceDispatcher>());
        }
    }
}
