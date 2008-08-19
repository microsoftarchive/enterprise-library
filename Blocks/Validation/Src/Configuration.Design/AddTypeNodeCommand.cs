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
using System.Text;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design
{
    /// <summary>
    /// Represents a command for adding <see cref="TypeNode"/> to a <see cref="ValidationSettingsNode"/>.
    /// </summary>
	public class AddTypeNodeCommand : AddChildNodeCommand
	{
		private IServiceProvider serviceProvider;

        /// <summary>
        /// Initialize a new instance of the <see cref="AddTypeNodeCommand"/> class with an <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        public AddTypeNodeCommand(IServiceProvider serviceProvider)
            : base(serviceProvider, typeof(TypeNode))
		{
			this.serviceProvider = serviceProvider;
		}

        /// <summary>
        /// <para>Creates an instance of the child node class and adds it as a child of the parent node. The node will be a <see cref="TypeNode"/>.</para>
        /// </summary>
        /// <param name="node">
        /// <para>The parent node to add the newly created <see cref="TypeNode"/>.</para>
        /// </param>
        protected override void ExecuteCore(ConfigurationNode node)
		{
            Type selectedType = SelectedType;
            if (null == selectedType) return;

			base.ExecuteCore(node);

            TypeNode validationTypeNode = ChildNode as TypeNode;
			if (validationTypeNode == null) return;

            try
            {
                validationTypeNode.SetType(selectedType);
            }
            catch (InvalidOperationException)
            {
                validationTypeNode.Remove();
                UIService.ShowError(string.Format(Resources.Culture, Resources.DuplicateValidationTypeErrorMessage, selectedType != null ? selectedType.Name : string.Empty));
            }
		}

        /// <summary>
        /// Gets the type that should be used to create the new <see cref="TypeNode"/>.
        /// </summary>
        protected virtual Type SelectedType
        {
            get
            {
                using (TypeSelectorUI selector = new TypeSelectorUI(
                    typeof(Object),
                    typeof(Object),
                    TypeSelectorIncludes.AbstractTypes))
                {
                    selector.CollapseAssemlbyNodes();
                    DialogResult result = selector.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        return selector.SelectedType;
                    }
                }
                return null;
            }
        }
	}
}
