//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Unity
{
	/// <summary>
	/// Container extension to the policies required to create the Data Access Application Block's
	/// objects described in the configuration file.
	/// </summary>
	public class DataAccessBlockExtension : EnterpriseLibraryBlockExtension
	{
		/// <summary>
		/// Adds the policies describing the Data Access Application Block's objects.
		/// </summary>
		protected override void Initialize()
		{
			DatabaseConfigurationView configurationView = new DatabaseConfigurationView(ConfigurationSource);
			string defaultDatabaseName = configurationView.DefaultName;

			foreach (ConnectionStringSettings connectionStringSettings 
				in configurationView.GetConnectionStringSettingsCollection())
			{
				if (!string.IsNullOrEmpty(connectionStringSettings.ProviderName))
				{
					DbProviderMapping mapping
						= configurationView.GetProviderMapping(
							connectionStringSettings.Name,
							connectionStringSettings.ProviderName);
					Type databaseType = mapping.DatabaseType;

					// add type mapping
					this.Context.Policies.Set<IBuildKeyMappingPolicy>(
						new BuildKeyMappingPolicy(new NamedTypeBuildKey(databaseType, connectionStringSettings.Name)),
						NamedTypeBuildKey.Make<Database>(connectionStringSettings.Name));

					// add default mapping, if appropriate
					if (connectionStringSettings.Name == defaultDatabaseName)
					{
						this.Context.Policies.Set<IBuildKeyMappingPolicy>(
							new BuildKeyMappingPolicy(new NamedTypeBuildKey(databaseType, connectionStringSettings.Name)),
							NamedTypeBuildKey.Make<Database>());
					}

					// add ctor selector, based on the actual db type
					IContainerPolicyCreator policyCreator = GetContainerPolicyCreator(databaseType, null);
					policyCreator.CreatePolicies(
						this.Context.Policies,
						connectionStringSettings.Name,
						connectionStringSettings,
						this.ConfigurationSource);
				}
			}
		}
	}
}