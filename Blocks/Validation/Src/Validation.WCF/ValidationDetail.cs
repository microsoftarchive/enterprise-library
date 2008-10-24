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
using System.Runtime.Serialization;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF
{
    /// <summary>
    /// This class holds the results of a single validation. Effectively,
    /// it&apos;s the same as a ValidationResult, but creating a separate
    /// class allows us to mark up a DataContract with impunity.
    /// </summary>
    [DataContract]
    public class ValidationDetail
    {
        private string message;
        private string key;
        private string tag;

        /// <summary>
        /// Construct a new <see cref="ValidationDetail"/> object with empty
        /// fields.
        /// </summary>
        public ValidationDetail()
        {
        }

        /// <summary>
        /// Construct a new <see cref="ValidationDetail"/> object with the given
        /// values.
        /// </summary>
        /// <param name="message">Message about validation failure.</param>
        /// <param name="key">Name describing the location of the validation result.</param>
        /// <param name="tag">Get a value characterizing the result.</param>
        public ValidationDetail(string message, string key, string tag)
        {
            this.message = message;
            this.key = key;
            this.tag = tag;
        }

        /// <summary>
        /// Get or set a message describing the validation failure.
        /// </summary>
        [DataMember]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        /// <summary>
        /// Get or set a name describing the location of the validation result.
        /// </summary>
        [DataMember]
        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        /// <summary>
        /// Get or set a value characterizing the fault.
        /// </summary>
        /// <remarks>The meaning for a tag is determined by the client code consuming the <see cref="ValidationDetail"/>.</remarks>
        [DataMember]
        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }
    }
}
