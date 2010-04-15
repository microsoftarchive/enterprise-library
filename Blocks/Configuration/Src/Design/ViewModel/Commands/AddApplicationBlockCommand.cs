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
using System.Collections.Specialized;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands
{
    /// <summary>
    /// Command Model implementation that will add a new block to the designer.<br/>
    /// This Command Model can be added by annotating an assembly with an <see cref="AddApplicationBlockCommandAttribute"/> attribute.<br/>
    /// </summary>
    public class AddApplicationBlockCommand : CommandModel
    {
        string sectionName;
        Type configurationSectionType;
        private IApplicationModel applicationModel;
        private ConfigurationSourceModel configurationModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddApplicationBlockCommand"/> class.
        /// </summary>
        /// <param name="configurationModel">The <see cref="ConfigurationSourceModel"/> instance that represents the configuration source that is currently edited.</param>
        /// <param name="commandAttribute">The <see cref="AddApplicationBlockCommandAttribute"/> that specifes metadata for this <see cref="AddApplicationBlockCommand"/> to be initialized with.</param>
        /// <param name="uiService"></param>
        public AddApplicationBlockCommand(ConfigurationSourceModel configurationModel, AddApplicationBlockCommandAttribute commandAttribute, IUIServiceWpf uiService)
            : base(commandAttribute, uiService)
        {
            this.configurationModel = configurationModel;
            INotifyCollectionChanged collectionChanged = configurationModel.Sections;
            collectionChanged.CollectionChanged += Sections_CollectionChanged;
            this.sectionName = commandAttribute.SectionName;
            this.configurationSectionType = commandAttribute.ConfigurationSectionType;
        }

        ///<summary>
        /// Initialization method for additional dependencies not specified by the constructor.
        ///</summary>
        ///<param name="applicationModel">The design-time application model.</param>
        [InjectionMethod]
        public void AddApplicationBlockCommandInitialize(IApplicationModel applicationModel)
        {
            this.applicationModel = applicationModel;
        }

        /// <summary>
        /// Gets the name of the section that should be added to the <see cref="ConfigurationSourceModel"/>.
        /// </summary>
        public string SectionName
        {
            get { return sectionName; }
        }

        void Sections_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }

        /// <summary>
        /// When overridden in a derived class, returns the intial configuration schema that should be added to the <see cref="ConfigurationSourceModel"/>.
        /// </summary>
        protected virtual ConfigurationSection CreateConfigurationSection()
        {
            return (ConfigurationSection)Activator.CreateInstance(configurationSectionType);
        }

        /// <summary>
        /// Protected method for determinging if command can execute.
        /// </summary>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.
        ///                 </param>
        protected override bool InnerCanExecute(object parameter)
        {
            return !configurationModel.HasSection(sectionName);
        }

        /// <summary>
        /// The section added during <see cref="InnerExecute"/>
        /// </summary>
        protected SectionViewModel AddedSection { get; private set; }


        /// <summary>
        /// Creates and adds a new section to the <see cref="ConfigurationSourceModel"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <remarks>
        /// After the section has been created <see cref="CreateConfigurationSection"/>, the <see cref="SectionViewModel"/> 
        /// may be modified during the <see cref="AfterSectionAdded"/> template method.
        /// </remarks>
        protected override void InnerExecute(object parameter)
        {
            AddedSection = configurationModel.AddSection(sectionName, CreateConfigurationSection());
            AfterSectionAdded();

            applicationModel.SetDirty();
            OnCanExecuteChanged();
        }

        /// <summary>
        /// Provides the opportunity to modify the section immediately after it's been added to the <see cref="ConfigurationSourceModel"/>.
        /// </summary>
        protected virtual void AfterSectionAdded()
        {
            AddedSection.IsExpanded = true;
            AddedSection.IsSelected = true;
        }
    }
}
