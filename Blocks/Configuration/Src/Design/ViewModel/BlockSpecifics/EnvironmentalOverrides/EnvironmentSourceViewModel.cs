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
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.EnvironmentalOverrides;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;
using Microsoft.Practices.Unity;
using System.IO;
using System.Globalization;
using System.Windows;
using System.Windows.Forms.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Win32;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{

#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class EnvironmentSourceViewModel : SectionViewModel, IElementExtendedPropertyProvider, IDisposable
    {
        readonly ApplicationViewModel applicationModel;  
        readonly SaveMergedEnvironmentConfigurationCommand saveMergedConfigurationCommand;
        readonly SaveEnvironmentConfigurationDeltaCommand saveEnvironmentDeltaCommand;
        readonly ElementLookup elementLookup;
        readonly EnvironmentalOverridesSection environmentSection;
        readonly List<EnvironmentOverriddenElementReference> overriddenElements = new List<EnvironmentOverriddenElementReference>();
        readonly IUIServiceWpf uiService;

        public EnvironmentSourceViewModel(IUnityContainer builder, IUIServiceWpf uiService, ApplicationViewModel sourceModel, ConfigurationSection section)
            : base(builder, EnvironmentalOverridesSection.EnvironmentallyOverriddenProperties, section, new Attribute[] { new EnvironmentalOverridesAttribute(false) })
        {
            this.uiService = uiService;
            this.environmentSection = (EnvironmentalOverridesSection)section;
            this.elementLookup = builder.Resolve<ElementLookup>();
            this.applicationModel = sourceModel;

            foreach (EnvironmentalOverridesElement mergeElement in environmentSection.MergeElements)
            {
                var payload = new EnvironmentOverriddenElementPayload(environmentSection, mergeElement.LogicalParentElementPath);
                var reference = new EnvironmentOverriddenElementReference(elementLookup, environmentSection);
                reference.InitializeWithConfigurationElementPayload(payload);

                overriddenElements.Add(reference);
            }

            saveMergedConfigurationCommand = CreateCommand<SaveMergedEnvironmentConfigurationCommand>(this);
            saveEnvironmentDeltaCommand = CreateCommand<SaveEnvironmentConfigurationDeltaCommand>(this);
        }

        public override void Delete()
        {
            applicationModel.RemoveEnvironment(this);
        }

        public bool SaveDelta()
        {
            if (string.IsNullOrEmpty(applicationModel.ConfigurationFilePath))
            {
                var saveDialogResult = uiService.ShowMessageWpf(
                    DesignResources.SaveDeltaUnsavedMainConfigurationMessage,
                    DesignResources.SaveDeltaDialogTitle,
                    System.Windows.MessageBoxButton.OKCancel);

                if (saveDialogResult == System.Windows.MessageBoxResult.Cancel)
                {
                    return false;
                }
                if (!applicationModel.SaveMainConfiguration())
                {
                    return false;
                }
            }

            if (String.IsNullOrEmpty(EnvironmentDeltaFile))
            {
                SaveFileDialog saveEnvironmentDeltaDialog = new SaveFileDialog
                {
                    Filter = DesignResources.DeltaDialogFilter,
                    Title = DesignResources.SaveDeltaDialogTitle
                };

                var dialogResult = uiService.ShowFileDialog(saveEnvironmentDeltaDialog);
                if (dialogResult.DialogResult != true)
                {
                    return false;
                }
                EnvironmentDeltaFile = dialogResult.FileName;
            }

            string path = EnvironmentDeltaFile;
            if (!System.IO.Path.IsPathRooted(path))
            {
                string mainDirectory = System.IO.Path.GetDirectoryName(applicationModel.ConfigurationFilePath);
                path = System.IO.Path.Combine(mainDirectory, path);
            }

            if (File.Exists(path) && !String.Equals(LastEnvironmentDeltaSavedFilePath, path))
            {
                string overwriteDeltaConfigurationFileWarning = string.Format(CultureInfo.CurrentCulture, DesignResources.SaveDeltaOverwriteExistingFile, path);
                var overwriteDeltaConfigurationFileWarningResult = uiService.ShowMessageWpf(overwriteDeltaConfigurationFileWarning, DesignResources.SaveDeltaDialogTitle, MessageBoxButton.OKCancel);

                if (overwriteDeltaConfigurationFileWarningResult == MessageBoxResult.Cancel)
                {
                    return false;
                }
            }
            if (applicationModel.EnsureCanSaveConfigurationFile(path))
            {
                DesignConfigurationSource source = new DesignConfigurationSource(path);
                Save(source);

                LastEnvironmentDeltaSavedFilePath = path;
                return true;
            }
            return false;
        }

        protected override IEnumerable<CommandModel> GetAllCommands()
        {
            return base.GetAllCommands().Union(new CommandModel[] { saveMergedConfigurationCommand, saveEnvironmentDeltaCommand });
        }

        protected override IEnumerable<Property> GetAllProperties()
        {
            return base.GetAllProperties().Union(new Property[] { ContainingSection.CreateProperty<EnvironmentSourceViewModelDeltaFileProperty>() }); ;
        }

        public string EnvironmentName
        {
            get { return environmentSection.EnvironmentName; }
        }

        public string LastEnvironmentDeltaSavedFilePath
        {
            get;
            set;
        }

        public string EnvironmentDeltaFile
        {
            get { return (string)Property("EnvironmentDeltaFile").Value; }
            set { Property("EnvironmentDeltaFile").Value = value; }
        }

        public string EnvironmentConfigurationFile
        {
            get { return (string)Property("EnvironmentConfigurationFile").Value; }
            set { Property("EnvironmentConfigurationFile").Value = value; }
        }

        public bool CanExtend(ElementViewModel subject)
        {
            if (subject.Attributes.OfType<EnvironmentalOverridesAttribute>().Where(x=>!x.CanOverride).Any())
            {
                return false;
            }

            if (!subject.IsElementPathReliableXPath)
            {
                return false;
            }

            return subject.Properties.Where(CanOverrideProperty).Count() > 0;
        }

        public static bool CanOverrideProperty(Property subjectProperty)
        {

            if (subjectProperty.Attributes.OfType<EnvironmentalOverridesAttribute>().Where(x => !x.CanOverride).Any()) return false;

            return (subjectProperty is IEnvironmentalOverridesProperty && ((IEnvironmentalOverridesProperty)subjectProperty).SupportsOverride);
        }

        public IEnumerable<Property> GetExtendedProperties(ElementViewModel subject)
        {
            var overriddenElementReference = overriddenElements.Where(x => x.ElementId == subject.ElementId).FirstOrDefault();
            if (overriddenElementReference == null)
            {
                overriddenElementReference = new EnvironmentOverriddenElementReference(elementLookup, environmentSection);
                overriddenElementReference.InitializeWithElementViewModel(subject);
                overriddenElements.Add(overriddenElementReference);
            }

            Type environmentalOverriddenElementProperty = typeof(EnvironmentOverriddenElementProperty);
            var overridesAttribute = subject.Attributes.OfType<EnvironmentalOverridesAttribute>().FirstOrDefault();
            if (overridesAttribute != null && overridesAttribute.CustomOverridesPropertyType != null)
            {
                environmentalOverriddenElementProperty = overridesAttribute.CustomOverridesPropertyType;
            }

            yield return ContainingSection.CreateProperty(environmentalOverriddenElementProperty,
                new ParameterOverride("environmentModel", this),
                new ParameterOverride("subject", subject),
                new DependencyOverride<EnvironmentOverriddenElementPayload>(new InjectionParameter<EnvironmentOverriddenElementPayload>(overriddenElementReference.EnvironmentOverriddenElementPayload)));
        }

        private class EnvironmentSourceViewModelDeltaFileProperty : CustomProperty<string>, ILogicalPropertyContainerElement
        {
            public EnvironmentSourceViewModelDeltaFileProperty(IServiceProvider serviceProvider)
                : base(serviceProvider, "EnvironmentDeltaFile", new Attribute[]
                {
                    new ResourceDisplayNameAttribute(typeof(DesignResources), "OverridesDeltaFileDisplayName"),
                    new ValidationAttribute(typeof(RequiredFieldValidator)),
                    new ValidationAttribute(typeof(FilePathExistsValidator)),
                    new ValidationAttribute(typeof(EnvironmentalOverridesSectionDeltaFileValidator)),
                    new EditorAttribute(CommonDesignTime.EditorTypes.FilteredFilePath, CommonDesignTime.EditorTypes.UITypeEditor),
                    new FilteredFileNameEditorAttribute(typeof(Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties.Resources), "EnvironmentDeltaFileFilter") { CheckFileExists = false }
                })
            {
            }

            public ElementViewModel ContainingElement
            {
                get { return ContainingSection; }
            }

            public string ContainingElementDisplayName
            {
                get { return ContainingSection.Name; }
            }
        }

        #region IDisposable Members

        protected override void Dispose(bool disposing)
        {

            foreach(IDisposable item in overriddenElements)
            {
                item.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion
    }

#pragma warning restore 1591
}

