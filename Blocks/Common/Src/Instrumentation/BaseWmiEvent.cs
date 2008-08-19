//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Management.Instrumentation;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
    /// <summary>
    /// Base class for Enterprise Library's WMI Events. 
    /// </summary>
    [InstrumentationClass(InstrumentationType.Event)]
    public abstract class BaseWmiEvent
    {
        private DateTime utcTimeStamp = DateTime.UtcNow;

        /// <summary>
        /// Gets the timestamp for this WMI Event.
        /// </summary>
        public DateTime UtcTimeStamp
        {
            get { return utcTimeStamp; }
        }
    }
}
