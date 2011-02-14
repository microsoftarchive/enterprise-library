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

using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.Unity.Configuration.Design.Commands
{

    public class AddTemplatedRegistrationConstructorCommand : TemplatedInjectionMemberCommandBase
    {
        InjectionMemberCollectionViewModel collection;
        ConstructorInfo constructor;

        public AddTemplatedRegistrationConstructorCommand(InjectionMemberCollectionViewModel collection, ConstructorInfo constructor, IUIServiceWpf uiService)
            : base(new ConfigurationElementType(typeof(ConstructorElement)), collection, uiService)
        {
            this.collection = collection;
            this.constructor = constructor;
        }

        public ConstructorInfo Constructor
        {
            get { return constructor; }
        }

        public override string Title
        {
            get
            {
                return constructor.ToString();
            }
        }

        protected override void InnerExecute(object parameter)
        {
            base.InnerExecute(parameter);

            ElementCollectionViewModel parametersCollection = (ElementCollectionViewModel)base.AddedElementViewModel.ChildElement("Parameters");
            foreach (var parameterInfo in Constructor.GetParameters())
            {
                var parameterElement = parametersCollection.AddNewCollectionElement(typeof(ParameterElement));
                parameterElement.Property("Name").Value = parameterInfo.Name;
                parameterElement.Property("TypeName").Value = parameterInfo.ParameterType.AssemblyQualifiedName;
            }
        }

        protected override bool InnerCanExecute(object parameter)
        {
            return !AddRegistrationConstructorCommand.InjectionMembersContainsConstructorElement(collection);
        }
    }
}
