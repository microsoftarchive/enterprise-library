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
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class ExceptionTypeAddCommand : TypePickingCollectionElementAddCommand
    {
        IUIServiceWpf uiService;

        public ExceptionTypeAddCommand(IUIServiceWpf uiService, TypePickingCommandAttribute commandAttribute, ConfigurationElementType configurationElementType, ElementCollectionViewModel elementCollectionModel)
            : base(commandAttribute, configurationElementType, elementCollectionModel)
        {
            this.uiService = uiService;
        }

        protected override bool AfterSelectType(Type selectedType)
        {
            var result = this.ElementCollectionModel.ChildElements;

            var count = result.Where(x => ((ExceptionTypeData)x.ConfigurationElement).Type == selectedType).Count();

            if (count > 0)
            {
                uiService.ShowMessageWpf(
                    string.Format(Resources.Culture, Resources.DuplicateExceptionTypeMessage, selectedType.Name),
                    Resources.DuplicationExceptionTypeTitle,
                    System.Windows.MessageBoxButton.OK);
                return false;
            }

            return true;
        }
    }
}
