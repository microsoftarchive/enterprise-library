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
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Configuration;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{

    public class ConfigurationSourceModel
    {
        private readonly IUIServiceWpf uiService;
        private readonly ConfigurationSourceDependency viewModelDependency;
        private readonly ConfigurationSectionLocator sectionLocator;
        private readonly IUnityContainer builder;
        private readonly ObservableCollection<SectionViewModel> sections;
        private readonly ObservableCollection<EnvironmentalOverridesViewModel> environments;
        private readonly ElementLookup lookup;

        public ConfigurationSourceModel(ElementLookup lookup, ConfigurationSourceDependency viewModelDependency, ConfigurationSectionLocator sectionLocator, IUnityContainer builder, IUIServiceWpf uiService)
        {
            this.uiService = uiService;
            this.sectionLocator = sectionLocator;
            this.builder = builder;
            this.viewModelDependency = viewModelDependency;
            this.lookup = lookup;
            this.environments = new ObservableCollection<EnvironmentalOverridesViewModel>();

            sections = new ObservableCollection<SectionViewModel>();
        }

        public ObservableCollection<SectionViewModel> Sections
        {
            get { return sections; }
        }

        public void Load(IDesignConfigurationSource configSource)
        {
            Clear();

            var locator = builder.Resolve<ConfigurationSectionLocator>();

            foreach (var sectionName in locator.ConfigurationSectionNames)
            {
                try
                {
                    ConfigurationSection section = configSource.GetLocalSection(sectionName);
                    if (section != null)
                    {
                        var sectionViewModel = SectionViewModel.CreateSection(builder, sectionName, section);
                        
                        InitializeSection(sectionViewModel, new InitializeContext(configSource));
                        sections.Add(sectionViewModel);

                    }
                }
                catch (Exception e)
                {
                    uiService.ShowError(e, string.Format("Error Loading Section {0}", sectionName));
                }
            }

            foreach (var section in sections)
            {
                section.UpdateLayout();
            }
        }

        private void Initialize()
        {
            //review: this seems tangled
            var elementLookup = builder.Resolve<ElementLookup>();
            elementLookup.AddCustomElement(builder.Resolve<CustomAttributesPropertyExtender>());
        }

        private void InitializeSection(SectionViewModel section, InitializeContext context)
        {
            //first init section
            INeedInitialization sectionInitialize = section as INeedInitialization;
            if (sectionInitialize != null)
            {
                sectionInitialize.Initialize(context);
            }

            //then init properties
            var sectionPropertiesThatNeedInitialization = section.Properties.OfType<INeedInitialization>();
            var propertiesOfContainedElementsThatNeedInitialization = section.DescendentElements().SelectMany(x => x.Properties).OfType<INeedInitialization>();

            foreach (var propertyThatNeedsInitialization in 
                    sectionPropertiesThatNeedInitialization.Union(
                        propertiesOfContainedElementsThatNeedInitialization))
            {
                propertyThatNeedsInitialization.Initialize(context);
            }
        }

        public void Save(IDesignConfigurationSource configurationSource)
        {
            var locator = builder.Resolve<ConfigurationSectionLocator>();
            foreach (string sectionName in locator.ConfigurationSectionNames)
            {
                configurationSource.RemoveLocalSection(sectionName);
            }
            foreach (var section in Sections.Where( x=>null == x as EnvironmentalOverridesViewModel))
            {
                section.Save(configurationSource);
            }

            foreach (EnvironmentalOverridesViewModel envrionment in Environments)
            {
                envrionment.SaveDelta();
            }
        }

        public void New()
        {
            Clear();
            Initialize();
        }

        public void LoadEnvironment(EnvironmentMergeSection environment, string environmentDeltaFile)
        {
            EnvironmentalOverridesViewModel environmentSection = (EnvironmentalOverridesViewModel)SectionViewModel.CreateSection(builder, EnvironmentMergeSection.EnvironmentMergeData, environment);
            environmentSection.EnvironmentDeltaFile = environmentDeltaFile;

            environments.Add(environmentSection);
            sections.Add(environmentSection);

            InitializeSection(environmentSection, new InitializeContext(null));

        }

        public void NewEnvironment()
        {
            LoadEnvironment(new EnvironmentMergeSection() { EnvironmentName =  "Environment"}, string.Empty);
        }

        public void RemoveEnvironment(EnvironmentalOverridesViewModel environnment)
        {
            environments.Remove(environnment);
            lookup.RemoveSection(environnment);
            sections.Remove(environnment);
        }

        private void Clear()
        {
            sections.Clear();
            environments.Clear();

            viewModelDependency.OnCleared();
        }

        public IEnumerable<EnvironmentalOverridesViewModel> Environments
        {
            get { return environments; }
        }

        public bool HasSection(string sectionName)
        {
            return Sections.Where(x=>x.SectionName == sectionName).Any();
        }

        public void AddSection(string sectionName, ConfigurationSection section)
        {
            var sectionViewModel = SectionViewModel.CreateSection(builder, sectionName, section);

            InitializeSection(sectionViewModel, new InitializeContext(null));

            sectionViewModel.UpdateLayout();
            sections.Add(sectionViewModel);
        }

        public void RemoveSection(string sectionName)
        {
            SectionViewModel section = Sections.Where(x=>x.SectionName == sectionName).FirstOrDefault();
            if (section != null)
            {
                lookup.RemoveSection(section);
                sections.Remove(section);
            }
            
        }

    }
}
