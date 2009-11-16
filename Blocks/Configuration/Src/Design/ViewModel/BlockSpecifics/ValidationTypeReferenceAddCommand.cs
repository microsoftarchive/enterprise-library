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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class ValidationTypeReferenceAddCommand : TypePickingCollectionElementAddCommand
    {
        public ValidationTypeReferenceAddCommand(TypePickingCommandAttribute commandAttribute, ConfigurationElementType configurationElementType, ElementCollectionViewModel elementCollectionModel)
            : base(commandAttribute, configurationElementType, elementCollectionModel)
        {
        }

        protected override void SetProperties(ElementViewModel createdElement, Type selectedType)
        {
            createdElement.Property("Name").Value = selectedType.FullName; 
            createdElement.Property("AssemblyName").Value = selectedType.Assembly.GetName().FullName;
        }
    }
}
