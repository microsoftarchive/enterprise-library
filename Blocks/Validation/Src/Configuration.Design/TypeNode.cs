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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Drawing.Design;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design
{
	/// <summary>
	/// Represents a <see cref="ValidatedTypeReference"/>.
	/// </summary>
	[Image(typeof(TypeNode))]
	[SelectedImage(typeof(TypeNode))]
	public sealed class TypeNode : ConfigurationNode
	{
		private static ValidationTypeNodeNameFormatter nameFormatter = new ValidationTypeNodeNameFormatter();
		private EventHandler<ConfigurationNodeChangedEventArgs> defaultRuleNodeRemoved;
		private RuleSetNode defaultRuleNode;
		private Type type;
		private string typeName;
		private string assemblyName;
		private string defaultRuleName;

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeNode"/> class with default values.
		/// </summary>
		public TypeNode()
			: this(new ValidatedTypeReference())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeNode"/> class for a <see cref="ValidatedTypeReference"/>.
		/// </summary>
		/// <param name="typeReferenceData">The <see cref="ValidatedTypeReference"/> to be represented.</param>
		public TypeNode(ValidatedTypeReference typeReferenceData)
			: base(nameFormatter.CreateName(typeReferenceData))
		{
			defaultRuleNodeRemoved = new EventHandler<ConfigurationNodeChangedEventArgs>(OnDefaultRuleNodeRemoved);
			typeName = typeReferenceData.Name;
			assemblyName = typeReferenceData.AssemblyName;
		}

		internal void SetType(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");

			Name = nameFormatter.CreateName(type.FullName);

			this.type = type;
			typeName = type.FullName;
			assemblyName = type.Assembly.FullName;
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
			get
			{
				return base.Name;
			}
		}

		internal string AssemblyName
		{
			get { return assemblyName; }
		}

		/// <summary>
		/// Gets the name of the represented type.
		/// </summary>
		[SRDescription("TypeNameDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[ReadOnly(true)]
		public string TypeName
		{
			get { return typeName; }
		}

		/// <summary>
		/// Gets or sets the related <see cref="RuleSetNode"/>.
		/// </summary>
		[Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
		[ReferenceType(typeof(RuleSetNode), true)]
		[SRDescription("DefaultRuleDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public RuleSetNode DefaultRule
		{
			get { return defaultRuleNode; }
			set
			{
				defaultRuleNode = LinkNodeHelper.CreateReference<RuleSetNode>(defaultRuleNode,
				 value,
				 defaultRuleNodeRemoved,
				 null);

				defaultRuleName = defaultRuleNode == null ? string.Empty : defaultRuleNode.Name;
			}
		}

		private void OnDefaultRuleNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
		{
			this.defaultRuleNode = null;
			this.defaultRuleName = null;
		}

		internal Type ResolveType()
		{
			if (type != null) return type;

			string typeString = typeName;
			if (!String.IsNullOrEmpty(assemblyName))
			{
				typeString = string.Concat(typeString, ", ", assemblyName);
			}
			type = Type.GetType(typeString, false);
			if (type == null)
			{
				IUIService uiService = ServiceHelper.GetUIService(Site);
				DialogResult dialogResult = uiService.ShowMessage(string.Format(Resources.ResolveTypeManuallyMessage, typeName), Resources.ResolveTypeManuallyCaption, System.Windows.Forms.MessageBoxButtons.YesNo);
				if (dialogResult == DialogResult.Yes)
				{
					using (TypeSelectorUI typeSelector = new TypeSelectorUI(typeof(object), typeof(object), TypeSelectorIncludes.AbstractTypes))
					{
						if (DialogResult.OK == typeSelector.ShowDialog())
						{
							type = typeSelector.SelectedType;
						}
					}
				}
			}

			return type;
		}
	}
}