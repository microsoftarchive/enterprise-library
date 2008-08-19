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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Represents a key value pair that is editable in the design time.
    /// </summary>
    public class EditableKeyValue
    {
        private string key_;
        private string value_;

        /// <summary>
        /// Initialize a new instance of the <see cref="EditableKeyValue"/> class.
        /// </summary>
        public EditableKeyValue()
        {
        }

        /// <summary>
		/// Initialize a new instance of the <see cref="EditableKeyValue"/> class with a key and vlaue.
        /// </summary>
        /// <param name="key">The key for the pair.</param>
        /// <param name="value">The value for the pair.</param>
        public EditableKeyValue(string key, string value)
        {
            this.key_ = key;
            this.value_ = value;
        }

        /// <summary>
        /// Gets or sets the key for the pair.
        /// </summary>
		/// <value>
		/// The key for the pair.
		/// </value>
        public string Key
        {
            get { return key_; }
            set { key_ = value; }
        }

        /// <summary>
        /// Gets or set the value for the pair.
        /// </summary>
		/// <value>
		/// The value for the pair.
		/// </value>
        public string Value
        {
            get { return value_; }
            set { this.value_ = value; }
        }

        /// <summary>
        /// Returns a string representation of the key value pair.
        /// </summary>
		/// <returns>A string representation of the key value pair.</returns>
        public override string ToString()
        {
            return string.Format(Resources.Culture, Resources.KeyValueEditorFormat, Key, Value);
        }
    }
}
