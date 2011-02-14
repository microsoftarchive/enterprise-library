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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;

namespace Microsoft.Practices.Unity.Configuration.Design.Commands
{
    public class AddRegistrationMethodCommand : TemplatedInjectionMemberCommandContainerBase
    {
        IApplicationModel applicationModel;
        InjectionMemberCollectionViewModel collection;

        public AddRegistrationMethodCommand(CommandAttribute commandAttribute, ElementCollectionViewModel collection, IApplicationModel applicationModel, DefaultCollectionElementAddCommand defaultAddMethodCommand, IUIServiceWpf uiService)
            : base(commandAttribute, collection, defaultAddMethodCommand, uiService)
        {
            this.applicationModel = applicationModel;
            this.collection = collection as InjectionMemberCollectionViewModel;
        }

        protected override IEnumerable<TemplatedInjectionMemberCommandBase> GetTemplateCommandsForType(Type registrationType)
        {
            foreach (var addTemplateMethodCommand in
                                       registrationType
                                               .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                                               .Where(x => x.DeclaringType != typeof(object))
                                               .Where(x => !x.IsSpecialName)
                                               .Select(x => new AddTemplatedRegistrationMethodCommand(collection, x, UIService)))
            {
                //todo: find a better solution for this.
                addTemplateMethodCommand.DefaultCollectionElementAddCommandInitialization(applicationModel);
                yield return addTemplateMethodCommand;
            }
        }
    }
}
