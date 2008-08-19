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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Properties;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
	/// <summary>
	/// Represents a design time representation of a <see cref="ExceptionTypeData"/> configuration element.
	/// </summary>
    [Image(typeof(ExceptionTypeNode))]
    [SelectedImage(typeof(ExceptionTypeNode))]
    public sealed class ExceptionTypeNode : ConfigurationNode
    {
        private static ExceptionTypeNodeNameFormatter nodeNameFormatter = new ExceptionTypeNodeNameFormatter();
        
		private string typeName;
		private PostHandlingAction postHandlingAction;

        /// <summary>
        /// Initialize a new instance of the <see cref="ExceptionTypeNode"/> class.
        /// </summary>
        public ExceptionTypeNode() : this(new ExceptionTypeData(Resources.DefaultExceptionTypeNodeName, typeof(Exception), PostHandlingAction.NotifyRethrow))
        {
        }

        /// <summary>
		/// Initialize a new instance of the <see cref="ExceptionTypeNode"/> class with a <see cref="ExceptionTypeData"/> instance.
        /// </summary>
        /// <param name="exceptionTypeData">A <see cref="ExceptionTypeData"/> instance.</param>
        public ExceptionTypeNode(ExceptionTypeData exceptionTypeData)
            : base(nodeNameFormatter.CreateName(exceptionTypeData))
        {
            if (exceptionTypeData == null)
            {
                throw new ArgumentNullException("exceptionTypeData");
            }

            this.typeName = exceptionTypeData.TypeName;
			this.postHandlingAction = exceptionTypeData.PostHandlingAction;
        }

        /// <summary>
        /// Gets the name of the node.
        /// </summary>
		/// <value>
		/// The name of the node.
		/// </value>
        [ReadOnly(true)]
        public override string Name
        {
            get { return base.Name; }            
        }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> of the <see cref="Exception"/> for the policy.
        /// </summary>
		/// <value>
		/// The <see cref="Type"/> of the <see cref="Exception"/> for the policy.
		/// </value>
        [Required]
        [SRDescription("ExceptionTypeNodeNameDescription", typeof(Resources))]
        [ReadOnly(true)]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Type
        {
            get { return typeName; }
            set
            {
				if (null == value) throw new ArgumentNullException("value");
                typeName = value;
                Name = nodeNameFormatter.CreateName(typeName);
            }
        }

        /// <summary>
        /// <para>Determines how a rethrow is handled.</para>
        /// <list type="table">
        ///		<listheader>
        ///			<item>Enumeration</item>
        ///			<description>Description</description>
        ///		</listheader>
        ///		<item>
        ///			<term>None</term>
        ///			<description>
        ///			Indicates that no rethrow should occur.
        ///			</description>
        ///		</item>
        ///		<item>
        ///			<term>Notify</term>
        ///			<description>
        ///			Notify the caller that a Rethrow is recommended.
        ///			</description>
        ///		</item>
        ///		<item>
        ///			<term>Throw</term>
        ///			<description>
        ///			Throws the exception after the exception has been handled by all handlers in the chain.
        ///			</description>
        ///		</item>
        /// </list>
        /// </summary>
        [SRDescription("ExceptionTypePostHandlingActionDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [Required]
        public PostHandlingAction PostHandlingAction
        {
            get { return postHandlingAction; }
            set { postHandlingAction = value; }
        }        

        /// <summary>
        /// Determines if the the child nodes are sorted by name.
        /// </summary>
		/// <value>
		/// Returns <see langword="false"/>.
		/// </value>
		/// <remarks>
		/// Child nodes must be in order that they are added because they are handled in a chain.
		/// </remarks>
        public override bool SortChildren
        {
            get { return false; }
        }		
    }
}