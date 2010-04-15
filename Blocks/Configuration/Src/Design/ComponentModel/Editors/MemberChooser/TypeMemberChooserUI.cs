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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Windows.Forms.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors.MemberChooser;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design
{
	/// <summary>
	/// User interface for selecting multiple members from a type.
	/// </summary>
	public partial class TypeMemberChooserUI : Form
	{
		private TreeNode typeTreeNode;

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeMemberChooserUI"/> class.
		/// </summary>
		/// <param name="type">The <see cref="Type"/> for which the members are to be chosen from.</param>
		/// <param name="uiService">The <see cref="IUIService"/> to provide UI services.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "uiService")]
        public TypeMemberChooserUI(Type type, IUIService uiService)
		{
			InitializeComponent();

			typeTreeNode = new TypeTreeNode(type);
			ContainerTreeNode propertiesTreeNode = new ContainerTreeNode(MemberChooserResources.PropertiesNode);

			//add properties to the treeview
			foreach (PropertyInfo propertyInfo in type.GetProperties())
			{
                if (propertyInfo.CanRead && propertyInfo.GetIndexParameters().Length == 0)
                {
                    if (!propertiesTreeNode.Nodes.ContainsKey(propertyInfo.Name))
                    {
                        propertiesTreeNode.Nodes.Add(new PropertyTreeNode(propertyInfo));
                    }
                }
			}

			//add methods to the treeview
			ContainerTreeNode methodsTreeNode = new ContainerTreeNode(MemberChooserResources.MethodsNode);
			foreach (MethodInfo methodInfo in type.GetMethods())
			{
				if (!methodInfo.IsSpecialName &&
					methodInfo.GetParameters().Length == 0 &&
					methodInfo.ReturnType != typeof(void))
				{
					if (!methodsTreeNode.Nodes.ContainsKey(methodInfo.Name))
					{
						methodsTreeNode.Nodes.Add(new MethodTreeNode(methodInfo));
					}
				}
			}

			//add methods to the treeview
			ContainerTreeNode fieldsTreeNode = new ContainerTreeNode(MemberChooserResources.FieldsNode);
			foreach (FieldInfo fieldInfo in type.GetFields())
			{
				if (!fieldsTreeNode.Nodes.ContainsKey(fieldInfo.Name))
				{
					fieldsTreeNode.Nodes.Add(new FieldTreeNode(fieldInfo));
				}
			}

			typeTreeNode.Nodes.AddRange(new TreeNode[] { 
                propertiesTreeNode, 
                methodsTreeNode, 
                fieldsTreeNode });

			typeTreeNode.ExpandAll();
			memberTreeView.Nodes.Add(typeTreeNode);
		}

		/// <summary>
		/// Returns the selected members.
		/// </summary>
		public IEnumerable<MemberInfo> GetSelectedMembers()
		{
			return GetSelectedMembers(typeTreeNode);
		}

		private IEnumerable<MemberInfo> GetSelectedMembers(TreeNode parentNode)
		{
			foreach (TreeNode treeNode in parentNode.Nodes)
			{
				if (treeNode.Checked)
				{
					MemberInfoTreeNode memberInfoNode = treeNode as MemberInfoTreeNode;
					if (memberInfoNode != null && memberInfoNode.MemberInfo != null)
					{
						yield return memberInfoNode.MemberInfo;
					}
				}

				foreach (MemberInfo selectedMember in GetSelectedMembers(treeNode))
				{
					yield return selectedMember;
				}
			}
		}

		private sealed class TypeTreeNode : TreeNode
		{
			public TypeTreeNode(Type type)
				: base(type.Name)
			{
				ImageIndex = 0;
				SelectedImageIndex = 0;
			}
		}

		private sealed class ContainerTreeNode : TreeNode
		{
			public ContainerTreeNode(string containerNodeName)
				: base(containerNodeName)
			{
				ImageIndex = 4;
				SelectedImageIndex = 4;
			}

		}

		private abstract class MemberInfoTreeNode : TreeNode
		{
			MemberInfo memberInfo;

			protected MemberInfoTreeNode(MemberInfo memberInfo)
				: base(memberInfo.Name)
			{
				this.memberInfo = memberInfo;
			}

			public MemberInfo MemberInfo
			{
				get { return memberInfo; }
			}
		}

		private sealed class MethodTreeNode : MemberInfoTreeNode
		{
			public MethodTreeNode(MethodInfo methodInfo)
				: base(methodInfo)
			{
				ImageIndex = 2;
				SelectedImageIndex = 2;
			}
		}

		private sealed class PropertyTreeNode : MemberInfoTreeNode
		{
			public PropertyTreeNode(PropertyInfo propertyInfo)
				: base(propertyInfo)
			{
				ImageIndex = 3;
				SelectedImageIndex = 3;
			}
		}

		private sealed class FieldTreeNode : MemberInfoTreeNode
		{
			public FieldTreeNode(FieldInfo fieldInfo)
				: base(fieldInfo)
			{
				ImageIndex = 1;
				SelectedImageIndex = 1;
			}
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		private void memberTreeView_AfterCheck(object sender, TreeViewEventArgs e)
		{
			if (e.Node.Nodes.Count != 0)
			{
				RecursiveSetCheckedState(e.Node, e.Node.Checked);
			}
		}

		private void RecursiveSetCheckedState(TreeNode treeNode, bool checkedState)
		{
			foreach (TreeNode childNode in treeNode.Nodes)
			{
				childNode.Checked = checkedState;
				RecursiveSetCheckedState(childNode, checkedState);
			}
		}

	}
}
