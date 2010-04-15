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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class AddExceptionPolicyCommand : DefaultCollectionElementAddCommand
    {
        public AddExceptionPolicyCommand(ElementCollectionViewModel collection, IUIServiceWpf uiService)
            : base(new ConfigurationElementType(typeof(ExceptionPolicyData)), collection, uiService)
        {

        }

        protected override void InnerExecute(object parameter)
        {
            base.InnerExecute(parameter);

            if (AddedElementViewModel != null)
            {
                var exceptionTypesElement = (ElementCollectionViewModel)AddedElementViewModel.ChildElement("ExceptionTypes");
                var addedExceptionType = exceptionTypesElement.AddNewCollectionElement(typeof(ExceptionTypeData));

                addedExceptionType.Property("Name").Value = "All Exceptions";
                addedExceptionType.Property("TypeName").Value = typeof(Exception).AssemblyQualifiedName;
            }
        }
    }
#pragma warning restore 1591
}
