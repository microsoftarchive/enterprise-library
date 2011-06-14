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

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    /// <summary>
    /// This class is primarily a testing hook. Testing code that
    /// calls <see cref="DateTime.Now"/> is fraught with pain and often results in
    /// weird test failures. To avoid this, all the code in the
    /// caching block that needs the current <see cref="DateTime"/>
    /// calls <see cref="CachingTimeProvider.Now" /> instead. Tests
    /// can use <see cref="CachingTimeProvider.SetTimeProviderForTests"/> to
    /// change the definition of "now" for testing purposes.
    /// </summary>
    /// <remarks>Don't change the time provider in production code. That way lies madness.</remarks>
    public static class CachingTimeProvider
    {
        private static Func<DateTimeOffset> timeProvider = () => DateTimeOffset.Now;

        /// <summary>
        /// Returns the current date time as given by the current time provider func.
        /// </summary>
        public static DateTimeOffset Now { get { return timeProvider(); } }

        /// <summary>
        /// Change the current time provider.
        /// </summary>
        /// <param name="newTimeProvider">Method to call to return the current time.</param>
        public static void SetTimeProviderForTests(Func<DateTimeOffset> newTimeProvider)
        {
            timeProvider = newTimeProvider;
        }

        /// <summary>
        /// Resets the time provider to the default.
        /// </summary>
        public static void ResetTimeProvider()
        {
            timeProvider = () => DateTimeOffset.Now;
        }
    }
}
