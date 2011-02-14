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
    public class AddTemplatedRegistrationMethodCommand : TemplatedInjectionMemberCommandBase
    {
        InjectionMemberCollectionViewModel collection;
        MethodInfo method;

        public AddTemplatedRegistrationMethodCommand(InjectionMemberCollectionViewModel collection, MethodInfo method, IUIServiceWpf uiService)
            : base(new ConfigurationElementType(typeof(MethodElement)), collection, uiService)
        {
            this.collection = collection;
            this.method = method;
        }

        public MethodInfo Method
        {
            get { return method; }
        }

        protected override void InnerExecute(object parameter)
        {
            base.InnerExecute(parameter);

            base.AddedElementViewModel.Property("Name").Value = Method.Name;

            ElementCollectionViewModel parametersCollection = (ElementCollectionViewModel)base.AddedElementViewModel.ChildElement("Parameters");
            foreach (var parameterInfo in Method.GetParameters())
            {
                var parameterElement = parametersCollection.AddNewCollectionElement(typeof(ParameterElement));
                parameterElement.Property("Name").Value = parameterInfo.Name;
                parameterElement.Property("TypeName").Value = parameterInfo.ParameterType.AssemblyQualifiedName;
            }
        }

        public override string Title
        {
            get
            {
                return method.ToString();
            }
        }
    }
}
