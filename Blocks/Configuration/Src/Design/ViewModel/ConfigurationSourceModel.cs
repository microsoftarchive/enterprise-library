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
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// The <see cref="ConfigurationSourceModel"/> represents a single <see cref="IConfigurationSource"/> that is
    /// presented in the design-time.
    /// </summary>
    public class ConfigurationSourceModel
    {
        private readonly IUnityContainer builder;
        private readonly SaveOperation saveOperation;
        private readonly ElementLookup lookup;
        private readonly ObservableCollection<SectionViewModel> sections;
        private readonly ReadOnlyObservableCollection<SectionViewModel> readOnlySectionView;
        private readonly IUIServiceWpf uiService;
        private readonly ConfigurationSourceDependency viewModelDependency;

        /// <summary>
        /// Initializes a new <see cref="ConfigurationSourceModel"/>.
        /// </summary>
        /// <param name="lookup">The <see cref="ElementLookup"/> service used for locating elements.</param>
        /// <param name="viewModelDependency">The <see cref="ConfigurationSourceDependency"/> for notifying others of changes in this configuration source.</param>
        /// <param name="builder">The container for building new objects.</param>
        /// <param name="uiService">The user-interface service for presenting dialogs and windows to the user.</param>
        /// <param name="saveOperation">Save operation integration with a host environment.</param>
        public ConfigurationSourceModel(ElementLookup lookup, ConfigurationSourceDependency viewModelDependency, IUnityContainer builder, IUIServiceWpf uiService, SaveOperation saveOperation)
        {
            this.uiService = uiService;
            this.builder = builder;
            this.viewModelDependency = viewModelDependency;
            this.lookup = lookup;
            this.saveOperation = saveOperation;
            sections = new ObservableCollection<SectionViewModel>();
            readOnlySectionView = new ReadOnlyObservableCollection<SectionViewModel>(sections);
        }

        /// <summary>
        /// Gets the collection of <see cref="SectionViewModel"/> elements contained by this <see cref="ConfigurationSourceModel"/>.
        /// </summary>
        public ReadOnlyObservableCollection<SectionViewModel> Sections
        {
            get
            {
                return readOnlySectionView;
            }
        }

        /// <summary>
        /// Loads a <see cref="ConfigurationSourceModel"/> from the <see cref="IDesignConfigurationSource"/>.
        /// </summary>
        /// <param name="configSource">The <see cref="IDesignConfigurationSource"/> to load this <see cref="ConfigurationSourceModel"/> from.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the load was successful.
        /// Returns <see langword="false"/>, if any exceptions occurredduring the load.
        /// </returns>
        /// <remarks>
        /// Any exceptions that occur during the load are logged to the <see cref="ConfigurationLogWriter"/> and also
        /// displayed to the user via the <see cref="IUIServiceWpf"/>.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public bool Load(IDesignConfigurationSource configSource)
        {
            Clear();
            Initialize();

            IDesignConfigurationSource sectionConfigurationSource = null;
            try
            {
                sectionConfigurationSource = GetSectionSource(configSource);
            }
            catch (Exception ex)
            {
                ConfigurationLogWriter.LogException(ex);
                uiService.ShowMessageWpf(string.Format(CultureInfo.CurrentCulture, Resources.ErrorLoadingConfigSourceFile, ex.Message),
                                            Resources.ErrorTitle,
                                         MessageBoxButton.OK);
                return false;
            }

            if (sectionConfigurationSource != null)
            {
                try
                {
                    AddSection(ConfigurationSourceSection.SectionName,
                               configSource.GetLocalSection(ConfigurationSourceSection.SectionName),
                               new InitializeContext(configSource));

                    LoadSectionsFromSource(sectionConfigurationSource);
                }
                catch
                {
                    var disposable = sectionConfigurationSource as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }

                    throw;
                }
            }
            else
            {
                LoadSectionsFromSource(configSource);
            }


            Validate();

            return true;
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void LoadSectionsFromSource(IDesignConfigurationSource sectionConfigurationSource)
        {
            var locator = builder.Resolve<ConfigurationSectionLocator>();
            foreach (var sectionName in locator.ConfigurationSectionNames)
            {
                try
                {
                    var section = sectionConfigurationSource.GetLocalSection(sectionName);
                    if (section != null)
                    {
                        AddSection(sectionName, section, new InitializeContext(sectionConfigurationSource));
                    }
                }
                catch (Exception e)
                {
                    uiService.ShowError(e, string.Format(CultureInfo.CurrentCulture, Resources.ErrorCouldNotLoadSection, sectionName));
                }
            }
        }

        private static IDesignConfigurationSource GetSectionSource(IDesignConfigurationSource mainConfigurationSource)
        {
            var configSourceSection =
                mainConfigurationSource.GetLocalSection(ConfigurationSourceSection.SectionName) as ConfigurationSourceSection;

            return GetSelectedConfigSource(configSourceSection, mainConfigurationSource);
        }

        private IDesignConfigurationSource GetSectionSourceFromModel(IDesignConfigurationSource mainConfigurationSource)
        {
            var configurationSourceSection = Sections
                .Where(s => typeof(ConfigurationSourceSection).IsAssignableFrom(s.ConfigurationType))
                .OfType<ConfigurationSourceSectionViewModel>()
                .Select(m => m.ConfigurationElement)
                .Cast<ConfigurationSourceSection>()
                .FirstOrDefault();

            return GetSelectedConfigSource(configurationSourceSection, mainConfigurationSource);
        }

        private static IDesignConfigurationSource GetSelectedConfigSource(ConfigurationSourceSection configSourceSection, IDesignConfigurationSource mainConfigurationSource)
        {
            if (configSourceSection == null) return null;

            var selectedSource =
                configSourceSection.Sources.Where(s => s.Name == configSourceSection.SelectedSource).
                    FirstOrDefault();

            return (selectedSource == null ? null : selectedSource.CreateDesignSource(mainConfigurationSource));
        }

        private void Validate()
        {
            foreach (var section in Sections)
            {
                section.Validate();
            }
        }

        private void Initialize()
        {
            lookup.AddCustomElement(builder.Resolve<CustomAttributesPropertyExtender>());
        }

        /// <summary>
        /// Saves the <see cref="ConfigurationSourceModel"/> to a <see cref="IDesignConfigurationSource"/>.
        /// </summary>
        /// <param name="configurationSource">The <see cref="IDesignConfigurationSource"/> to save to.</param>
        /// <returns>Returns <see langword="true"/> if the save was successful.
        /// Returns <see langword="false"/> if errors occurredduring the save.
        /// </returns>
        /// <remarks>
        /// Errors that occur during save will be displayed logged using <see cref="ConfigurationLogWriter"/> and displayed 
        /// to the user through the <see cref="IUIServiceWpf"/> service.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public bool Save(IDesignConfigurationSource configurationSource)
        {
            saveOperation.BeginPerformSaveOperation();
            try
            {
                var sectionsToSave = Sections.Where(x => null == x as EnvironmentSourceViewModel);

                var sectionSaveSource = GetSectionSourceFromModel(configurationSource);
                if (sectionSaveSource != null)
                {
                    try
                    {
                        try
                        {
                            SaveConfigurationSource(configurationSource);
                        }
                        catch (Exception e)
                        {
                            ConfigurationLogWriter.LogException(e);
                            uiService.ShowError(e, Resources.ErrorSavingConfigurationSourceOnMainFile);
                            return false;
                        }
                        try
                        {
                            SaveSections(
                                sectionsToSave.Where(
                                    x => !typeof(ConfigurationSourceSection).IsAssignableFrom(x.ConfigurationType)),
                                sectionSaveSource);
                        }
                        catch (Exception e)
                        {
                            ConfigurationLogWriter.LogException(e);
                            uiService.ShowError(e, Resources.ErrorSavingConfigurationSectionsOnSelectedSource);
                            return false;
                        }
                        return true;
                    }
                    finally
                    {
                        var disposable = sectionSaveSource as IDisposable;
                        if (disposable != null) disposable.Dispose();
                    }
                }

                try
                {
                    SaveSections(sectionsToSave, configurationSource);
                }
                catch (Exception e)
                {
                    ConfigurationLogWriter.LogException(e);
                    uiService.ShowError(e, Resources.ErrorSavingConfigurationSectionsOnMainFile);
                    return false;
                }
            }
            finally
            {
                saveOperation.EndPerformSaveOperation();
            }
            return true;
        }

        private void SaveConfigurationSource(IDesignConfigurationSource configurationSource)
        {
            var configurationSourceSectionModel = Sections
                .Where(s => typeof(ConfigurationSourceSection).IsAssignableFrom(s.ConfigurationType))
                .OfType<ConfigurationSourceSectionViewModel>()
                .Cast<ConfigurationSourceSectionViewModel>()
                .First();

            ClearConfigurationSections(configurationSource);
            configurationSourceSectionModel.Save(configurationSource);
        }

        private void ClearConfigurationSections(IDesignConfigurationSource source)
        {
            var locator = builder.Resolve<ConfigurationSectionLocator>();
            foreach (var sectionName in locator.ClearableConfigurationSectionNames)
            {
                source.RemoveLocalSection(sectionName);
            }
        }

        private void SaveSections(IEnumerable<SectionViewModel> sectionsToSave, IDesignConfigurationSource source)
        {
            ClearConfigurationSections(source);

            foreach (var section in sectionsToSave)
            {
                section.Save(source);
            }
        }

        /// <summary>
        /// Clears the existing configuration source and readies it to begin a new configuration.
        /// </summary>
        public void New()
        {
            Clear();
            Initialize();
        }

        private void Clear()
        {
            foreach (var section in sections)
            {
                lookup.RemoveSection(section);
            }

            viewModelDependency.OnCleared();

            foreach(var section in sections)
            {
                section.Dispose();
            }
            sections.Clear();
        }

        /// <summary>
        /// Determines if this <see cref="ConfigurationSourceModel"/> has a section by a given name.
        /// </summary>
        /// <param name="sectionName">The section name to seek.</param>
        /// <returns>
        /// Returns <see langword="true"/> if a section with a <see cref="SectionViewModel.SectionName"/> matching <paramref name="sectionName"/> exists.
        /// Otherwise, returns <see langword="false"/>
        /// </returns>
        public bool HasSection(string sectionName)
        {
            return Sections.Where(x => x.SectionName == sectionName).Any();
        }

        /// <summary>
        /// Adds a <see cref="ConfigurationSection"/> to the <see cref="ConfigurationSourceModel"/>.
        /// </summary>
        /// <param name="sectionName">The section name to use when adding.</param>
        /// <param name="section">The <see cref="ConfigurationSection"/> to add.</param>
        /// <returns>The <see cref="SectionViewModel"/> for the added section.</returns>
        public SectionViewModel AddSection(string sectionName, ConfigurationSection section)
        {
            return AddSection(sectionName, section, new InitializeContext(null));
        }

        private SectionViewModel AddSection(string sectionName, ConfigurationSection section,
                                            InitializeContext initializeContext)
        {
            var sectionViewModel = SectionViewModel.CreateSection(builder, sectionName, section);
            SectionInitializer.InitializeSection(sectionViewModel, initializeContext);
            lookup.AddSection(sectionViewModel);
            sections.Add(sectionViewModel);

            return sectionViewModel;
        }

        /// <summary>
        /// Removes a section with the given name.
        /// </summary>
        /// <param name="sectionName">The name of the section to remove.</param>
        /// <remarks>
        /// If the section does not exist, then it does nothing.
        /// </remarks>
        public void RemoveSection(string sectionName)
        {
            var section = Sections.Where(x => x.SectionName == sectionName).FirstOrDefault();
            if (section != null)
            {
                lookup.RemoveSection(section);
                sections.Remove(section);
                section.Dispose();
            }
        }

        /// <summary>
        /// Returns a value indicating the <see cref="ConfigurationSourceModel"/> is valid for saving.
        /// </summary>
        /// <returns>
        /// Returns <see langword="true"/> when the sections and elements have no validation results that are <see cref="ValidationResult.IsError"/>.
        /// Otherwise, returns <see langword="false"/>.
        /// </returns>
        public bool IsValidForSave()
        {
            Validate();
            var allElements = sections.SelectMany(
                s => s.DescendentElements()
                         .Union(new ElementViewModel[] {s}));

            var errors = allElements
                .SelectMany(e => e.Properties)
                .SelectMany(p => p.ValidationResults)
                .Union(allElements.SelectMany(v => v.ValidationResults));

            return !errors.Any(e => e.IsError);
        }
    }

    class SectionInitializer
    {
        public static void InitializeSection(SectionViewModel section, InitializeContext context)
        {
            //first init section
            section.Initialize(context);

            // then initialize elements
            var elements = section.DescendentElements().OfType<ElementViewModel>();
            foreach (var element in elements)
            {
                element.Initialize(context);
            }

            //then init properties
            var sectionPropertiesThatNeedInitialization = section.Properties;
            var propertiesOfContainedElementsThatNeedInitialization =
                section.DescendentElements().SelectMany(x => x.Properties);

            foreach (var propertyThatNeedsInitialization in
                sectionPropertiesThatNeedInitialization.Concat(
                    propertiesOfContainedElementsThatNeedInitialization))
            {
                propertyThatNeedsInitialization.Initialize(context);
            }
        }
    }
}
