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
        /// <see cref="GetRegistrations"/>.
        /// </summary>
        /// <returns>A <see cref="Expression"/> that creates an <see cref="NotificationTraceListener"/></returns>
        protected override Expression<Func<Diagnostics.TraceListener>> GetCreationExpression()
        {
            return () => new NotificationTraceListener(Container.Resolved<ITraceDispatcher>());
        }
    }
}
