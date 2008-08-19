//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Instrumentation
{
    /// <summary>
	/// Base class for WMI events fired when an validation operation is performed.
	/// </summary>
    public class ValidationCalledEvent : ValidationEvent
    {
        private string typeBeingValidated;

        /// <summary>
		/// Initializes a new instance of the <see cref="ValidationCalledEvent"/> class.
        /// </summary>
		/// <param name="typeBeingValidated">The name of the type being validated.</param>
        public ValidationCalledEvent(string typeBeingValidated)
        {
            this.typeBeingValidated = typeBeingValidated;
        }

        /// <summary>
        /// Gets the name of the type being validated.
        /// </summary>
        public string TypeBeingValidated
        {
            get { return typeBeingValidated; }
        }
    }
}
