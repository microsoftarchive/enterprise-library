// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

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