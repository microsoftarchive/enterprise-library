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
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
	/// <summary>
	/// Represents a design time representation of a <see cref="ExceptionPolicyData"/> configuration element.
	/// </summary>
    [Image(typeof(ExceptionPolicyNode))]
    [SelectedImage(typeof(ExceptionPolicyNode))]
    public sealed class ExceptionPolicyNode : ConfigurationNode
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="ExceptionPolicyNode"/> class.
        /// </summary>
        public ExceptionPolicyNode() : this(new ExceptionPolicyData(Resources.DefaultExceptionPolicyNodeName))
        {
        }

        /// <summary>
		/// Initialize a new instance of the <see cref="ExceptionPolicyNode"/> class with a <see cref="ExceptionPolicyData"/> instance.
        /// </summary>
        /// <param name="exceptionPolicyData">
		/// A <see cref="ExceptionPolicyData"/> instance.
		/// </param>
        public ExceptionPolicyNode(ExceptionPolicyData exceptionPolicyData)
            : base(exceptionPolicyData == null ? Resources.DefaultExceptionPolicyNodeName : exceptionPolicyData.Name)
        {            
        }
    }
}