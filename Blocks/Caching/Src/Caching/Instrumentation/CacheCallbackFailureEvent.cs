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

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
    /// <summary>
    /// Represents the WMI event fired when a configured callback could not be executed.
    /// </summary>
	public class CacheCallbackFailureEvent : CacheEvent
	{
		private string key;
		private string exceptionMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheCallbackFailureEvent"/> class.
        /// </summary>
        /// <param name="instanceName">Name of the <see cref="CacheManager"/> instance the failure ocurred in.</param>
        /// <param name="key">The key which was used to access the cache when the failure ocurred.</param>
        /// <param name="exceptionMessage">The message that represents the exception thrown when the configured callback could not be executed.</param>
       public CacheCallbackFailureEvent(string instanceName, string key, string exceptionMessage)
			: base(instanceName)
		{
			this.key = key;
			this.exceptionMessage = exceptionMessage;
		}

        /// <summary>
        /// Get the key which was used to access the cache when the failure ocurred.
        /// </summary>
		public string Key
		{
			get { return key; }
		}

        /// <summary>
        /// Gets the message that represents the exception thrown when the configured callback could not be executed.
        /// </summary>
        public string ExceptionMessage
		{
			get { return exceptionMessage; }
		}
	}
}
