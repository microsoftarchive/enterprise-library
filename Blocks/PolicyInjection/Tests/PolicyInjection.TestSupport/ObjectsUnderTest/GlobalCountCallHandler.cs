//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.TestSupport.ObjectsUnderTest
{
    [ConfigurationElementType(typeof(CustomCallHandlerData))]
    public class GlobalCountCallHandler : ICallHandler
    {
        public static Dictionary<string, int> Calls = new Dictionary<string, int>();
        private CallCounter counter;
        public readonly string callHandlerName;
        private int order = 0;

        public GlobalCountCallHandler(string name)
            : this(name, new CallCounter())
        { }

        public GlobalCountCallHandler(string name, CallCounter counter)
        {
            callHandlerName = name;
            this.counter = counter;
        }

        public GlobalCountCallHandler(NameValueCollection attributes)
            : this(attributes["callhandler"])
        { }

        #region ICallHandler Members
        /// <summary>
        /// Gets or sets the order in which the handler will be executed
        /// </summary>
        public int Order
        {
            get
            {
                return order;
            }
            set
            {
                order = value;
            }
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            if (!Calls.ContainsKey(callHandlerName))
            {
                Calls.Add(callHandlerName, 0);
            }
            Calls[callHandlerName]++;

            this.counter.CountCall(this.callHandlerName);

            return getNext().Invoke(input, getNext);
        }

        #endregion
    }

    public class GlobalCountCallHandlerAttribute : HandlerAttribute
    {
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return new GlobalCountCallHandler(this.Name, container.Resolve<CallCounter>());
        }

        public string Name { get; set; }
    }

    public class CallCounter
    {
        public IDictionary<string, int> Calls = new Dictionary<string, int>();

        internal void CountCall(string name)
        {
            if (!Calls.ContainsKey(name))
            {
                Calls.Add(name, 0);
            }
            Calls[name]++;
        }
    }
}
