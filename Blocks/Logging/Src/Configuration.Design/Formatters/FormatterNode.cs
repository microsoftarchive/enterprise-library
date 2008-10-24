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
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters
{
	   
	/// <summary>
	/// Represents a <see cref="FormatterData"/> configuration element. This class is abstract.
	/// </summary>
    [Image(typeof(FormatterNode))]
    public abstract class FormatterNode : ConfigurationNode
    {       

        /// <summary>
        /// Initialize a new instance of the <see cref="FormatterNode"/> class.
        /// </summary>        
        protected FormatterNode()
        {           
        }

        /// <summary>
        /// Gets the <see cref="FormatterData"/> this node represents.
        /// </summary>
		/// <value>
		/// The <see cref="FormatterData"/> this node represents.
		/// </value>
        [Browsable(false)]
        public abstract FormatterData FormatterData { get; }
    }
}
