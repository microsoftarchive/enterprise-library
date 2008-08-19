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

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
	/// <summary>
    /// Provides data for the <see cref="CachingInstrumentationProvider.cacheCallbackFailed"/> event.
	/// </summary>
	public class CacheCallbackFailureEventArgs : EventArgs
	{
		private string key;
		private Exception exception;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheCallbackFailureEventArgs"/> class.
        /// </summary>
        /// <param name="key">The key that was used accessing the <see cref="CacheManager"/> when this failure ocurred.</param>
        /// <param name="exception">The exception causing the failure.</param>
        public CacheCallbackFailureEventArgs(string key, Exception exception)
		{
			this.key = key;
			this.exception = exception;
		}

		/// <summary>
        /// Gets the key that was used accessing the <see cref="CacheManager"/> when this failure ocurred.
		/// </summary>
		public string Key
		{
			get { return key; }
		}

		/// <summary>
        /// Gets the exception causing the failure.
		/// </summary>
		public Exception Exception
		{
			get { return exception; }
		}
	}
}