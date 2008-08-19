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
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers
{
    /// <summary>
    /// Attribute to apply the <see cref="CachingCallHandler"/> directly to a class, property, or method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method)]
    public class CachingCallHandlerAttribute : HandlerAttribute
    {
        private TimeSpan expirationTime;

        /// <summary>
        /// Creates a <see cref="CachingCallHandlerAttribute"/> using the default expiration time of 5 minutes.
        /// </summary>
        public CachingCallHandlerAttribute()
        {
            expirationTime = CachingCallHandler.DefaultExpirationTime;
        }

        /// <summary>
        /// Creates a <see cref="CachingCallHandlerAttribute"/> using the given expiration time.
        /// </summary>
        /// <param name="hours">Hours until expiration.</param>
        /// <param name="minutes">Minutes until expiration.</param>
        /// <param name="seconds">Seconds until expiration.</param>
        public CachingCallHandlerAttribute(int hours, int minutes, int seconds)
        {
            expirationTime = new TimeSpan(hours, minutes, seconds);
        }

        /// <summary>
        /// Derived classes implement this method. When called, it
        /// creates a new call handler as specified in the attribute
        /// configuration.
        /// </summary>
        /// <returns>A new call handler object.</returns>
        public override ICallHandler CreateHandler()
        {
            return new CachingCallHandler(expirationTime, Order);
        }
    }
}
