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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Provides a user interface that can edit a reference to another node at design time.
    /// </summary>
    [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, Name="FullTrust")]
    [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.LinkDemand, Name="FullTrust")]
    public class ReferenceEditor : UITypeEditor
    {
        /// <summary>
        /// Edits the value of the specified object using the editor style indicated by <seealso cref="UITypeEditor.GetEditStyle(ITypeDescriptorContext)"/>.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that can be used to gain additional context information. </param>
        /// <param name="provider">An <see cref="IServiceProvider"/> that this editor can use to obtain services.</param>
        /// <param name="value">The object to edit.</param>
        /// <returns>The new value of the object.</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
			if (null == context) return null;
			if (null == provider) return null;

            IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));            

			if (service != null)
			{
                ReferenceTypeAttribute refTypeAttribute = GetReferenceType(context);
				ConfigurationNode currentNode = (ConfigurationNode)context.Instance;
				ConfigurationApplicationNode appNode = GetApplicationNode(currentNode);
                ConfigurationNode contextNode = (refTypeAttribute.LocalOnly) ? currentNode : appNode;
                ReferenceEditorUI control = new ReferenceEditorUI(contextNode, refTypeAttribute.ReferenceType, (ConfigurationNode)value, service, IsRequired(context));
				service.DropDownControl(control);
				if (control.SelectedNode != null)
				{
					Type propertyType = context.PropertyDescriptor.PropertyType;
					Type selectedNodeType = control.SelectedNode.GetType();
					if (propertyType == selectedNodeType || selectedNodeType.IsSubclassOf(propertyType))
					{
						return control.SelectedNode;
					}
				}
			}
            
            return null;
        }


        /// <summary>
        /// Gets the editor style used by the <seealso cref="UITypeEditor.EditValue(ITypeDescriptorContext, IServiceProvider, object)"/> method.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that can be used to gain additional context information.</param>
        /// <returns><see cref="UITypeEditorEditStyle.DropDown"/></returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

		private static bool IsRequired(ITypeDescriptorContext context)
		{
			foreach (Attribute attribute in context.PropertyDescriptor.Attributes)
			{
				if (attribute is RequiredAttribute)
				{
					return true;
				}
			}
			return false;
		}

		private static ConfigurationApplicationNode GetApplicationNode(ConfigurationNode node)
        {
			ConfigurationApplicationNode appNode = (ConfigurationApplicationNode)node.Hierarchy.FindNodeByType(typeof(ConfigurationApplicationNode));			
			return appNode;
        }

        private static ReferenceTypeAttribute GetReferenceType(ITypeDescriptorContext context)
        {
            foreach (Attribute attribute in context.PropertyDescriptor.Attributes)
            {
				ReferenceTypeAttribute refAttribute = attribute as ReferenceTypeAttribute;
                if (null != refAttribute)
                {
                    return refAttribute;
                }
            }
            
            throw new InvalidOperationException(Resources.ExceptionNoRefTypeAttribute);
        }
    }
}