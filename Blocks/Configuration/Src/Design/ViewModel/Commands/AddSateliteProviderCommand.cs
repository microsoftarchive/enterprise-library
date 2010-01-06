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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands
{
    /// <summary>
    /// Command Model implementation that will add a provider to the designer and ensures that its block dependency is available.<br/>
    /// This Command Model can be used by annotating a providers configuration element with the <see cref="AddSateliteProviderCommandAttribute"/> attribute.<br/>
    /// </summary>
    public class AddSateliteProviderCommand : DefaultCollectionElementAddCommand
    {
        readonly MenuCommandService commandService;
        readonly AddSateliteProviderCommandAttribute commandAttribute;
        readonly ElementLookup lookup;

        /// <summary>
        /// Intializes a new instance of the <see cref="AddSateliteProviderCommand"/> class.
        /// </summary>
        /// <remarks>
        /// This class is intended to be intialized by the <see cref="SectionViewModel.CreateElementCollectionAddCommands"/>.
        /// </remarks>
        /// <param name="commandAttribute">The <see cref="AddSateliteProviderCommandAttribute"/> that specifes metadata for this <see cref="AddSateliteProviderCommand"/> to be initialized with.</param>
        /// <param name="collection"></param>
        /// <param name="commandService"></param>
        /// <param name="configurationElementType"></param>
        public AddSateliteProviderCommand(AddSateliteProviderCommandAttribute commandAttribute, MenuCommandService commandService, ConfigurationElementType configurationElementType, ElementCollectionViewModel collection, ElementLookup lookup)
            : base(commandAttribute, configurationElementType, collection)
        {
            this.commandService = commandService;
            this.commandAttribute = commandAttribute;
            this.lookup = lookup;
        }

        public override void Execute(object parameter)
        {
            base.Execute(parameter);

            commandService.ExecuteAddBlockForSection(commandAttribute.SectionName);

            if (commandAttribute.DefaultProviderConfigurationType != null)
            {
                var declaringElement = lookup.FindInstancesOfConfigurationType(commandAttribute.DefaultProviderConfigurationType).FirstOrDefault();
                if (declaringElement != null)
                {
                    string defaultProvider = declaringElement.Property(commandAttribute.DefaultProviderConfigurationPropertyName).BindableProperty.BindableValue;
                    AddedElementViewModel.Property(commandAttribute.SateliteProviderReferencePropertyName).BindableProperty.BindableValue = defaultProvider;
                }
            }
        }
    }
}
