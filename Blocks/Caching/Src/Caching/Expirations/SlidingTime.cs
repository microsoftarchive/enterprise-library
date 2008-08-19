//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Caching.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Expirations
{
    /// <summary>
    ///	This provider tests if a item was expired using a time slice schema.
    /// </summary>
    [Serializable]    
    public class SlidingTime : ICacheItemExpiration
    {
        private DateTime timeLastUsed;
        private TimeSpan itemSlidingExpiration;

        /// <summary>
        ///	Create an instance of this class with the timespan for expiration.
        /// </summary>
        /// <param name="slidingExpiration">
        ///	Expiration time span
        /// </param>
        public SlidingTime(TimeSpan slidingExpiration)
        {
            // Check that expiration is a valid numeric value
            if (!(slidingExpiration.TotalSeconds >= 1))
            {
                throw new ArgumentOutOfRangeException("slidingExpiration",
                                                      Resources.ExceptionRangeSlidingExpiration);
            }

            this.itemSlidingExpiration = slidingExpiration;
        }


        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="slidingExpiration"/>
        /// <param name="originalTimeStamp"/>
		/// <remarks>
		/// This constructor is for testing purposes only. Never, ever call it in a real program
		/// </remarks>
        public SlidingTime(TimeSpan slidingExpiration, DateTime originalTimeStamp) : this(slidingExpiration)
        {
            timeLastUsed = originalTimeStamp;
        }        

        /// <summary>
        /// Returns sliding time window that must be exceeded for expiration to occur
        /// </summary>
        public TimeSpan ItemSlidingExpiration
        {
            get { return itemSlidingExpiration; }
        }

        /// <summary>
        /// Returns time that this object was last touched
        /// </summary>
        public DateTime TimeLastUsed
        {
            get { return timeLastUsed; }
        }
		
		/// <summary>
        ///	Specifies if item has expired or not.
        /// </summary>
        /// <returns>Returns true if the item has expired otherwise false.</returns>
        public bool HasExpired()
        {
            bool expired = CheckSlidingExpiration(DateTime.Now,
                                                  this.timeLastUsed,
                                                  this.itemSlidingExpiration);
            return expired;
        }

        /// <summary>
        ///	Notifies that the item was recently used.
        /// </summary>
        public void Notify()
        {
            this.timeLastUsed = DateTime.Now;
        }

        /// <summary>
        /// Used to set the initial value of TimeLastUsed. This method is invoked during the reinstantiation of
        /// an instance from a persistent store. 
        /// </summary>
        /// <param name="owningCacheItem">CacheItem to which this expiration belongs.</param>
        public void Initialize(CacheItem owningCacheItem)
        {
            timeLastUsed = owningCacheItem.LastAccessedTime;
        }

        /// <summary>
        ///	Check whether the sliding time has expired.
        /// </summary>
        /// <param name="nowDateTime">Current time </param>
        /// <param name="lastUsed">The last time when the item has been used</param>
        /// <param name="slidingExpiration">The span of sliding expiration</param>
        /// <returns>True if the item was expired, otherwise false</returns>
        private static bool CheckSlidingExpiration(DateTime nowDateTime,
                                                   DateTime lastUsed,
                                                   TimeSpan slidingExpiration)
        {
            // Convert to UTC in order to compensate for time zones
            DateTime tmpNowDateTime = nowDateTime.ToUniversalTime();

            // Convert to UTC in order to compensate for time zones
            DateTime tmpLastUsed = lastUsed.ToUniversalTime();

            long expirationTicks = tmpLastUsed.Ticks + slidingExpiration.Ticks;

            bool expired = (tmpNowDateTime.Ticks >= expirationTicks) ? true : false;

            return expired;
        }
    }
}