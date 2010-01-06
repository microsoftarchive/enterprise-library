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
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
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
        ConfigurationSourceModel configurationModel;
        Type configurationSectionType;
        IApplicationModel applicationModel;

        /// <summary>
        /// Intializes a new instance of the <see cref="AddApplicationBlockCommand"/> class.
        /// </summary>
        /// <remarks>
        /// TODO: This class is intended to be intialized by the XXXX 
        /// </remarks>
        /// <param name="configurationModel">The <see cref="ConfigurationSourceModel"/> instance that represents the configuration source that is currently edited.</param>
        /// <param name="commandAttribute">The <see cref="AddApplicationBlockCommandAttribute"/> that specifes metadata for this <see cref="AddApplicationBlockCommand"/> to be initialized with.</param>
        public AddApplicationBlockCommand(ConfigurationSourceModel configurationModel, AddApplicationBlockCommandAttribute commandAttribute)
            : base(commandAttribute)
        {
            this.configurationModel = configurationModel;
            this.configurationModel.Sections.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Sections_CollectionChanged);
            this.sectionName = commandAttribute.SectionName;
            this.configurationSectionType = commandAttribute.ConfigurationSectionType;
        }

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
        /// When overriden in a derived class, returns the intial configuration schema that should be added to the <see cref="ConfigurationSourceModel"/>.
        /// </summary>
        protected virtual ConfigurationSection CreateConfigurationSection()
        {
            return (ConfigurationSection)Activator.CreateInstance(configurationSectionType);
        }

        public override bool CanExecute(object parameter)
        {
            return !configurationModel.HasSection(sectionName);
        }

        protected SectionViewModel AddedSection
        {
            get;
            set;
        }

        public override void Execute(object parameter)
        {
            AddedSection = configurationModel.AddSection(sectionName, CreateConfigurationSection());
            AddedSection.IsExpanded = true;
            AddedSection.IsSelected = true;

            applicationModel.SetDirty();
            OnCanExecuteChanged();
        }
    }
}
