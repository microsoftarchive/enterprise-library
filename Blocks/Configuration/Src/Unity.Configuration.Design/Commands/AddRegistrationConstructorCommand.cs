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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;

namespace Microsoft.Practices.Unity.Configuration.Design.Commands
{
    public class AddRegistrationConstructorCommand : TemplatedInjectionMemberCommandContainerBase
    {

        InjectionMemberCollectionViewModel collection;
        IApplicationModel applicationModel;

        public AddRegistrationConstructorCommand(CommandAttribute commandAttribute, ElementCollectionViewModel collection, IApplicationModel applicationModel, DefaultCollectionElementAddConstructorCommand defaultAddContructorCommand, IUIServiceWpf uiService)
            : base(commandAttribute, collection, defaultAddContructorCommand, uiService)
        {
            this.collection = collection as InjectionMemberCollectionViewModel;
            this.applicationModel = applicationModel;
        }

        protected override IEnumerable<TemplatedInjectionMemberCommandBase> GetTemplateCommandsForType(Type registrationType)
        {
            foreach (var addTemplateConstructor in
                            registrationType
                                    .GetConstructors()
                                    .Where(x => x.GetParameters().Length > 0)
                                    .Select(x => new AddTemplatedRegistrationConstructorCommand(collection, x, UIService)))
            {
                //todo: find a better solution for this.
                addTemplateConstructor.DefaultCollectionElementAddCommandInitialization(applicationModel);
                yield return addTemplateConstructor;
            }
        }


        public static bool InjectionMembersContainsConstructorElement(InjectionMemberCollectionViewModel membersCollection)
        {
            return membersCollection.ChildElements.Where(x => typeof(ConstructorElement).IsAssignableFrom(x.ConfigurationType)).Any();
        }
    }
}
