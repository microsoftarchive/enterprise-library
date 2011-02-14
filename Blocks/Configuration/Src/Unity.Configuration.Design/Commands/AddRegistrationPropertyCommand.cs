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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;

namespace Microsoft.Practices.Unity.Configuration.Design.Commands
{
    public class AddRegistrationPropertyCommand : TemplatedInjectionMemberCommandContainerBase
    {
        InjectionMemberCollectionViewModel collection;
        IApplicationModel applicationModel;

        public AddRegistrationPropertyCommand(CommandAttribute commandAttribute, ElementCollectionViewModel collection, IApplicationModel applicationModel, DefaultCollectionElementAddCommand defaultAddPropertyCommand, IUIServiceWpf uiService)
            : base(commandAttribute, collection, defaultAddPropertyCommand, uiService)
        {
            this.collection = collection as InjectionMemberCollectionViewModel;

            this.applicationModel = applicationModel;
        }

        protected override IEnumerable<TemplatedInjectionMemberCommandBase> GetTemplateCommandsForType(Type registrationType)
        {
            foreach (var property in registrationType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.CanWrite && property.GetIndexParameters().Length == 0)
                {
                    var templatedPropery = new AddTemplatedInjectionPropertyCommand(collection, property, UIService);
                    templatedPropery.DefaultCollectionElementAddCommandInitialization(applicationModel);

                    yield return templatedPropery;
                }
            }
        }
    }
}
