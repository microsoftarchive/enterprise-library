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

using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands
{
    /// <summary>
    /// Command Model implementation that will add a provider to the designer and ensures that its block dependency is available.<br/>
    /// This Command Model can be used by annotating a providers configuration element with the <see cref="AddSateliteProviderCommandAttribute"/> attribute.<br/>
    /// </summary>
    public class AddSatelliteProviderCommand : DefaultCollectionElementAddCommand
    {
        readonly MenuCommandService commandService;
        readonly AddSateliteProviderCommandAttribute commandAttribute;
        readonly ElementLookup lookup;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddSatelliteProviderCommand"/> class.
        /// </summary>
        /// <remarks>
        /// This class is intended to be initialized by the <see cref="SectionViewModel.CreateElementCollectionAddCommands"/>.
        /// </remarks>
        /// <param name="commandAttribute">The <see cref="AddSateliteProviderCommandAttribute"/> that specifes metadata for this <see cref="AddSatelliteProviderCommand"/> to be initialized with.</param>
        /// <param name="collection"></param>
        /// <param name="commandService"></param>
        /// <param name="configurationElementType"></param>
        /// <param name="lookup"></param>
        /// <param name="uiService"></param>
        public AddSatelliteProviderCommand(AddSateliteProviderCommandAttribute commandAttribute, MenuCommandService commandService, ConfigurationElementType configurationElementType, ElementCollectionViewModel collection, ElementLookup lookup, IUIServiceWpf uiService)
            : base(commandAttribute, configurationElementType, collection, uiService)
        {
            this.commandService = commandService;
            this.commandAttribute = commandAttribute;
            this.lookup = lookup;
        }

        /// <summary>
        /// The method invoked during execution.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.
        ///                 </param>
        protected override void InnerExecute(object parameter)
        {
            base.InnerExecute(parameter);

            commandService.ExecuteAddBlockForSection(commandAttribute.SectionName);

            if (commandAttribute.DefaultProviderConfigurationType != null)
            {
                var declaringElement = lookup.FindInstancesOfConfigurationType(commandAttribute.DefaultProviderConfigurationType).FirstOrDefault();
                if (declaringElement != null)
                {
                    object defaultProviderValue = declaringElement.Property(commandAttribute.DefaultProviderConfigurationPropertyName).Value;
                    AddedElementViewModel.Property(commandAttribute.SateliteProviderReferencePropertyName).Value = defaultProviderValue;
                }
            }
        }
    }
}
