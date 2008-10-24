//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
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
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
	/// <summary>
	/// Represents a design time representation of a <see cref="ExceptionHandlerData"/> configuration element.
	/// </summary>
    [Image(typeof(ExceptionHandlerNode))]
    [SelectedImage(typeof(ExceptionHandlerNode))]
    public abstract class ExceptionHandlerNode : ConfigurationNode
    {        

        /// <summary>
        /// Initialize a new instance of the <see cref="ExceptionHandlerNode"/> class.
        /// </summary>        
        protected ExceptionHandlerNode()
        {            
        }

        /// <summary>
        /// When overriden by a class, gets the <see cref="ExceptionHandlerData"/> object this node represents.
        /// </summary>
		/// <value>
		/// The <see cref="ExceptionHandlerData"/> object this node represents.
		/// </value>
        [Browsable(false)]
        public abstract ExceptionHandlerData ExceptionHandlerData { get; }
    }
}
