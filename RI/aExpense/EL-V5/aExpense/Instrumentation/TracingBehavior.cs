#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// aExpense Reference Implementation
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace AExpense.Instrumentation
{
    /// <summary>
    /// Tracing behavior for showcasing a simple Virtual Method Interceptor scenario that traces method/property accessors.
    /// </summary>
    public class TracingBehavior : IInterceptionBehavior
    {
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            if (Logger.IsLoggingEnabled())
            {
                // Only log methods with arguments (bypass getters)
                if (input.Arguments.Count > 0)
                {
                    string arguments = string.Join(",", input.Arguments.OfType<object>());
                    Logger.Write(string.Format(CultureInfo.InvariantCulture, "{0}: Method {1}.{2} executed. Arguments: {3}",
                        DateTime.Now,
                        input.MethodBase.DeclaringType.Name,
                        input.MethodBase.Name,
                        arguments), Constants.ExpenseTracingCategory, Constants.TracingBehaviorPriority);
                }
            }

            return getNext()(input, getNext);
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Enumerable.Empty<Type>();
        }

        public bool WillExecute
        {
            get { return true; }
        }
    }
}