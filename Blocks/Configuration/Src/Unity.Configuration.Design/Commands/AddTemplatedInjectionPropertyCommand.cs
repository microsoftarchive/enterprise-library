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
    public class AddTemplatedInjectionPropertyCommand : TemplatedInjectionMemberCommandBase
    {
        PropertyInfo property;

        public AddTemplatedInjectionPropertyCommand(ElementCollectionViewModel collection, PropertyInfo property, IUIServiceWpf uiService)
            : base(new ConfigurationElementType(typeof(PropertyElement)), collection, uiService)
        {
            this.property = property;
        }

        public PropertyInfo Property
        {
            get { return property; }
        }

        protected override void InnerExecute(object parameter)
        {
            base.InnerExecute(parameter);

            AddedElementViewModel.Property("Name").Value = property.Name;
        }

        public override string Title
        {
            get
            {
                return Property.ToString();
            }
        }
    }
}
