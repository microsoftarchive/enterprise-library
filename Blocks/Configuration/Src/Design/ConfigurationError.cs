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

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Represents an error that occurs while operating on a <see cref="ConfigurationNode"/> object.
    /// </summary>   
    public class ConfigurationError
    {
        private readonly ConfigurationNode node;
        private readonly string message;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationError"/> class with the <see cref="ConfigurationNode"/> and an error message.
        /// </summary>
        /// <param name="node">The <see cref="ConfigurationNode"/> object.</param>
        /// <param name="message">The. error message.</param>
        public ConfigurationError(ConfigurationNode node, string message)
        {
            this.node = node;
            this.message = message;
        }

        /// <summary>
        /// Gets the <see cref="ConfigurationNode"/> where the error originated.
        /// </summary>
        /// <value>
        /// The <see cref="ConfigurationNode"/> where the error originated.
        /// </value>
		public ConfigurationNode ConfigurationNode
        {
            get { return node; }
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string Message
        {
            get { return message; }
        }

        /// <summary>
        /// Creates and returns a string representation of the current error.
        /// </summary>
        /// <returns>A string representation of the current error.</returns>
        public override string ToString()
        {
            return string.Format(Resources.Culture, Resources.ConfigurationErrorToString, node.Name,
												 node.Path,
                                                 message);
        }

    }
}