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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides
{
	/// <summary>
	/// Represents a designtime node for an environment that contains alternate configuration.
	/// </summary>
	[Image(typeof(EnvironmentNode))]
	[SelectedImage(typeof(EnvironmentNode))]
	[ProvideProperty("Overrides", typeof(ConfigurationNode))]
	public class EnvironmentNode : ConfigurationSectionNode, IExtenderProvider
	{
		private EnvironmentMergeData environmentData;
		private string environmentConfigurationFile;
		private string environmentDeltaFile;


		/// <summary>
		/// Initializes a new instance of <see cref="EnvironmentNode"/>.
		/// </summary>
		public EnvironmentNode()
			: this(new EnvironmentMergeData())
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="EnvironmentNode"/> given a <see cref="EnvironmentMergeData"/> instance.
		/// </summary>
		/// <param name="environmentData">The <see cref="EnvironmentMergeData"/> that contains the information for this environemnt.</param>
		public EnvironmentNode(EnvironmentMergeData environmentData)
			: base(environmentData.EnvironmentName)
		{
			this.environmentData = environmentData;
			this.environmentDeltaFile = environmentData.EnvironmentDeltaFile;
			this.environmentConfigurationFile = environmentData.EnvironmentConfigurationFile;
		}

		/// <summary>
		/// Gets <see cref="EnvironmentMergeData"/> represented by this node.
		/// </summary>
		[Browsable(false)]
		internal EnvironmentMergeData EnvironmentMergeData
		{
			get { return environmentData; }
		}

		/// <summary>
		/// Gets or sets the location of the environment delta configuration (.dconfig).
		/// </summary>
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("EnvironmentDeltaFileDescription", typeof(Resources))]
		[Required]
		[FileValidation]
		[ExternalConfigurationFileStorageCreationAttribute]
		public string EnvironmentDeltaFile
		{
			get { return environmentDeltaFile; }
			set { environmentDeltaFile = value; }
		}

		/// <summary>
		/// Gets or sets the location of the file should be used as output for merging the environmental configuration.
		/// </summary>
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("EnvironmentConfigurationFileDescription", typeof(Resources))]
		[Required]
		public string EnvironmentConfigurationFile
		{
			get { return environmentConfigurationFile; }
			set { environmentConfigurationFile = value; }
		}

		/// <summary>
		/// Returns a boolen on whether the given <see cref="System.Object"/> can contain overridden properties in its designtime.
		/// </summary>
		/// <param name="extendee">An instance of <see cref="ConfigurationNode"/> that is contained within a <see cref="IConfigurationUIHierarchy"/>.</param>
		/// <returns><see langword="true"/> if the <paramref name="extendee"/> can be extended, otherwise <see langword="false"/>.</returns>
		public bool CanExtend(object extendee)
		{
			ConfigurationNode configurationNodeToExtend = extendee as ConfigurationNode;
			if (configurationNodeToExtend == null) return false;
			if (configurationNodeToExtend.Hierarchy == null) return false;

			if (typeof(EnvironmentNode).IsAssignableFrom(configurationNodeToExtend.GetType())) return false;
			if (typeof(ConfigurationApplicationNode).IsAssignableFrom(configurationNodeToExtend.GetType())) return false;
            if (typeof(ConfigurationSourceSectionNode).IsAssignableFrom(configurationNodeToExtend.GetType())) return false;
            if (typeof(ConfigurationSourceElementNode).IsAssignableFrom(configurationNodeToExtend.GetType())) return false;

			return true;
		}

		/// <summary>
		/// Returns the <see cref="MergedConfigurationNode"/> for a given <see cref="ConfigurationNode"/>. 
		/// The <see cref="MergedConfigurationNode"/> represents the differences for a <see cref="ConfigurationNode"/> in a specific environment and
		/// can be displayed in the property grid.
		/// </summary>
		/// <param name="node">The <see cref="ConfigurationNode"/> whose <see cref="MergedConfigurationNode"/> should be returned.</param>
		/// <returns>An instance of <see cref="MergedConfigurationNode"/> that can be displayed in the property grid.</returns>
		[SRCategory("CategoryOverrides", typeof(Resources))]
		public MergedConfigurationNode GetOverrides(ConfigurationNode node)
		{
			ConfigurationNodeMergeData mergeData = environmentData.GetMergeData(node);

			return new MergedConfigurationNode(node, mergeData);
		}

		/// <summary>
		/// Updates the <see cref="MergedConfigurationNode"/> for a specific <see cref="ConfigurationNode"/> instance.
		/// </summary>
		/// <param name="node">The <see cref="ConfigurationNode"/> whose properties in this environment should be updated.</param>
		/// <param name="value">The <see cref="MergedConfigurationNode"/> instance that contains all the differences for the <paramref name="node"/>.</param>
		public void SetOverrides(ConfigurationNode node, MergedConfigurationNode value)
		{
			environmentData.UpdateMergeData(node, value.MergeData);
		}

		/// <summary>
		/// Gets or sets the flag indicating the section requires permission to be retrieved.
		/// </summary>
		/// <remarks>
		/// The section represented by this node is defined elsewhere, so the property does not need to be exposed.
		/// </remarks>
		[Browsable(false)]
		public override bool RequirePermission
		{
			get { return base.RequirePermission; }
			set { base.RequirePermission = value; }
		}
	}
}
