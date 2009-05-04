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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    /// <summary>
	/// Validate that each instance of a <see cref="Type"/> has a unique name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public sealed class UniqueNameAttribute : ValidationAttribute
    {
        private readonly Type nodeType;
		private readonly Type containerType;

        /// <summary>
        /// Initialize a new instance of the <see cref="UniqueNameAttribute"/> class with a <see cref="Type"/>.
        /// </summary>
        /// <param name="nodeType">The <see cref="Type"/> to have a unique name.</param>
		/// <param name="containerType">The root node type of where the name has to be unique.</param>
        public UniqueNameAttribute(Type nodeType, Type containerType)
        {
            this.nodeType = nodeType;
			this.containerType = containerType;
        }

		/// <summary>
		/// Gets the root node type of where the name has to be unique.
		/// </summary>
		/// <value>
		/// The root node type of where the name has to be unique.
		/// </value>
		public Type ContainerType 
		{
			get { return containerType;  }
		}

		/// <summary>
		/// Gets the node <see cref="Type"/> that has to have a unique name.
		/// </summary>
		/// <value>
		/// The node <see cref="Type"/> that has to have a unique name.
		/// </value>
		public Type NodeType 
		{
			get { return nodeType;  }
		}

		/// <summary>
		/// Validate value is unique for the given <paramref name="instance"/> and the <paramref name="propertyInfo"/> across a <see cref="Type"/>.
		/// </summary>
		/// <param name="instance">
		/// The instance to validate.
		/// </param>
		/// <param name="propertyInfo">
		/// The property containing the value to validate.
		/// </param>
		/// <param name="errors">
		/// The collection to add any errors that occur during the validation.
		/// </param> 		
		protected override void ValidateCore(object instance, PropertyInfo propertyInfo, IList<ValidationError> errors)
        {
            ConfigurationNode node = instance as ConfigurationNode;
            if (node != null)
            {
                string name = propertyInfo.GetValue(instance, null) as string;
                if (name != null)
                {
                    List<ConfigurationNode> nodesWithSameType = new List<ConfigurationNode>(node.Hierarchy.FindNodesByType(nodeType));
                    nodesWithSameType.ForEach(delegate (ConfigurationNode foundNode) {
                        if (foundNode != instance &&  string.Compare(foundNode.Name, name) == 0)
                        {
                            string errorMessage = string.Format(Resources.Culture, Resources.UniqueNameErrorMessage, name);
                            errors.Add(new ValidationError(instance as ConfigurationNode, propertyInfo.Name, errorMessage));
                        }
                    });
                }
            }
        }

    }
}
