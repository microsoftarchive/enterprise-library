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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration
{
    public class SectionBuilder {}

    public static class SectionBuilderExtensions
    {
        public static LocatorSectionBuilder LocatorSection(this SectionBuilder builder)
        {
            return new LocatorSectionBuilder();
        }
    }

    public class LocatorSectionBuilder : IProviderName
    {
        private readonly TypeRegistrationProvidersConfigurationSection section =
            new TypeRegistrationProvidersConfigurationSection();

        private TypeRegistrationProviderSettings currentSettings;

        LocatorSectionBuilder IProviderName.WithProviderName(string name)
        {
            currentSettings.Name = name;
            section.TypeRegistrationProviders.Add(currentSettings);
            return this;
        }

        public IProviderName AddConfigSection(string sectionName)
        {
            currentSettings = new TypeRegistrationProviderSettings {SectionName = sectionName};
            return this;
        }

        public IProviderName AddProvider<TProvider>()
        {
            currentSettings = new TypeRegistrationProviderSettings
            {
                ProviderTypeName = typeof (TProvider).AssemblyQualifiedName
            };
            return this;
        }

        public LocatorSectionBuilder RemoveProviderNamed(string name)
        {
            section.TypeRegistrationProviders.Remove(name);
            return this;
        }
        
        public void AddTo(IConfigurationSource configurationSource)
        {
            configurationSource.Add(null, 
                TypeRegistrationProvidersConfigurationSection.SectionName,
                section);
        }
    }

    public interface IProviderName
    {
        LocatorSectionBuilder WithProviderName(string name);
    }
}
