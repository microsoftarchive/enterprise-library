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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
	/// <summary>
	/// Main <see cref="UnityContainerExtension"/> for Enterprise Library.
	/// </summary>
	/// <remarks>
	/// This extension configures its container to resolve all Enterprise Library
	/// objects. It's a convienence method to save having to manually create a 
	/// <see cref="UnityContainerConfigurator"/> object and configure it yourself.
	/// </remarks>
	public class EnterpriseLibraryCoreExtension : UnityContainerExtension
	{
		private readonly IConfigurationSource configurationSource;

		/// <summary>
		/// Initializes a new instance of the <see cref="EnterpriseLibraryCoreExtension"/> class with the
		/// default <see cref="IConfigurationSource"/>.
		/// </summary>		
		[InjectionConstructor]
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
		/// Configures the Unity container to be able to resolve Enterprise Library
		/// objects.
		/// </summary>
		protected override void Initialize()
		{
            // Only need to do this once; do nothing if this extension has already
            // been added.
            if(Container.Configure<EnterpriseLibraryCoreExtension>() == this)
            {
                var configurator = new UnityContainerConfigurator(Container);
                EnterpriseLibraryContainer.ConfigureContainer(configurator, configurationSource);
            }
		}
	}
}
