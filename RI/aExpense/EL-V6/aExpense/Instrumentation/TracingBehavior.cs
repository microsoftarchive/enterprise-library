// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using AExpense.Instrumentation;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace AExpense.Instrumentation
{
    /// <summary>
    /// Tracing behavior for showcasing a simple Virtual Method Interceptor scenario
    /// </summary>
    public class TracingBehavior : IInterceptionBehavior
    {
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            AExpenseEvents.Log.TracingBehaviorVirtualMethodIntercepted(input);

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