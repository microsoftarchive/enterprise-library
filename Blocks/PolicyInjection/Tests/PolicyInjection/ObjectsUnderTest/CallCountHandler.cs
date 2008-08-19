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

using System;
using System.Runtime.Remoting.Messaging;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using System.Collections.Specialized;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.ObjectsUnderTest
{
    [ConfigurationElementType(typeof(CustomCallHandlerData))]
    public class CallCountHandler : ICallHandler
    {
        private int callCount;
        private int order = 0;

        public CallCountHandler()
        {
        }

        public CallCountHandler(NameValueCollection attributes)
        {
        }

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
            ++callCount;
            return getNext()(input, getNext);
        }
       
        public int CallCount
        {
            get { return callCount; }
        }
    }
}
