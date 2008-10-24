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
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers
{
    /// <summary>
    /// An <see cref="ICallHandler"/> that implements caching of the return values of
    /// methods. This handler stores the return value in the ASP.NET cache.
    /// </summary>
    [ConfigurationElementType(typeof(CachingCallHandlerData))]
    public class CachingCallHandler : ICallHandler
    {
        /// <summary>
        /// The default expiration time for the cached entries: 5 minutes
        /// </summary>
        public static readonly TimeSpan DefaultExpirationTime = new TimeSpan(0, 5, 0);

        private int order = 0;
        private ICacheKeyGenerator keyGenerator;
        private TimeSpan expirationTime;

        /// <summary>
        /// Creates a <see cref="CachingCallHandler"/> that uses the default expiration time of 5 minutes.
        /// </summary>
        public CachingCallHandler()
            : this(DefaultExpirationTime)
        {
        }

        /// <summary>
        /// Creates a <see cref="CachingCallHandler"/> that uses the given expiration time.
        /// </summary>
        /// <param name="expirationTime">Length of time the cached data goes unused before it is eligible for
        /// reclamation.</param>
        public CachingCallHandler(TimeSpan expirationTime)
        {
            keyGenerator = new DefaultCacheKeyGenerator();
            this.expirationTime = expirationTime;
        }

        /// <summary>
        /// Creates a <see cref="CachingCallHandler"/> that uses the given expiration time.
        /// </summary>
        /// <param name="expirationTime">Length of time the cached data goes unused before it is eligible for
        ///reclamation.</param>
        /// <param name="order">Order in which handler will be executed.</param>
        public CachingCallHandler(TimeSpan expirationTime, int order)
        {
            keyGenerator = new DefaultCacheKeyGenerator();
            this.expirationTime = expirationTime;
            this.order = order;
        }

        /// <summary>
        /// Gets or sets the expiration time for cache data.
        /// </summary>
        /// <value>The expiration time.</value>
        public TimeSpan ExpirationTime
        {
            get { return expirationTime; }
            set { expirationTime = value; }
        }

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

        /// <summary>
        /// Implements the caching behavior of this handler.
        /// </summary>
        /// <param name="input"><see cref="IMethodInvocation"/> object describing the current call.</param>
        /// <param name="getNext">delegate used to get the next handler in the current pipeline.</param>
        /// <returns>Return value from target method, or cached result if previous inputs have been seen.</returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            if (TargetMethodReturnsVoid(input))
            {
                return getNext()(input, getNext);
            }

            object[] inputs = new object[input.Inputs.Count];
            for (int i = 0; i < inputs.Length; ++i)
            {
                inputs[i] = input.Inputs[i];
            }

            string cacheKey = keyGenerator.CreateCacheKey(input.MethodBase, inputs);

            object[] cachedResult = (object[])HttpRuntime.Cache.Get(cacheKey);

            if (cachedResult == null)
            {
                IMethodReturn realReturn = getNext()(input, getNext);
                if (realReturn.Exception == null)
                {
                    AddToCache(cacheKey, realReturn.ReturnValue);
                }
                return realReturn;
            }

            IMethodReturn cachedReturn = input.CreateMethodReturn(cachedResult[0], input.Arguments);
            return cachedReturn;
        }

        private bool TargetMethodReturnsVoid(IMethodInvocation input)
        {
            MethodInfo targetMethod = input.MethodBase as MethodInfo;
            return targetMethod != null && targetMethod.ReturnType == typeof(void);
        }

        #endregion

        private void AddToCache(string key, object value)
        {
            object[] cacheValue = new object[] { value };
            HttpRuntime.Cache.Insert(
                key,
                cacheValue,
                null,
                Cache.NoAbsoluteExpiration,
                expirationTime,
                CacheItemPriority.Normal, null);
        }
    }

    /// <summary>
    /// This interface describes classes that can be used to generate cache key strings
    /// for the <see cref="CachingCallHandler"/>.
    /// </summary>
    public interface ICacheKeyGenerator
    {
        /// <summary>
        /// Creates a cache key for the given method and set of input arguments.
        /// </summary>
        /// <param name="method">Method being called.</param>
        /// <param name="inputs">Input arguments.</param>
        /// <returns>A (hopefully) unique string to be used as a cache key.</returns>
        string CreateCacheKey(MethodBase method, object[] inputs);
    }

    /// <summary>
    /// The default <see cref="ICacheKeyGenerator"/> used by the <see cref="CachingCallHandler"/>.
    /// </summary>
    public class DefaultCacheKeyGenerator : ICacheKeyGenerator
    {
        private readonly Guid KeyGuid = new Guid("ECFD1B0F-0CBA-4AA1-89A0-179B636381CA");

        #region ICacheKeyGenerator Members

        /// <summary>
        /// Create a cache key for the given method and set of input arguments.
        /// </summary>
        /// <param name="method">Method being called.</param>
        /// <param name="inputs">Input arguments.</param>
        /// <returns>A (hopefully) unique string to be used as a cache key.</returns>
        public string CreateCacheKey(MethodBase method, params object[] inputs)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}:", KeyGuid);
            if (method.DeclaringType != null)
            {
                sb.Append(method.DeclaringType.FullName);
            }
            sb.Append(':');
            sb.Append(method.Name);

            if (inputs != null)
            {
                foreach (object input in inputs)
                {
                    sb.Append(':');
                    if (input != null)
                    {
                        sb.Append(input.GetHashCode().ToString());
                    }
                }
            }

            return sb.ToString();
        }

        #endregion
    }
}
