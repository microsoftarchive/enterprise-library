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
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
    /// <summary>
    /// Defines events that are producing instrumentation events.
    /// </summary>
    [AttributeUsage(AttributeTargets.Event, Inherited = false, AllowMultiple = false)]
    public sealed class InstrumentationProviderAttribute : InstrumentationBaseAttribute
    {
        /// <summary>
        /// Initializes this object with the instrumentation subject name being produced.
        /// </summary>
        /// <param name="subjectName">Subect name of event being produced.</param>
        public InstrumentationProviderAttribute(string subjectName)
        : base(subjectName)
        {
            
        }
    }
}
