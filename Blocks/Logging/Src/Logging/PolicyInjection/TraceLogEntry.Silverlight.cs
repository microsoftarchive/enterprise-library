//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Runtime.Serialization;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection
{
    /// <summary>
    /// A <see cref="LogEntry"/> class that contains the extra information logged by the <see cref="LogCallHandler"/>.
    /// </summary>
    public class TraceLogEntry : LogEntry
    {
        /// <summary>
        /// Gets or sets the name of type implementing the method being called.
        /// </summary>
        /// <value>The type name</value>
        [IgnoreDataMember]
        public string TypeName
        {
            get { return this.GetValueOrNull("TypeName"); }
            set { this.ExtendedProperties["TypeName"] = value; }
        }

        /// <summary>
        /// Gets or sets the method name being called.
        /// </summary>
        /// <value>The method name</value>
        [IgnoreDataMember]
        public string MethodName
        {
            get { return this.GetValueOrNull("MethodName"); }
            set { this.ExtendedProperties["MethodName"] = value; }
        }

        /// <summary>
        /// Return value from the call.
        /// </summary>
        /// <value>return value</value>
        [IgnoreDataMember]
        public string ReturnValue
        {
            get { return this.GetValueOrNull("ReturnValue"); }
            set { this.ExtendedProperties["ReturnValue"] = value; }
        }

        /// <summary>
        /// The call stack from the current call.
        /// </summary>
        /// <value>call stack string.</value>
        [IgnoreDataMember]
        public string CallStack
        {
            get { return this.GetValueOrNull("CallStack"); }
            set { this.ExtendedProperties["CallStack"] = value; }
        }

        /// <summary>
        /// Exception thrown from the target.
        /// </summary>
        /// <value>If exception was thrown, this is the exception object. Null if no exception thrown.</value>
        [IgnoreDataMember]
        public string Exception
        {
            get { return this.GetValueOrNull("Exception"); }
            set { this.ExtendedProperties["Exception"] = value; }
        }

        /// <summary>
        /// Total time to call the target.
        /// </summary>
        /// <value>null if not logged, else the elapsed time.</value>
        [IgnoreDataMember]
        public TimeSpan? CallTime
        {
            get {
                string serialized = this.GetValueOrNull("CallTime");
                TimeSpan callTime;
                if (!string.IsNullOrEmpty(serialized) && TimeSpan.TryParse(serialized, CultureInfo.InvariantCulture, out callTime))
                {
                    return callTime;
                }

                return null;
            }
            set { this.ExtendedProperties["CallTime"] = value.HasValue ? value.Value.ToString(null, CultureInfo.InvariantCulture) : null; }
        }

        /// <summary>
        /// This is to support WMI instrumentation by returning
        /// the actual <see cref="CallTime"/> 
        /// </summary>
        [IgnoreDataMember]
        public TimeSpan ElapsedTime
        {
            get { return this.CallTime.HasValue ? this.CallTime.Value : TimeSpan.Zero; }
        }

        private string GetValueOrNull(string key)
        {
            object value;
            if (this.ExtendedProperties.TryGetValue(key, out value))
            {
                return value as string;
            }

            return null;
        }
    }
}
