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
using System.Reflection;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design
{
    /// <summary>
    /// Represents a command for adding a member to a <see cref="RuleSetNode"/>.
    /// </summary>
    public class ChooseMembersCommand : AddChildNodeCommand
    {
        private IServiceProvider serviceProvider;

        /// <summary>
		/// Initialize a new instance of the <see cref="ChooseMembersCommand"/> class with an <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        public ChooseMembersCommand(IServiceProvider serviceProvider)
            : base(serviceProvider, typeof(ConfigurationNode))
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Adds either a <see cref="FieldNode"/>, <see cref="MethodNode"/> or <see cref="PropertyNode"/> to the current <see cref="RuleSetNode"/>,
        /// based on the selected member.
        /// </summary>
        /// <param name="node">The parent node to newly added configuration node.</param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
            try
            {
                UIService.BeginUpdate();
                Type type = FindType(node);
                if (type != null)
                {
                    IUIService uiService = ServiceHelper.GetUIService(serviceProvider);
                    TypeMemberChooser memberChooser = new TypeMemberChooser(uiService);
                    foreach (MemberInfo memberInfo in memberChooser.ChooseMembers(type))
                    {
                        if (memberInfo != null)
                        {
                            if (node.Nodes.Contains(memberInfo.Name)) continue;

                            ConfigurationNode childNode = null;
                            if (memberInfo is PropertyInfo)
                            {
                                childNode = new PropertyNode(memberInfo.Name);
                            }
                            else if (memberInfo is MethodInfo)
                            {
                                childNode = new MethodNode(memberInfo.Name);
                            }
                            else if (memberInfo is FieldInfo)
                            {
                                childNode = new FieldNode(memberInfo.Name);
                            }
                            else
                            {
                                Debug.Assert(false, "memberInfo should be either PropertyInfo, MethodInfo or FieldInfo");
                            }

                            node.AddNode(childNode);
                            UIService.SetUIDirty(node.Hierarchy);
                        }
                    }
                }
            }
            finally
            {
                UIService.EndUpdate();
            }
        }

        private Type FindType(ConfigurationNode node)
        {
            while (node as TypeNode == null && node.Parent != null)
            {
                node = node.Parent;
            }
            TypeNode typeNode = node as TypeNode;
            Debug.Assert(typeNode != null, "no TypeNode found up in hierarchy");

            return typeNode.ResolveType();
        }
    }
}
