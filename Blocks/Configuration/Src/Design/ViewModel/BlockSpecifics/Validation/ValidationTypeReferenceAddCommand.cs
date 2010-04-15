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
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
#pragma warning disable 1591

    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class ValidationTypeReferenceAddCommand : TypePickingCollectionElementAddCommand
    {
        public ValidationTypeReferenceAddCommand(IUIServiceWpf uiService, IAssemblyDiscoveryService discoveryService, TypePickingCommandAttribute commandAttribute, ConfigurationElementType configurationElementType, ElementCollectionViewModel elementCollectionModel)
            : base(uiService, discoveryService, commandAttribute, configurationElementType, elementCollectionModel)
        { }

        protected override void SetProperties(ElementViewModel createdElement, Type selectedType)
        {
            createdElement.Property("Name").Value = selectedType.FullName;
            createdElement.Property("AssemblyName").Value = selectedType.Assembly.GetName().FullName;
        }

        protected override bool AfterSelectType(Type selectedType)
        {
            var result = this.ElementCollectionModel.ChildElements;

            var count = result.Where(x => ((ValidatedTypeReference)x.ConfigurationElement).AssemblyName == selectedType.Assembly.FullName
                && ((ValidatedTypeReference)x.ConfigurationElement).Name == selectedType.FullName).Count();

            if (count > 0)
            {
                this.UIService.ShowMessageWpf(
                    string.Format(CultureInfo.CurrentCulture, Resources.DuplicateValidationTypeMessage, selectedType.Name),
                    Resources.DuplicationValidationTypeTitle,
                    System.Windows.MessageBoxButton.OK);
                return false;
            }

            return true;
        }
    }
#pragma warning restore 1591
}
