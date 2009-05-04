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
	/// Base class for attributes used to identify instrumentation producers or consumers.
	/// </summary>
    public class InstrumentationBaseAttribute : Attribute
    {
        string subjectName;

        /// <summary>
		/// Initializes this instance with the instrumentation subject name.
        /// </summary>
        /// <param name="subjectName">Subject name being produced or consumed</param>
        protected InstrumentationBaseAttribute(string subjectName)
        {
            if (String.IsNullOrEmpty(subjectName)) throw new ArgumentException("subjectName");
            
            this.subjectName = subjectName;
        }

        /// <summary>
		/// Gets the subject name
        /// </summary>
        public string SubjectName { get { return subjectName; } }
    }
}
