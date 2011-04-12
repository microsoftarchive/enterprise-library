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

using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// A class with the data for the LogCallHandler.
    /// </summary>
    public partial class LogCallHandlerData : CallHandlerData
    {
        /// <summary>
        /// Get the set of <see cref="TypeRegistration"/> objects needed to
        /// register the call handler represented by this config element and its associated objects.
        /// </summary>
        /// <param name="nameSuffix">A suffix for the names in the generated type registration objects.</param>
        /// <returns>The set of <see cref="TypeRegistration"/> objects.</returns>
        public override IEnumerable<TypeRegistration> GetRegistrations(string nameSuffix)
        {
            var logBeforeCall = false;
            var logAfterCall = false;
            switch (this.LogBehavior)
            {
                case HandlerLogBehavior.Before:
                    logBeforeCall = true;
                    break;

                case HandlerLogBehavior.After:
                    logAfterCall = true;
                    break;

                case HandlerLogBehavior.BeforeAndAfter:
                    logBeforeCall = true;
                    logAfterCall = true;
                    break;
            }
            var categories = new List<string>(this.Categories.Select(cat => cat.Name));

            yield return
                new TypeRegistration<ICallHandler>(() =>
                                                   new LogCallHandler(Container.Resolved<LogWriter>())
                                                       {
                                                           Order = this.Order,
                                                           LogBeforeCall = logBeforeCall,
                                                           LogAfterCall = logAfterCall,
                                                           BeforeMessage = this.BeforeMessage,
                                                           AfterMessage = this.AfterMessage,
                                                           EventId = this.EventId,
                                                           IncludeCallStack = this.IncludeCallStack,
                                                           IncludeCallTime = this.IncludeCallTime,
                                                           IncludeParameters = this.IncludeParameterValues,
                                                           Priority = this.Priority,
                                                           Severity = this.Severity,
                                                           Categories = categories
                                                       })
                    {
                        Name = this.Name + nameSuffix,
                        Lifetime = TypeRegistrationLifetime.Transient
                    };
        }
    }

    /// <summary>
    /// This enum control when the logging call handler will add log entries.
    /// </summary>
    public enum HandlerLogBehavior
    {
        /// <summary>
        /// Log both before and after the call.
        /// </summary>
        BeforeAndAfter = 0,
        /// <summary>
        /// Log only before the call.
        /// </summary>
        Before = 1,
        /// <summary>
        /// Log only after the call.
        /// </summary>
        After = 2
    }
}
