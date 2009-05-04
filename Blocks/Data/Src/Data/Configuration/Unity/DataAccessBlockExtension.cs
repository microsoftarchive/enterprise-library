//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
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
            var configurator = new UnityContainerConfigurator(Container);
            var settings = new DatabaseSyntheticConfigSettings(ConfigurationSource);
            string defaultDatabaseName = settings.DefaultDatabase;

            foreach (var typeRegistration in settings.CreateRegistrations())
            {
                configurator.Register(typeRegistration);

                if (typeRegistration.Name == defaultDatabaseName)
                {
                    Context.Policies.Set<IBuildKeyMappingPolicy>(
                        new BuildKeyMappingPolicy(new NamedTypeBuildKey(typeRegistration.ImplementationType,
                                                                        typeRegistration.Name)),
                        NamedTypeBuildKey.Make<Database>());
                }
            }
        }
    }
}
