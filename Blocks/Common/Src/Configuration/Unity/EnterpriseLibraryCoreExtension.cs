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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
	/// <summary>
	/// Main <see cref="UnityContainerExtension"/> for Enterprise Library.
	/// </summary>
	/// <remarks>
	/// This extension specifies the <see cref="IConfigurationSource"/> to be used by all the extensions for the Blocks
	/// and adds the <see cref="InstrumentationStrategy"/> to the strategy chain.
	/// </remarks>
	public class EnterpriseLibraryCoreExtension : UnityContainerExtension
	{
		private readonly IConfigurationSource configurationSource;

		/// <summary>
		/// Initializes a new instance of the <see cref="EnterpriseLibraryCoreExtension"/> class with the
		/// default <see cref="IConfigurationSource"/>.
		/// </summary>
		public EnterpriseLibraryCoreExtension()
			: this(ConfigurationSourceFactory.Create())
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="EnterpriseLibraryCoreExtension"/> class with the
		/// the specified <see cref="IConfigurationSource"/>.
		/// </summary>
		/// <param name="configurationSource">The <see cref="IConfigurationSource"/> to use when retrieving
		/// configuration information.</param>
		public EnterpriseLibraryCoreExtension(IConfigurationSource configurationSource)
		{
			this.configurationSource = configurationSource;
		}

		/// <summary>
		/// Adds the policies used by other extensions to locate an <see cref="IConfigurationSource"/> and
		/// a <see cref="ConfigurationReflectionCache"/>, and adds the <see cref="InstrumentationStrategy"/>
		/// to the strategy chain.
		/// </summary>
		protected override void Initialize()
		{
			Context.Policies.Set<IConfigurationObjectPolicy>(
				new ConfigurationObjectPolicy(this.configurationSource), 
				typeof(IConfigurationSource));
			Context.Policies.Set<IReflectionCachePolicy>(
				new ReflectionCachePolicy(new ConfigurationReflectionCache()),
				typeof(IReflectionCachePolicy));

			Context.Strategies.AddNew<InstrumentationStrategy>(UnityBuildStage.PostInitialization);
		}
	}
}
