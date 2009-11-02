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
using Console.Wpf.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;
using Console.Wpf.ViewModel.BlockSpecifics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Console.Wpf.ViewModel
{

    public class ConfigurationSourceModel
    {
        private readonly IUIServiceWpf uiService;
        private readonly ConfigurationSourceDependency viewModelDependency;
        private readonly ConfigurationSectionLocator sectionLocator;
        private readonly IUnityContainer builder;
        private readonly ObservableCollection<SectionViewModel> sections;
        private readonly ObservableCollection<SectionViewModel> environments;

        public ConfigurationSourceModel(ConfigurationSourceDependency viewModelDependency, ConfigurationSectionLocator sectionLocator, IUnityContainer builder, IUIServiceWpf uiService)
        {
            this.uiService = uiService;
            this.sectionLocator = sectionLocator;
            this.builder = builder;
            this.viewModelDependency = viewModelDependency;
            this.environments = new ObservableCollection<SectionViewModel>();

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
                        sectionViewModel.AfterOpen(configSource);

                        AddSectionViewModel(section.SectionInformation.SectionName, sectionViewModel);
                    }
                }
                catch (Exception e)
                {
                    uiService.ShowError(e, string.Format("Error Loading Section {0}", sectionName));
                }
            }

            Initialize();
        }

        private void Initialize()
        {

            //review: this seems tangled
            var elementLookup = builder.Resolve<ElementLookup>();
            elementLookup.AddCustomElement(builder.Resolve<CustomAttributesPropertyExtender>());

        }

        public void Save(IDesignConfigurationSource configurationSource)
        {
            foreach (var section in Sections)
            {
                section.Save(configurationSource);
            }
        }

        public void New()
        {
            Clear();
            Initialize();
        }

        public void LoadEnvironment(EnvironmentMergeSection environment)
        {
            var environmentSection = SectionViewModel.CreateSection(builder, EnvironmentMergeSection.EnvironmentMergeData, environment);
            environments.Add(environmentSection);
            sections.Add(environmentSection);
        }

        public void NewEnvironment()
        {
            LoadEnvironment(new EnvironmentMergeSection() { EnvironmentName =  "Environment"});
        }

        public void RemoveEnvironment(EnvironmentalOverridesViewModel environnment)
        {
            environments.Remove(environnment);
            sections.Remove(environnment);
        }

        private void Clear()
        {
            sections.Clear();
            environments.Clear();

            viewModelDependency.OnCleared();
        }

        public IEnumerable<SectionViewModel> Environments
        {
            get { return environments; }
        }

        public bool HasSection(string sectionName)
        {
            return Sections.Where(x=>x.SectionName == sectionName).Any();
        }

        public void AddSection(string sectionName, ConfigurationSection defaultSection)
        {
            var sectionViewModel = SectionViewModel.CreateSection(builder, sectionName, defaultSection);
            sectionViewModel.InitializeAsNew();

            AddSectionViewModel(sectionName, sectionViewModel);
        }

        public void RemoveSection(string sectionName)
        {
            SectionViewModel section = Sections.Where(x=>x.SectionName == sectionName).FirstOrDefault();
            if (section != null)
            {
                Sections.Remove(section);
            }
        }

        private void AddSectionViewModel(string sectionName, SectionViewModel sectionViewModel)
        {
            sectionViewModel.UpdateLayout();
            sections.Add(sectionViewModel);
        }

    }
}
