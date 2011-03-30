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

using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
#if !SILVERLIGHT
using System.Diagnostics;
#else
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration settings that describe a <see cref="LogSource"/>.
    /// </summary>
    partial class TraceSourceData
    {
        ///<summary>
        /// Returns the type <see cref="TypeRegistration"/> entries describing the <see cref="TraceSource"/> represented
        /// by this configuration object.
        ///</summary>
        ///<returns>A set of registry entries.</returns>        
        public TypeRegistration GetRegistrations()
        {
            return
                new TypeRegistration<LogSource>(
                    () =>
                        new LogSource(
                            this.Name,
                            Container.ResolvedEnumerable<TraceListener>(this.TraceListeners.Select(tl => tl.Name)),
                            this.DefaultLevel,
                            this.AutoFlush,
                            Container.Resolved<ILoggingInstrumentationProvider>()))
                {
                    Name = this.Name,
                    Lifetime = TypeRegistrationLifetime.Transient
                };
        }
    }
}
