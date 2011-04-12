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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity
{
    /// <summary>
    /// The <see cref="UnityContainer"/> specific configurator for <see cref="TypeRegistration"/> entries.
    /// </summary>
    public partial class UnityContainerConfigurator : IContainerConfigurator
    {
        /// <summary>
        /// Initializer for the configurator.
        /// </summary>
        /// <param name="container">The <see cref="IUnityContainer"/> to configure.</param>
        public UnityContainerConfigurator(IUnityContainer container)
        {
            this.container = container;

            //this.container.AddNewExtensionIfNotPresent<Interception>();
            this.container.AddNewExtensionIfNotPresent<TransientPolicyBuildUpExtension>();
            //this.container.AddNewExtensionIfNotPresent<ReaderWriterLockExtension>();
            this.container.AddNewExtensionIfNotPresent<LifetimeInspector>();
            this.container.AddNewExtensionIfNotPresent<PolicyListAccessor>();
            AddValidationExtension();
            AddInterceptionExtension();
        }

        /// <summary>
        /// Consume the set of <see cref="TypeRegistration"/> objects and
        /// configure the associated container.
        /// </summary>
        /// <param name="configurationSource">Configuration source to read registrations from.</param>
        /// <param name="rootProvider"><see cref="ITypeRegistrationsProvider"/> that knows how to
        /// read the <paramref name="configurationSource"/> and return all relevant type registrations.</param>
        public void RegisterAll(IConfigurationSource configurationSource, ITypeRegistrationsProvider rootProvider)
        {
            this.RegisterConfigurationSource(configurationSource);
            this.RegisterAllCore(configurationSource, rootProvider);
        }

        /// <summary>
        /// Consume the set of <see cref="TypeRegistration"/> objects and
        /// configure the associated container.
        /// </summary>
        /// <param name="configurationSource">Configuration source to read registrations from.</param>
        /// <param name="rootProvider"><see cref="ITypeRegistrationsProvider"/> that knows how to
        /// read the <paramref name="configurationSource"/> and return all relevant type registrations.</param>
        protected void RegisterAllCore(IConfigurationSource configurationSource, ITypeRegistrationsProvider rootProvider)
        {
            foreach (var registration in rootProvider.GetRegistrations(configurationSource))
            {
                Register(registration);
            }
        }

        private void AddValidationExtension()
        {
            // We load this by name so we don't have a hard dependency from common -> validation
            const string extensionElementName =
                "{clr-namespace:Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Unity;assembly=Microsoft.Practices.EnterpriseLibrary.Validation.Silverlight}ValidationBlockExtension";

            var vabExtension = XamlActivator.CreateInstance<UnityContainerExtension>(extensionElementName);

            if (vabExtension != null && container.Configure(vabExtension.GetType()) == null)
            {
                container.AddExtension(vabExtension);
            }
        }

        private void AddInterceptionExtension()
        {
            // We load this by name so we don't have a hard dependency from common -> interception
            const string extensionElementName =
                "{clr-namespace:Microsoft.Practices.Unity.InterceptionExtension;assembly=Microsoft.Practices.Unity.Interception.Silverlight}Interception";

            var interceptionExtension = XamlActivator.CreateInstance<UnityContainerExtension>(extensionElementName);

            if (interceptionExtension != null && container.Configure(interceptionExtension.GetType()) == null)
            {
                container.AddExtension(interceptionExtension);
            }
        }
    }
}
